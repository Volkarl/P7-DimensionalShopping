import time, sys
from selenium import webdriver
from selenium.webdriver.chrome.options import Options
from switchUserAgent import getUserAgentString
from crawler import crawlUrl

url = sys.argv[1]
print(url)

# Options and user agent
options = Options()
options.headless = True
userAgent = getUserAgentString(sys.argv[2])
print(f'Configuring user agent: {userAgent}')
options.add_argument(f'user-agent={userAgent}')

# Initialize
driver = webdriver.Chrome(options = options)  # Optional argument, if not specified it will search the path environment var

# Cookies
deleteCookies = sys.argv[3]
if deleteCookies == "True" : 
	print("Deleting Cookies")
	driver.delete_all_cookies()
#### Is this necessary? I feel like we're not using any cookies anyway, since we open a new chrome webdriver each request

# Get website
result = crawlUrl(driver, url)
print(result)

# Terminate
driver.quit()
