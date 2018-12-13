#!/bin/bash
location=$1

# Creates and configures TUN/TAP device driver, necessary for creating a connection with openVPN
# See official documentation at https://www.kernel.org/doc/Documentation/networking/tuntap.txt
mkdir -p /dev/net 
mknod /dev/net/tun c 10 200
chmod 666 /dev/net/tun
# Changes permissions to allow all users to read and write (not execute)

pcUsername=$(cat /etc/username)

echo $location $(cat /etc/ipvanish/email) $(cat /etc/ipvanish/password) $pcUsername > /home/$pcUsername/arguments
sudo -u $pcUsername -i /bin/bash - <<-'EOF'
	~/P7-DimensionalShopping/Backend/startVPN.exp $(cat arguments)
EOF
# We impersonate the user $pcUsername (which ensures that our home directory ~ points to the correct location)
# Connects to the VPN location

touch /tmp/ready
# Marks the container as ready for traffic

node /home/$pcUsername/P7-DimensionalShopping/Backend/nodejs/server.js
# Starts the node server
