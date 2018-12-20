# Script that installs necessary dependencies for building the project
# From website: https://blogs.taiga.nl/martijn/2018/06/14/lean-asp-net-core-2-1-manually-setup-a-razor-pages-project-with-bootstrap-npm-and-webpack/

# BEFORE RUNNING THE SCRIPT -------------------------------------------------------------
# 1. Download NodeJS from their website: https://nodejs.org/en/download/
# 2. Create an account for NPM on their website: https://www.npmjs.com/
# 3. Confirm your email address
# 4. (If not done) Clone the git repository: "https://github.com/Volkarl/P7-DimensionalShopping.git"
# 5. Set working directory (cd) to the local repository folder, named P7-DimensionalShopping by default


# START ---------------------------------------------------------------------------------
npm install
# This installs the node package manager and is the reason we installed NodeJS in the first place
npm login
# Login with your credentials for NPM

cd .\Application\DimensionalPriceRunner\DimensionalPriceRunner\ClientApp\
npm init -y
npm install --save jquery popper.js bootstrap
npm install --save-dev webpack webpack-cli style-loader css-loader
npm install --save-dev aspnet-webpack webpack-dev-middleware webpack-hot-middleware
npm install --save-dev mini-css-extract-plugin
npm install --save-dev copy-webpack-plugin
# Downloads the necessary packages required for building the project with webpack
# Additionally, downloads jquery and bootstrap

