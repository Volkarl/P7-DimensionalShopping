#!/bin/bash

# Location is found using this site: https://account.ipvanish.com/index.php?t=Server+List&page=1
location=$1
password=$2

# Uses a "here string" (the <<< symbol) to redirect our parameters (pass, location and vpn protocol) to the script
# This is necessary because the script uses "read" three times to read from standard input, expecting the user to input the values by hand

cd ~/ipvanish
./ipvanish-vpn-linux start <<< "thinhar2@gmail.com"\ $password\ $location\ "udp"
