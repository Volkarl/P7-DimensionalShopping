import time, sys
from selenium import webdriver
from selenium.webdriver.chrome.options import Options
#from fake_useragent import UserAgent

url = "https://www.whatsmyua.info"

print(url)

# Options
options = Options()
options.headless = True

# User Agent
#ua = UserAgent()
#userAgent = ua.random
#options.add_argument(f'user-agent={userAgent}')         # WHAT THE FUCK IS THIS f
#options.add_argument('user-agent=Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2272.101 Safari/537.36')
##### OMFG I am using an underscore in the bottom one. Test if it works with a minus
# Figure out how the fake_useragent module works



# Not all combinations are allowed
# These combinations were found using website: https://developers.whatismybrowser.com/useragents/explore/
# We used the top result for each of the operating system that is the highest version (within the top 10) and the correct device type
def userAgentString(x):
    return {
    	'PcWindowsFirefox'		: 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.113 Safari/537.36',
        'PcLinuxChrome'   		: 'Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/44.0.2403.157 Safari/537.36',
        'PhoneAndroidChrome'	: 'Mozilla/5.0 (Linux; Android 6.0.1; SM-G532G Build/MMB29T) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.83 Mobile Safari/537.36',
        'PhoneIOSSafari'		: 'Mozilla/5.0 (iPhone; CPU iPhone OS 11_0 like Mac OS X) AppleWebKit/604.1.38 (KHTML, like Gecko) Version/11.0 Mobile/15A372 Safari/604.1',
        'PcMacosxSafari'		: 'Mozilla/5.0 (Macintosh; Intel Mac OS X 10_10_5) AppleWebKit/603.3.8 (KHTML, like Gecko) Version/10.1.2 Safari/603.3.8'
    }.get(x, '')    

# User Agent
userAgent = userAgentString(sys.argv[1])
print(f'Configuring user agent: {userAgent}')
options.add_argument(f'user-agent={userAgent}')

driver = webdriver.Chrome(options = options)  # Optional argument, if not specified it will search the path environment var

# Cookies
deleteCookies = sys.argv[2]
if deleteCookies == "True" : 
	print("Deleting Cookies")
	driver.delete_all_cookies()
#### Is this necessary? I feel like we're not using any cookies anyway, since we open a new chrome webdriver each request

# Get website
driver.get(url)
uainfo = driver.find_element_by_xpath('//*[@id="rawUa"]').text
uainfo = uainfo.replace('rawUa: ','')
print(uainfo)

driver.quit()
