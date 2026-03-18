#!/bin/bash
# Build script for ReferenceService

echo "Building ReferenceService..."

# Set .NET version
export DOTNET_ROOT=$(dotnet --version 2>/dev/null | head -1)

# Restore NuGet packages
echo "Restoring NuGet packages..."
dotnet restore src/ReferenceService.slnx

# Build solution
echo "Building solution..."
dotnet build src/ReferenceService.slnx --configuration Release --no-restore

if [ $? -eq 0 ]; then
    echo "Build completed successfully!"
    exit 0
else
    echo "Build failed!"
    exit 1
fi
