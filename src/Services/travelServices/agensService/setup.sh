#!/bin/bash

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

echo -e "${YELLOW}=====================================${NC}"
echo -e "${YELLOW}Agency Service Setup Script${NC}"
echo -e "${YELLOW}=====================================${NC}"

# Check prerequisites
echo -e "\n${YELLOW}Checking prerequisites...${NC}"

# Check .NET SDK
if ! command -v dotnet &> /dev/null; then
    echo -e "${RED}✗ .NET SDK not found. Please install .NET 8 SDK${NC}"
    exit 1
fi
echo -e "${GREEN}✓ .NET SDK installed${NC}"

# Check SQL Server
echo -e "\n${YELLOW}Setting up database...${NC}"

# Restore dependencies
echo -e "\n${YELLOW}Restoring NuGet packages...${NC}"
dotnet restore
if [ $? -eq 0 ]; then
    echo -e "${GREEN}✓ Dependencies restored${NC}"
else
    echo -e "${RED}✗ Failed to restore dependencies${NC}"
    exit 1
fi

# Build solution
echo -e "\n${YELLOW}Building solution...${NC}"
dotnet build -c Release
if [ $? -eq 0 ]; then
    echo -e "${GREEN}✓ Solution built successfully${NC}"
else
    echo -e "${RED}✗ Build failed${NC}"
    exit 1
fi

# Run migrations
echo -e "\n${YELLOW}Running database migrations...${NC}"
cd src/API/AgencyService.Api
dotnet ef database update --project ../../Infrastructure/AgencyService.Infrastructure
if [ $? -eq 0 ]; then
    echo -e "${GREEN}✓ Database migrations completed${NC}"
else
    echo -e "${YELLOW}! Database migrations skipped (ensure database is up to date)${NC}"
fi
cd ../../..

# Success message
echo -e "\n${GREEN}=====================================${NC}"
echo -e "${GREEN}Setup completed successfully!${NC}"
echo -e "${GREEN}=====================================${NC}"

echo -e "\n${YELLOW}Next steps:${NC}"
echo "1. (Optional) Start Docker containers: docker-compose up -d"
echo "2. Run the API: dotnet run --project src/API/AgencyService.Api"
echo "3. Access Swagger: http://localhost:5000/swagger/index.html"
echo "4. Access GraphQL: http://localhost:5000/graphql"
