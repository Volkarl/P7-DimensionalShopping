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
apt install python-pip

pip install selenium

SELENIUM_STANDALONE_VERSION=3.4.0
SELENIUM_SUBDIR=$(echo "$SELENIUM_STANDALONE_VERSION" | cut -d"." -f-2)
wget -N http://selenium-release.storage.googleapis.com/$SELENIUM_SUBDIR/selenium-server-standalone-$SELENIUM_STANDALONE_VERSION.jar -P ~/
sudo mv -f ~/selenium-server-standalone-$SELENIUM_STANDALONE_VERSION.jar /usr/local/bin/selenium-server-standalone.jar

#chrome
curl -sS -o - https://dl-ssl.google.com/linux/linux_signing_key.pub | apt-key add
echo "deb http://dl.google.com/linux/chrome/deb/ stable main" >> /etc/apt/sources.list.d/google-chrome.list
apt install google-chrome-stable -y

#chromedriver
mkdir ~/webdriver
wget -N http://chromedriver.storage.googleapis.com/$CHROME_DRIVER_VERSION/chromedriver_linux64.zip -P ~/
unzip ~/chromedriver_linux64.zip -d ~/
rm ~/chromedriver_linux64.zip
sudo mv -f ~/chromedriver ~/webdriver
#do path stuff
sudo chown root:root ~/webdriver/chromedriver
sudo chmod 0755 ~/webdriver/chromedriver

