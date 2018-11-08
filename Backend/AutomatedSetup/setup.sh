#!/bin/bash

# This script needs to be run with sudo

username=$1

# ---------------------------------------------------------------------------------------------------------
# Install python and pip (python package manager)
apt-get install wget -y

apt-get install python3.7 -y
apt-get install python3-pip -y

# ---------------------------------------------------------------------------------------------------------
# Web driver

# Install the most up-to-date version of firefox
apt-get install firefox -y

# Install selenium, necessary for automating webdriver
pip3 install selenium

# Get Firefox webdriver 
webLoc='./webdriver'
mkdir -p $webLoc
wget "https://github.com/mozilla/geckodriver/releases/download/v0.23.0/geckodriver-v0.23.0-linux64.tar.gz"
tar -xzvf geckodriver-v0.23.0-linux64.tar.gz geckodriver
rm geckodriver-v0.23.0-linux64.tar.gz
mv geckodriver $webLoc
chmod +x $webLoc/geckodriver

# ---------------------------------------------------------------------------------------------------------
# DNS Settings
# The DNS's used to resolve any urls are written in the automatically generated file: /etc/resolv.conf
# Here, we install the linux default program to controls what is written in that file
# Then we add the Google public DNS servers to the configuration, ensuring that it is always present
# in the /etc/resolv.conf file
# If we didn't do this, we could not get access to any websites after connecting to our VPN service 

apt-get install -y resolvconf
echo "nameserver 8.8.8.8" >> /etc/resolvconf/resolv.conf.d/head
echo "nameserver 8.8.4.4" >> /etc/resolvconf/resolv.conf.d/head

# ---------------------------------------------------------------------------------------------------------
# Install openVPN and set it up for use with IPVanish

apt-get install -y openvpn
vanishLoc='./ipvanish'
mkdir $vanishLoc

# Download the provided IPVanish VPN connectivity script
wget "http://files.ipvanish.com/ipvanish-vpn-linux" -P $vanishLoc 
chmod +x $vanishLoc/ipvanish-vpn-linux

# These are needed for the ipvanish-vpn-linux script to run
apt-get install sudo -y
apt-get install iputils-ping -y
apt-get install host -y

# Creates and configures TUN/TAP device driver, necessary for creating a connection with openVPN
# See official documentation at https://www.kernel.org/doc/Documentation/networking/tuntap.txt
mkdir /dev/net > /dev/null 2>&1
mknod /dev/net/tun c 10 200 > /dev/null 2>&1
# Both have their stdout and stderr piped to /dev/null, if they fail because the files already exist
# (Which they do, unless the OS is a minimal Ubuntu installation)
chmod 666 /dev/net/tun
# Changed permission to allow all users to read and write (not execute)

# ---------------------------------------------------------------------------------------------------------
# Extra packages and commands necessary to run the backend script

backendLoc='./P7-DimensionalShopping/Backend'
# Assign execute permissions to the backend scripts
chmod +x $backendLoc/query.py
chmod +x $backendLoc/startVPN.exp

# Install tldextract for analyzing URL
pip3 install tldextract
# Install the expect interpreter, which allows us to automate the execution of the vpn connection script
apt-get install expect -y

# Ensures that this user (sw706) can use sudo without having to supply a password
# This is needed because the query.py script calls a number of subshells, which 
# need to be in sudo. The script itself cannot be run in sudo, otherwise the
# webdriver won't work (running browsers in sudo is dangerous)
echo "$username ALL=(ALL) NOPASSWD: ALL" >> /etc/sudoers

##### REEEEEEEEEEEEEEEEEEEEEEEEEEee
########## ENSURE THIS ONE IS ACTUALLY RUN, BECAUSE IT WASNT FIRST I TESTED
