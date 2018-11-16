#!/bin/bash

# Creates and configures TUN/TAP device driver, necessary for creating a connection with openVPN
# See official documentation at https://www.kernel.org/doc/Documentation/networking/tuntap.txt
mkdir -p /dev/net 
mknod /dev/net/tun c 10 200
chmod 666 /dev/net/tun
# Changes permissions to allow all users to read and write (not execute)

/bin/bash 
# Run terminal, because the container will otherwise exit immedeately
# Now we can test directly on the container

# Build with docker build PATHTODOCKERFILE
# For now, run the docker container with: docker run -it --cap-add=NET_ADMIN IDOFDOCKERIMAGE
# Perhaps I can find a way to add --cap-add=NET_ADMIN to this file
# I may be able to remove -it as well, once I don't want to run it in the terminal (perhaps I can already do it now, who knows)

# Then run the query scripts in non-root

# Now we are ready to execute the query.py script
# Missing: initialize connection somehow?
