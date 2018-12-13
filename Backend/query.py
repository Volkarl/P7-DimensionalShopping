#!/usr/bin/env python3
import sys, subprocess, os, time
from selenium import webdriver
from selenium.webdriver.firefox.options import Options
from switchUserAgent import getUserAgentString
from ipvanishServers import getServerURL
from crawler import crawlUrl

url = sys.argv[1]
requestedUserAgent = sys.argv[2]
deleteCookies = sys.argv[3]
pcUsername = sys.argv[4]

def bashCall(cmdString):
    subprocess.call(cmdString, shell=True)

def isUserRoot():
	return os.geteuid() == 0

if isUserRoot():
	# bashCall(f'sudo -su {pcUsername} query.py {url} {requestedUserAgent} {deleteCookies} {locationURL} {ipvanishEmail} {ipvanishPassword} {pcUsername}')
    # The above bashCall may be a solution, but, whatever: just don't run it with root to begin with
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

homeDir = f'/home/{pcUsername}'
# We use full path to ensure the files can be found where we expect

# Initialize
driver = webdriver.Firefox(executable_path = f'{homeDir}/webdriver/geckodriver', options = options)
# time.sleep(3)
# Sleeping 3 seconds helps (mostly) fix a race condition within selenium (marionette). A stupid workaround for an incomprehensible problem.
# Sometimes it still fails with: "Failed to decode marionette", but now less so than previously. What the fuck. 

# Cookies
if deleteCookies: 
	print("Deleting Cookies")
	driver.delete_all_cookies()

# Get website
driver.get(url)

# Crawl HTML of website
result = crawlUrl(driver, url)
print(result)

# Terminate VPN connection and selenium session
driver.quit()

## Perhaps add graceful termination with try-catch that calls my terminate commands