import sys, subprocess
from selenium import webdriver
from selenium.webdriver.firefox.options import Options
from switchUserAgent import getUserAgentString
from crawler import crawlUrl

url = sys.argv[1]
requestedUserAgent = sys.argv[2]
deleteCookies = sys.argv[3]
location = sys.argv[4]
password = sys.argv[5]

# Options and user agent
options = Options()
options.headless = True
userAgent = getUserAgentString(requestedUserAgent)
print(f'Configuring user agent: {userAgent}')
options.add_argument(f'user-agent={userAgent}')

# Initialize VPN Connection
subprocess.check_call("./startVPN.sh %s %s" % (location, password)) #try shell=True if it doesnt work

# Initialize
driver = webdriver.Firefox(options = options)  # Optional argument, if not specified it will search the path environment var

# Cookies
if deleteCookies == "True" : 
	print("Deleting Cookies")
	driver.delete_all_cookies()
#### Is this necessary? I feel like we're not using any cookies anyway, since we open a new webdriver each request

# Get website
driver.get(url)

# Crawl HTML of website
result = crawlUrl(driver, url)
print(result)

# Terminate
driver.quit()
