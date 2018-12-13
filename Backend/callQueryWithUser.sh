#!/bin/bash

url=$1
requestedUserAgent=$2
deleteCookies=$3

pcUsername=$(cat /etc/username)

echo $url $requestedUserAgent $deleteCookies > /home/$pcUsername/queryArguments
sudo -u $pcUsername -i /bin/bash - <<-'EOF'
	~/P7-DimensionalShopping/Backend/query.py $(cat queryArguments)
EOF
# We impersonate the user $pcUsername to ensure we run without root (not allowed while opening webdriver)