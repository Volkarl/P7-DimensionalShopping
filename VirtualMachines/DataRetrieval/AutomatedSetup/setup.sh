#!/bin/bash
CHROME_DRIVER_VERSION=2.42
# download and install chocolate (package manager)
apt update
apt install curl -y
# install stuff with chocolate
apt install p7zip-full -y
apt install python3.7 -y

# apt does path?

# use pip(package manager) to install selenium
apt install python3-pip -y

#chrome
python3 -m pip install selenium

webLoc = /home/sw706e18/webdriver
#chromedriver
mkdir $webLoc
#wget -N http://chromedriver.storage.googleapis.com/$CHROME_DRIVER_VERSION/chromedriver_linux64.zip -P ~/
wget https://github.com/mozilla/geckodriver/releases/download/v0.22.0/geckodriver-v0.22.0-arm7hf.tar.gz -P $webLoc
#unzip ~/chromedriver_linux64.zip -d ~/
#rm ~/chromedriver_linux64.zip
#sudo mv -f ~/chromedriver ~/webdriver

#do path stuff
export PATH=$PATH:$webLoc
chown -R sw706e18:sw706e18 /home/sw706e18
chmod +x /home/sw706e18/webdriver/chromedriver
