#!/bin/bash

# Takes as input a string of the like: 
url=$1
requestedUserAgent=$2
deleteCookies=$3
location=$4
ipvanishEmail=$5
ipvanishPassword=$6
pcUsername=$7

# Creates and configures TUN/TAP device driver, necessary for creating a connection with openVPN
# See official documentation at https://www.kernel.org/doc/Documentation/networking/tuntap.txt
mkdir -p /dev/net 
mknod /dev/net/tun c 10 200
chmod 666 /dev/net/tun
# Changes permissions to allow all users to read and write (not execute)

mv /usr/bin/nping /usr/bin/ping ##### maybe /usr/ping instead
# Renames the nping command to ping, necessary to get the ipvanish-vpn-linux script to run nping instead of the normal ping that fails

echo $url $requestedUserAgent $deleteCookies $location $ipvanishEmail $ipvanishPassword $pcUsername > /home/$pcUsername/arguments
sudo -u $pcUsername -i /bin/bash - <<-'EOF'
	~/P7-DimensionalShopping/Backend/query.py $(cat arguments)
EOF
# We impersonate the user $pcUsername (which ensures that our home directory ~ points to the correct location)

#/home/$pcUsername instead of ~ ?

#tail -f /dev/null
# Runs forever

#/bin/bash 
# Run terminal, because the container will otherwise exit immedeately
# Now we can test directly on the container

# Build with docker build PATHTODOCKERFILE
# For now, run the docker container with: docker run -it --cap-add=NET_ADMIN IDOFDOCKERIMAGE
# Perhaps I can find a way to add --cap-add=NET_ADMIN to this file
# I may be able to remove -it as well, once I don't want to run it in the terminal (perhaps I can already do it now, who knows)

# Then run the query scripts in non-root

# Now we are ready to execute the query.py script
# Missing: initialize connection somehow?
