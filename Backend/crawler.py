import tldextract
import re

def markAsResult(text):
	return f'RESULT:<{text}>'

def getElementText(driver, xpath):
	return markAsResult(driver.find_element_by_xpath(xpath).text)

def crawlUrl(driver, url):
	tdl = tldextract.extract(url)
	domain = tdl.domain
	print(f'domain: {domain}')
	if domain == 'expedia':
		return crawlExpedia(driver, tdl.suffix)
	elif domain == 'whatsmyua': 
		return crawlUserAgent(driver)
	elif domain == 'ipstack':
		return crawlLocation(driver)
	elif domain == 'w3schools':
		return crawlCookies(driver)
	else: 
		return ""

# -----------------------------------------------------------------------------------------------------
# Requires website: https://www.w3schools.com/js/js_cookies.asp
# Clicks on button that shows existing cookies, then clicks button that creates cookie, then returns value from first step
# This is useful to test whether the cookie we created persists through sessions

def crawlCookies(driver):
	displayCookieButton = driver.find_element_by_xpath('//*[@id="main"]/p[21]/button[1]')
	createCookieButton = driver.find_element_by_xpath('//*[@id="main"]/p[21]/button[2]')
	displayCookieButton.click()
	# Opens a popup that shows the existing cookies saved by the page
	popup = driver.switchTo().alert()
	cookies = popup.getText()
	popup.accept()
	# Click the OK on the alert popup
	createCookieButton.click()
	return markAsResult(cookies)

# -----------------------------------------------------------------------------------------------------
# Requires website: https://ipstack.com/
# The website shows our location

def crawlLocation(driver):
	return getElementText(driver, '/html/body/div/section[1]/div/div/div[2]/div[2]/div/div[7]/span')

# -----------------------------------------------------------------------------------------------------
# Requires website: https://www.whatsmyua.info 
# This website shows which user agent we are using

def crawlUserAgent(driver):
	return getElementText(driver, '//*[@id="rawUa"]').replace('rawUa: ','')

# -----------------------------------------------------------------------------------------------------
# Requries website such as: 

def valuta(x):
    return {
        'com'   : '$',			############# WATCH OUT, BECAUSE .COM DATES ARE DIFFERENT, SO WE CAN'T JUST CONVERT 
        'dk'   	: 'DKK ',
        'co.uk' : '£', 
        'de' 	: '€' 			############# THIS DOESN'T WORK EITHER, SINCE .DE PRICES ARE REVERSED: 210 € instead of € 210
    }.get(x, 'DKK')    

# Returns price of top element in flightModuleList
def crawlExpedia(driver, urlSuffix):
	print(f'suffix: {urlSuffix}')
	text      = getElementText(driver, '//*[@id="flightModuleList"]')

	pricereg  = re.compile("\d+[,.]?\d*\s*\$|\$\s*\d+[.,]?\d*")
	pricelist = pricereg.findall(text)
	if pricelist :
	    print(*pricelist[0])
	    print("----price---------------------------")
	else: print("pricelist was empty, something is wrong")

	durationreg  = re.compile("[\d]+:[\d]+[^A-Z]*[[\d]+:[\d]+[^\s]*")
	durationlist = durationreg.findall(text)
	if durationlist :
	    print(*durationlist[0])
	    print("---------duration----------------------")
	else: print("durationlist was empty, something is wrong")

	ratingreg  = re.compile("\(\d*.\d*\/\d*\)")
	ratinglist = ratingreg.findall(text)
	if ratinglist :
	    print(*ratinglist[0])
	    print("-----------------rating--------------")
	else: print("ratinglist was empty, something is wrong")

	timereg  = re.compile("\d+[a-zA-Z]+\s\d*[a-zA-Z]*\s\W(?:\d+\s*)?[a-zA-Z]*\W")
	timelist = timereg.findall(text)
	if timelist :
	    print(*timelist[0])
	    print("-----------------------time--------")
	else: print("timelist was empty, something is wrong")

	# this one is.... complicated, and took a while to write... in nano... without parenthasis help because im smart
	legsreg  = re.compile("[A-Z]{3} -(?:(?:\s*\d+[a-z]+)+\s[a-z]+\s+[A-Z]{3}(?:[a-zA-Z\s]+(?:\s*\d+[a-z]+)+\s[a-z]+\s+[A-Z]{3})+)?\s[a-zA-Z\s:]*\s*(?:- )?[A-Z]{3}")
	legslist = legsreg.findall(text)
	if legslist :
	    print(*legslist[0])
	    print("---------------------------legs----")
	else: print("legslist was empty, something is wrong")

	resultArray =  [pricelist[0],durationlist[0],ratinglist[0],timelist[0],legslist[0]]
	bashCall(f'echo resultArray > /home/crawlResult')
	#return getElementText(driver, '//*[@id="flightModuleList"]').split('\n')[0].split(valuta(urlSuffix))[1]
