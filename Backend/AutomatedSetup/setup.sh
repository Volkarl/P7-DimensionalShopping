#!/bin/bash

# ---------------------------------------------------------------------------------------------------------
# Python and package managers

# Update apt (package manager)
apt update
apt install curl -y

# Install python and pip (python package manager)
apt install python3.7 -y
apt install python3-pip -y

# ---------------------------------------------------------------------------------------------------------
# Web driver

# Install selenium, necessary for automating webdriver
python3 -m pip install selenium

# Get Firefox webdriver 
webLoc='/home/sw706e18/webdriver'
mkdir -p $webLoc
wget https://github.com/mozilla/geckodriver/releases/download/v0.22.0/geckodriver-v0.22.0-linux64.tar.gz
tar -xzvf geckodriver-v0.22.0-linux64.tar.gz geckodriver
rm geckodriver-v0.22.0-linux64.tar.gz
mv geckodriver $webLoc

chmod +x /home/sw706e18/webdriver/geckodriver

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
pip install tldextract

# BEFORE RUNNING THE BACKEND SCRIPT, RUN THIS: export PATH=$PATH:/home/sw706e18/webdriver 