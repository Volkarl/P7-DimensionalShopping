#!/bin/bash
#CHROME_DRIVER_VERSION=2.42
# update apt (package manager)
apt update
apt install curl -y
# install stuff with apt
apt install python3.7 -y
# use pip(package manager) to install selenium
apt install python3-pip -y
python3 -m pip install selenium

#webdriver 
webLoc='/home/sw706e18/webdriver'
# firefoxwebdriver
mkdir -p $webLoc
wget https://github.com/mozilla/geckodriver/releases/download/v0.22.0/geckodriver-v0.22.0-linux64.tar.gz
tar -xzvf geckodriver-v0.22.0-linux64.tar.gz geckodriver
rm geckodriver-v0.22.0-linux64.tar.gz
mv geckodriver $webLoc

#do meta stuff
chmod +x /home/sw706e18/webdriver/geckodriver
