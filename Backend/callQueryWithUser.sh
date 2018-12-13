#!/bin/bash

url=$1
requestedUserAgent=$2
deleteCookies=$3
location=$4
email=$5
password=$6
pcUsername=$7

echo $url $requestedUserAgent $deleteCookies $location $email $password $pcUsername > /home/$pcUsername/arguments
sudo -u $pcUsername -i /bin/bash - <<-'EOF'
	~/P7-DimensionalShopping/Backend/query.py $(cat arguments)
EOF
# We impersonate the user $pcUsername (which ensures that our home directory ~ points to the correct location)
