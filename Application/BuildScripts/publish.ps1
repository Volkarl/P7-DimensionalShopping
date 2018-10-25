# Script for publishing the project

# BEFORE RUNNING THE SCRIPT -------------------------------------------------------------
# 1. Ensure you've got the necessary dependencies for building the project by following instructions in (and then executing) "projectSetupScript.ps1"
# 2. Set working directory (cd) to the local repository folder, named P7-DimensionalShopping by default

cd .\Application\DimensionalPriceRunner\DimensionalPriceRunner
# Navigates to the project root, where the .csproj file is placed 

dotnet publish && dotnet ./bin/Debug/netcoreapp2.1/publish/DimensionalPriceRunner.dll

# NOTE: "dotnet publish" cannot seem to include the style sheet when we call it from solution root (where the .sln file is placed). However, when called in project root (where .csproj file is), it works properly. 
