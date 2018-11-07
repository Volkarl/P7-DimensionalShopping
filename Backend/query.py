#!/usr/bin/env python3
import sys, subprocess, os
from selenium import webdriver
from selenium.webdriver.firefox.options import Options
from switchUserAgent import getUserAgentString
from crawler import crawlUrl

url = sys.argv[1]
requestedUserAgent = sys.argv[2]
deleteCookies = sys.argv[3]
location = sys.argv[4] # Valid locations are found using this site: https://account.ipvanish.com/index.php?t=Server+List&page=1
ipvanishEmail = sys.argv[5]
ipvanishPassword = sys.argv[6]
pcUsername = sys.argv[7]

def bashCall(cmdString):
    subprocess.call(cmdString, shell=True)

def isUserRoot():
	return os.geteuid() == 0

if isUserRoot():
    exit("Running with root privileges is not allowed. Exiting.")
    # Root is not allowed when running browsers (risky)
    # I couldn't downgrade privileges, so this was the best alternative

# Options and user agent string
options = Options()
options.headless = True
userAgent = getUserAgentString(requestedUserAgent)
print(f'Configuring user agent: {userAgent}')
options.add_argument(f'user-agent={userAgent}')

# There is no logging for the openVPN opening the VPN connection. The way it can be done is running adding the command: 
# 'echo "log /home/sw706/ipvanish/openvpn.log" >> /home/sw706/ipvanish/openvpn.conf' after the "ipvanish-linux" script
# has run at least once (this is the script that is run within "startVPN.exp") and then only initializing the VPN 
# connection with the command: "sudo openvpn --config /home/sw706/ipvanish/openvpn.conf"
# The problem here is the fact that the "openvpn-linux" script automatically downloads a new configuration file for 
# openvpn every time it is run (one that doesn't mention logging) and then also initializes the connection through
# openvpn. This makes it so we cannot see the reason the VPN connection fails (for instance with mistyped IP).

# Initialize VPN Connection
homeDir = f'/home/{pcUsername}'

# Calls an expect script with a bash subshell that enters our ipvanish user information whenever it is prompted
bashCall(f'sudo {homeDir}/P7-DimensionalShopping/Backend/startVPN.exp {location} {ipvanishEmail} {ipvanishPassword}')

# Initialize
driver = webdriver.Firefox(executable_path = f'{homeDir}/webdriver/geckodriver', options = options)

# Cookies
if deleteCookies: 
	print("Deleting Cookies")
	driver.delete_all_cookies()
#### Is this necessary? I feel like we're not using any cookies anyway, since we open a new webdriver each request

# Get website
driver.get(url)

# Crawl HTML of website
result = crawlUrl(driver, url)
print(result)

# Terminate VPN connection and selenium session
bashCall(f'sudo {homeDir}/ipvanish/ipvanish-vpn-linux stop')
driver.quit()

## Perhaps add graceful termination with try-catch that calls my terminate commands