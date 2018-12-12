#!/bin/bash

# Creates and configures TUN/TAP device driver, necessary for creating a connection with openVPN
# See official documentation at https://www.kernel.org/doc/Documentation/networking/tuntap.txt
mkdir -p /dev/net 
mknod /dev/net/tun c 10 200
chmod 666 /dev/net/tun
# Changes permissions to allow all users to read and write (not execute)

touch /tmp/ready
# Marks the container as ready for traffic

node /home/$(cat /etc/username)/P7-DimensionalShopping/Backend/nodejs/server.js
# Starts the node server
