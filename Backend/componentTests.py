#!/usr/bin/env python3
import sys, subprocess
from switchUserAgent import getUserAgentString

ipvanishUsername = sys.argv[1]
ipvanishPassword = sys.argv[2]

nonRootUser = "sw706"
defaultVpnServer = "jnb-c01.ipvanish.com"
defaultUserAgent = "PcWindowsChrome"
defaultDeleteCookies = "False"

def pythonCall(cmdString):
	subprocess.call(["python3", cmdString])

def queryCall(arguments):
	pythonCall(f'query.py {arguments} {ipvanishUsername} {ipvanishPassword} {nonRootUser}')

# Perform tests -----------------

# Test userAgent
userAgent = defaultUserAgent
expectedUserAgent = getUserAgentString(userAgent)
res1 = queryCall(f'https://www.whatsmyua.info {userAgent} {defaultDeleteCookies} {defaultVpnServer}')
print (f'{res1 == userAgent} | Result {res1}, Expected {userAgent}')

# Test location
expectedLocation = "South Africa" # Corresponding location
res2 = queryCall(f'https://ipstack.com/ {defaultUserAgent} {defaultDeleteCookies} {defaultVpnServer}')
print (f'{res2 == expectedLocation} | Result {res2}, Expected {expectedLocation}')

# Test cookies
deleteCookies = "True"
res3 = queryCall(f'https://www.w3schools.com/js/js_cookies.asp {defaultUserAgent} {deleteCookies} {defaultVpnServer}')
res4 = queryCall(f'https://www.w3schools.com/js/js_cookies.asp {defaultUserAgent} {deleteCookies} {defaultVpnServer}')
# Each call returns any saved cookies for the site, and afterwards clicks on a button to save a new cookie
# Both calls should return the empty string
print (f'{res3 == "" and res4 == ""} | Result {res3} {res4}, Expected ""')
