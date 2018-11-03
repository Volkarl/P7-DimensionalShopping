import sys, subprocess
from selenium import webdriver
from selenium.webdriver.firefox.options import Options
from switchUserAgent import getUserAgentString
from crawler import crawlUrl

url = sys.argv[1]
requestedUserAgent = sys.argv[2]
deleteCookies = sys.argv[3]
location = sys.argv[4] # Location is found using this site: https://account.ipvanish.com/index.php?t=Server+List&page=1
password = sys.argv[5]

# Options and user agent
options = Options()
options.headless = True
userAgent = getUserAgentString(requestedUserAgent)
print(f'Configuring user agent: {userAgent}')
options.add_argument(f'user-agent={userAgent}')

# Initialize VPN Connection
def bashCall(cmd):
    subprocess.call(cmd, shell=True)

bashCall(f'./startVPN.exp {location} {password}')

# Initialize
driver = webdriver.Firefox(executable_path = 'home/sw706/webdriver/geckodriver', options = options)  # Optional argument, if not specified it will search the path environment var

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
bashCall('/home/sw706/ipvanish/ipvanish-vpn-linux stop')
driver.quit()

######## NEED TO TERMINATE THE VPN CONNECTION AS WELL!