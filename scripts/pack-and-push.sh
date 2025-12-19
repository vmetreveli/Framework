#!/usr/bin/env bash
set -euo pipefail
REPO_ROOT="$(cd "$(dirname "$0")/.." && pwd)"
cd "$REPO_ROOT"

# Ensure dotnet is installed
if ! command -v dotnet >/dev/null 2>&1; then
  echo "dotnet CLI not found in PATH. Install .NET SDK and retry. Aborting."
  exit 1
fi

# Parse args
VERSION=""
DRY_RUN=0
SIGN=0
while [[ $# -gt 0 ]]; do
  case "$1" in
    --version) VERSION="$2"; shift 2;;
    --dry-run) DRY_RUN=1; shift;;
    --sign) SIGN=1; shift;;
    --help) echo "Usage: $0 [--version X.Y.Z] [--dry-run] [--sign]"; exit 0;;
    *) echo "Unknown arg $1"; exit 1;;
  esac
done

if [[ -z "$VERSION" ]]; then
  if git describe --tags --abbrev=0 >/dev/null 2>&1; then
    TAG=$(git describe --tags --abbrev=0)
    VERSION="${TAG#v}"
  else
    echo "No tag found and --version not provided; set VERSION env or tag a release (vX.Y.Z)."
    exit 1
  fi
fi

echo "Using version: $VERSION"

mkdir -p .nupkgs
dotnet restore
dotnet build --configuration Release
# Use relative PackageReadmeFile so it matches the README included by Directory.Build.props (pack will include the file into the nupkg)
# Do NOT pass an absolute path here because NuGet will expect that exact path to be present in the package.
dotnet pack --configuration Release --no-build -p:PackageVersion="$VERSION" -p:PackageReadmeFile="README.md" -o .nupkgs

if [[ $DRY_RUN -eq 1 ]]; then
  echo "Dry run: packages produced in .nupkgs"
  ls -la .nupkgs
  exit 0
fi

# Optional signing step: if --sign provided, the following environment variables must be set:
# NUGET_SIGNING_CERT_PATH - path to the .pfx certificate used for signing
# NUGET_SIGNING_CERT_PASSWORD - password for the .pfx
# Optional: NUGET_TIMESTAMP_URL - timestamper URL to pass to the sign command
if [[ $SIGN -eq 1 ]]; then
  if [[ -z "${NUGET_SIGNING_CERT_PATH:-}" || -z "${NUGET_SIGNING_CERT_PASSWORD:-}" ]]; then
    echo "Signing requested but NUGET_SIGNING_CERT_PATH or NUGET_SIGNING_CERT_PASSWORD is not set. Aborting."
    exit 1
  fi

  # Ensure dotnet nuget sign is available
  if ! dotnet nuget sign --help >/dev/null 2>&1; then
    echo "'dotnet nuget sign' is not available in this SDK. Ensure you have a recent .NET SDK that includes 'dotnet nuget sign' (or use nuget.exe). Aborting."
    exit 1
  fi

  for n in .nupkgs/*.nupkg; do
    if [[ -f "$n" ]]; then
      echo "Signing $n..."
      TIMESTAMP_OPT=( )
      if [[ -n "${NUGET_TIMESTAMP_URL:-}" ]]; then
        TIMESTAMP_OPT=(--timestamper "${NUGET_TIMESTAMP_URL}")
      fi

      # Use dotnet nuget sign to sign the package in-place
      dotnet nuget sign "$n" --certificate-path "${NUGET_SIGNING_CERT_PATH}" --certificate-password "${NUGET_SIGNING_CERT_PASSWORD}" "${TIMESTAMP_OPT[@]}"

      echo "Verifying signatures for $n..."
      dotnet nuget verify -Signatures "$n"
    fi
  done
fi

if [[ -z "${NUGET_API_KEY:-}" ]]; then
  echo "Environment variable NUGET_API_KEY is not set. Aborting."
  exit 1
fi

for n in .nupkgs/*.nupkg; do
  if [[ -f "$n" ]]; then
    echo "Pushing $n..."
    dotnet nuget push "$n" -k "$NUGET_API_KEY" -s https://api.nuget.org/v3/index.json --skip-duplicate
  fi
done

