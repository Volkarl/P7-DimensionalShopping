#!/bin/bash

# Takes as input a string of the like: 
url=$1
requestedUserAgent=$2
deleteCookies=$3
location=$4
pcUsername=$5

# Creates and configures TUN/TAP device driver, necessary for creating a connection with openVPN
# See official documentation at https://www.kernel.org/doc/Documentation/networking/tuntap.txt
mkdir -p /dev/net 
mknod /dev/net/tun c 10 200
chmod 666 /dev/net/tun
# Changes permissions to allow all users to read and write (not execute)

echo $url $requestedUserAgent $deleteCookies $location $(cat /etc/ipvanish/email) $(cat /etc/ipvanish/password) $pcUsername > /home/$pcUsername/arguments
sudo -u $pcUsername -i /bin/bash - <<-'EOF'
	~/P7-DimensionalShopping/Backend/query.py $(cat arguments)
EOF
# We impersonate the user $pcUsername (which ensures that our home directory ~ points to the correct location)
