#!/bin/bash

# Takes pcUsername as input from entrypoint, as defined in dockerfile or kubernetes configuration
pcUsername=$1

echo pcUsername > /etc/pcUsername 
# Saves pcUsername in file, so that we can read it from the file "callQuery.sh", and know where our openVPN files are placed

# Creates and configures TUN/TAP device driver, necessary for creating a connection with openVPN
# See official documentation at https://www.kernel.org/doc/Documentation/networking/tuntap.txt
mkdir -p /dev/net 
mknod /dev/net/tun c 10 200
chmod 666 /dev/net/tun
# Changes permissions to allow all users to read and write (not execute)

# node /home/$pcUsername/P7-DimensionalShopping/Backend/nodejs/server.js
/bin/bash