#!/bin/bash

# ---------------------------------------------------------------------------------------------------------
# Python and package managers

# ??? DO WE EVEN NEED THIS?
apt install curl -y

# Install python and pip (python package manager)
apt install python3.7 -y
apt install python3-pip -y

# ---------------------------------------------------------------------------------------------------------
# Web driver

# Install the most up-to-date version of firefox
sudo apt-get install firefox

# Install selenium, necessary for automating webdriver
#python3 -m pip install selenium #### NOT NEEDED?
pip3 install selenium

# Get Firefox webdriver 
webLoc='/home/sw706/webdriver' ######################## USE ENV VAR INSTEAD OF sw706
mkdir -p $webLoc
wget https://github.com/mozilla/geckodriver/releases/download/v0.22.0/geckodriver-v0.22.0-linux64.tar.gz
tar -xzvf geckodriver-v0.22.0-linux64.tar.gz geckodriver
rm geckodriver-v0.22.0-linux64.tar.gz
mv geckodriver $webLoc

chmod +x $webloc/geckodriver

# ---------------------------------------------------------------------------------------------------------
# The DNS's used to resolve any urls are written in the automatically generated file: /etc/resolv.conf
# Here, we install the linux default program to controls what is written in that file
# Then we add the Google public DNS servers to the configuration, ensuring that it is always present
# in the /etc/resolv.conf file
# If we didn't do this, we could not get access to any websites after connecting to our VPN service 

# ??? , because this file would only try to resolve urls locally ????????????????????????????????

apt-get install -y resolvconf
echo "nameserver 8.8.8.8" >> /etc/resolvconf/resolv.conf.d/head
echo "nameserver 8.8.4.4" >> /etc/resolvconf/resolv.conf.d/head

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

chmod +x startVPN.sh

# Install tldextract module for Python3
pip3 install tldextract

# Install the expect interpreter, which allows us to automate the execution of the vpn connection script
sudo apt-get install expect

# BEFORE RUNNING THE BACKEND SCRIPT, RUN THIS: export PATH=$PATH:/home/sw706/webdriver #### REMEMBER ENV VAR
# tHIS APPENDS MY WEBDRIVER PATH TO THE ENV VAR
######## FIND SOME WAY TO MAKE THIS PART OF THE ENV VARS PERMANENTLY
# No longer necessary, probably