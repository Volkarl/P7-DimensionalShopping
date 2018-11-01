#!/bin/bash

#this guid is uses Ubuntu 17
#run woth sudo


# input argument
# location is taken from this site
# https://account.ipvanish.com/index.php?t=Server+List&page=1
# choose an address
location=$1

password=$2


#install openvpn
apt-get install -y openvpn 

#To create a VPN directory
 mkdir ~/ipvanish

#go to directory
 cd ~/ipvanish 

# download the required IPVanish VPN connectivity script
wget http://files.ipvanish.com/ipvanish-vpn-linux

# assign executable permission to the IPVanish Linux script
chmod +x ipvanish-vpn-linux


# set-up the VPN connection
# Connecting IPVanish VPN
# Enter your IPVanish username: Your IPVanish username
# Enter your IPVanish password: Your IPVanish password
# Enter VPN server name: Select server from our server list.
# Enter VPN protocol: udp
./ipvanish-vpn-linux start <<< thinhar2@gmail.com\ $password\ $location\ udp

