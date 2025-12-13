#!/usr/bin/env bash
set -euo pipefail

# Packs the Framework.Infrastructure project into ./artifacts
ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
PROJECT_PATH="$ROOT_DIR/Framework.Infrastructure/Framework.Infrastructure.csproj"
OUTPUT_DIR="$ROOT_DIR/artifacts"
mkdir -p "$OUTPUT_DIR"

# Check dotnet SDK version
DOTNET_VERSION_RAW=$(dotnet --version 2>/dev/null || true)
if [ -z "$DOTNET_VERSION_RAW" ]; then
  echo "dotnet CLI not found. Please install .NET 10 SDK from https://aka.ms/dotnet/download" >&2
  exit 1
fi

# Accept 10.x versions only
if [[ ! "$DOTNET_VERSION_RAW" =~ ^9\.[0-9]+ ]]; then
  echo "Warning: Installed dotnet SDK version is $DOTNET_VERSION_RAW." >&2
  echo "This project targets net9.0. Install the .NET 9 SDK (dotnet 9.x) or change the TargetFramework in the csproj." >&2
  echo "Proceeding may fail. Continue? (y/N)"
  read -r yn
  if [[ "$yn" != "y" && "$yn" != "Y" ]]; then
    exit 1
  fi
fi

dotnet restore "$PROJECT_PATH"
dotnet pack "$PROJECT_PATH" -c Release -o "$OUTPUT_DIR" /p:IncludeSymbols=true /p:SymbolPackageFormat=snupkg

echo "Packages created in: $OUTPUT_DIR"
ls -la "$OUTPUT_DIR" || true
