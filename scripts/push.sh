#!/usr/bin/env bash
set -euo pipefail

# Pushes all .nupkg and .snupkg files from ./artifacts to a NuGet feed
# Usage: ./scripts/push.sh [--source <sourceUrl>]
# If NUGET_API_KEY is not set in the environment, the script will prompt for it safely.

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
ARTIFACTS_DIR="$ROOT_DIR/artifacts"
SOURCE_URL="https://api.nuget.org/v3/index.json"

# simple arg parsing for --source
if [[ ${1-} == "--source" ]]; then
  if [[ -z ${2-} ]]; then
    echo "Usage: $0 --source <sourceUrl>" >&2
    exit 2
  fi
  SOURCE_URL="$2"
fi

if [[ -z "${NUGET_API_KEY-}" ]]; then
  # Prompt for API key without echoing to terminal
  echo "NUGET_API_KEY is not set. You can paste it now (it will not be stored)."
  read -s -p "Enter NuGet API key: " ENTERED_KEY
  echo
  if [[ -z "$ENTERED_KEY" ]]; then
    echo "No API key provided. Aborting." >&2
    exit 1
  fi
  # Export for this script only
  export NUGET_API_KEY="$ENTERED_KEY"
  # We will unset it before exiting
  UNSET_AFTER=1
else
  UNSET_AFTER=0
fi

shopt -s nullglob
PACKAGE_FILES=("$ARTIFACTS_DIR"/*.nupkg "$ARTIFACTS_DIR"/*.snupkg)
if [[ ${#PACKAGE_FILES[@]} -eq 0 ]]; then
  echo "No packages found in $ARTIFACTS_DIR. Run ./scripts/pack.sh first." >&2
  if [[ $UNSET_AFTER -eq 1 ]]; then unset NUGET_API_KEY; fi
  exit 1
fi

# Print list and ask for confirmation
echo "Found ${#PACKAGE_FILES[@]} package(s) to push:"
for p in "${PACKAGE_FILES[@]}"; do echo "  - $p"; done

read -p "Push these packages to $SOURCE_URL? (y/N) " confirm
if [[ "$confirm" != "y" && "$confirm" != "Y" ]]; then
  echo "Aborted by user."
  if [[ $UNSET_AFTER -eq 1 ]]; then unset NUGET_API_KEY; fi
  exit 0
fi

for pkg in "${PACKAGE_FILES[@]}"; do
  echo "Pushing $pkg to $SOURCE_URL"
  dotnet nuget push "$pkg" --api-key "$NUGET_API_KEY" --source "$SOURCE_URL" --skip-duplicate || true
done

# Remove API key from environment for safety if we read it in this run
if [[ $UNSET_AFTER -eq 1 ]]; then
  unset NUGET_API_KEY
fi

echo "Done."

