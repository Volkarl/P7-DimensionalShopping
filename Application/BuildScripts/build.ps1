// Script for building the project

// BEFORE RUNNING THE SCRIPT -------------------------------------------------------------
// 1. Ensure you've got the necessary dependencies for building the project by following instructions in (and then executing) "setup.ps1"
// 2. Set working directory (cd) to the local repository folder, named P7-DimensionalShopping by default

cd .\Application\DimensionalPriceRunner\DimensionalPriceRunner
// Navigates to the project root, where the .csproj file is placed 
dotnet watch run 
// "watch" ensures that whenever the project is saved, the page on the web can just be refreshed to see changes
// As such, you can run this without "watch", it'll just only use the snapshot at the time of build

// See the result on a browser at: "https://localhost:5001/"
