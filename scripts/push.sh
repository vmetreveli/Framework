#!/usr/bin/env bash
set -euo pipefail

# Pushes all .nupkg and .snupkg files from ./artifacts to NuGet.org
# Usage: NUGET_API_KEY=... ./scripts/push.sh [--source https://api.nuget.org/v3/index.json]

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
ARTIFACTS_DIR="$ROOT_DIR/artifacts"
SOURCE_URL="https://api.nuget.org/v3/index.json"

if [[ ${1-} == "--source" ]]; then
  SOURCE_URL="$2"
fi

if [[ -z "${NUGET_API_KEY-}" ]]; then
  echo "Environment variable NUGET_API_KEY is required to push packages." >&2
  exit 1
fi

shopt -s nullglob
PACKAGE_FILES=("$ARTIFACTS_DIR"/*.nupkg "$ARTIFACTS_DIR"/*.snupkg)
if [[ ${#PACKAGE_FILES[@]} -eq 0 ]]; then
  echo "No packages found in $ARTIFACTS_DIR. Run ./scripts/pack.sh first." >&2
  exit 1
fi

for pkg in "${PACKAGE_FILES[@]}"; do
  echo "Pushing $pkg to $SOURCE_URL"
  dotnet nuget push "$pkg" --api-key "$NUGET_API_KEY" --source "$SOURCE_URL" --skip-duplicate || true
done

echo "Done."

