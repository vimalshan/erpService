#!/bin/bash

# Approval Service - Build and Test Script

set -e

echo "=== Approval Service Build & Test ==="

# Colors for output
GREEN='\033[0;32m'
RED='\033[0;31m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Check prerequisites
echo -e "${GREEN}Checking prerequisites...${NC}"
commands=("dotnet" "docker" "docker-compose")
for cmd in "${commands[@]}"; do
    if ! command -v $cmd &> /dev/null; then
        echo -e "${RED}✗ $cmd not found${NC}"
        exit 1
    fi
    echo -e "${GREEN}✓ $cmd found${NC}"
done

# Start dependencies
echo -e "${GREEN}Starting dependencies...${NC}"
docker-compose up -d
sleep 10

# Build solution
echo -e "${GREEN}Building solution...${NC}"
dotnet build ApprovalService.sln
if [ $? -ne 0 ]; then
    echo -e "${RED}Build failed!${NC}"
    exit 1
fi
echo -e "${GREEN}Build successful!${NC}"

# Run tests
echo -e "${GREEN}Running tests...${NC}"
find src -name "*.Tests.csproj" -type f | while read project; do
    echo -e "${YELLOW}Testing $project${NC}"
    dotnet test "$project" --logger "console;verbosity=minimal"
    if [ $? -ne 0 ]; then
        echo -e "${RED}Tests failed!${NC}"
        exit 1
    fi
done

# Run API
echo -e "${GREEN}Starting API...${NC}"
cd src/ApprovalService.API
dotnet run
