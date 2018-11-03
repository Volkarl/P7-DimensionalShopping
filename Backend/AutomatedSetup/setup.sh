#!/bin/bash

# ---------------------------------------------------------------------------------------------------------
# Python and package managers

# Update apt (package manager)
apt update
apt install curl -y

# Install python and pip (python package manager)
apt install python3.7 -y
apt install python3-pip -y
#apt install python-pip -y # WHAT IS THE DIFFERENCE TO THE ABOVE? THIS DOESNT SEEM TO WORK WITH PYTHON 3

# ---------------------------------------------------------------------------------------------------------
# Web driver

# To ensure it's updated
sudo apt-get install firefox

# Install selenium, necessary for automating webdriver
python3 -m pip install selenium
#pip install selenium ######### doesnt work for python 3
pip3 install selenium

# Get Firefox webdriver 
webLoc='/home/sw706/webdriver' ######################## USE ENV VAR INSTEAD OF sw706
mkdir -p $webLoc
wget https://github.com/mozilla/geckodriver/releases/download/v0.22.0/geckodriver-v0.22.0-linux64.tar.gz
tar -xzvf geckodriver-v0.22.0-linux64.tar.gz geckodriver
rm geckodriver-v0.22.0-linux64.tar.gz
mv geckodriver $webLoc

chmod +x /home/sw706/webdriver/geckodriver ### ENV VAR

# ---------------------------------------------------------------------------------------------------------
# Install openVPN and set it up for use with IPVanish
apt-get install -y openvpn 
mkdir ~/ipvanish

# Download the provided IPVanish VPN connectivity script
wget http://files.ipvanish.com/ipvanish-vpn-linux -P ~/ipvanish 
# Assign executable permission
chmod +x ~/ipvanish/ipvanish-vpn-linux

# ---------------------------------------------------------------------------------------------------------
# Extra packages necessary for the backend script

#chmod +x startVPN.sh
######### DOESNT SEEM NECESSARY

#pip install tldextract doesnt work for p3
pip3 install tldextract

# Install the expect interpreter, which allows us to automate the execution of the vpn connection script
sudo apt-get install expect

# BEFORE RUNNING THE BACKEND SCRIPT, RUN THIS: export PATH=$PATH:/home/sw706/webdriver #### REMEMBER ENV VAR
# tHIS APPENDS MY WEBDRIVER PATH TO THE ENV VAR
######## FIND SOME WAY TO MAKE THIS PART OF THE ENV VARS PERMANENTLY