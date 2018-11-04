#!/usr/bin/env python3
import sys, subprocess, os
from selenium import webdriver
from selenium.webdriver.firefox.options import Options
from switchUserAgent import getUserAgentString
from crawler import crawlUrl

url = sys.argv[1]
requestedUserAgent = sys.argv[2]
deleteCookies = sys.argv[3]
location = sys.argv[4] # Location is found using this site: https://account.ipvanish.com/index.php?t=Server+List&page=1
password = sys.argv[5]

def bashCall(cmdString):
    subprocess.call(cmdString, shell=True)

def isUserRoot():
	return os.geteuid() == 0

#def runWithNonRoot(cmdLambda):
#	pid = os.fork()
#	if pid == 0:
#		# We are now in the non-root sub-process
#		try: 
#			os.seteuid(1000) # 1000 is a non-root user
#			cmdLambda()
#		finally: 
#			os._exit(0) # Terminate with exit code 0
#	os.waitpid(pid, 0) # Wait for completion of process with pid 0

if(isUserRoot):
    os.seteuid(1000) 
    # Set process user id to something other than root, because root is not allowed
    # when running browsers (risky)

# Options and user agent
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
# Calls an expect script with a bash subshell that enters our ipvanish user information whenever it is prompted
bashCall(f'sudo ./startVPN.exp {location} {password}')

# Initialize
driver = webdriver.Firefox(executable_path = '/home/sw706/webdriver/geckodriver', options = options)

# Cookies
#if deleteCookies == "True" : 
#	print("Deleting Cookies")
#	driver.delete_all_cookies()
#### Is this necessary? I feel like we're not using any cookies anyway, since we open a new webdriver each request

# Get website
driver.get(url)

# Crawl HTML of website
result = crawlUrl(driver, url)
print(result)

# Terminate VPN connection and selenium session
bashCall('sudo /home/sw706/ipvanish/ipvanish-vpn-linux stop')
driver.quit()