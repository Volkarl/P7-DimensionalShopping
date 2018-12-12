import tldextract, re

def markAsResult(text):
	return f'RESULT:<{text}>:RESULT'

def getElementText(driver, xpath):
	return driver.find_element_by_xpath(xpath).text

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
	return markAsResult(getElementText(driver, '/html/body/div/section[1]/div/div/div[2]/div[2]/div/div[7]/span'))

# -----------------------------------------------------------------------------------------------------
# Requires website: https://www.whatsmyua.info 
# This website shows which user agent we are using

def crawlUserAgent(driver):
	return markAsResult(getElementText(driver, '//*[@id="rawUa"]').replace('rawUa: ',''))

# -----------------------------------------------------------------------------------------------------
# Requries website such as: 

def valuta(x):
    return {
        'com'   : '$',
        'dk'   	: 'DKK ',
        'co.uk' : '£', 
        'de' 	: '€'
    }.get(x, 'DKK')    

# Returns price of top element in flightModuleList
def crawlExpedia(driver, urlSuffix):
	print(f'suffix: {urlSuffix}')
	text = getElementText(driver, '//*[@id="flightModuleList"]')

	priceList = regexFindAllIn(text, "\d+[,.]?\d*\s*"+ re.escape(valuta(urlSuffix)) + "|" + re.escape(valuta(urlSuffix)) + "\s*\d+[.,]?\d*", "Price")
	durationList = regexFindAllIn(text, "[\d]+:[\d]+[^A-Z]*[[\d]+:[\d]+[^\s]*", "Duration")
	ratingList = regexFindAllIn(text, "\(\d*.\d*\/\d*\)", "Rating")
	timeList = regexFindAllIn(text, "\d+[a-zA-Z]+\s\d*[a-zA-Z]*\s\W(?:\d+\s*)?[a-zA-Z]*\W", "Time")
	legList = regexFindAllIn(text, "[A-Z]{3} -(?:(?:\s*\d+[a-z]+)+\s[a-z]+\s+[A-Z]{3}(?:[a-zA-Z\s]+(?:\s*\d+[a-z]+)+\s[a-z]+\s+[A-Z]{3})+)?\s[a-zA-Z\s:]*\s*(?:- )?[A-Z]{3}", "Leg")

	resultList = priceList[0] + "\n" + durationList[0] + "\n" + ratingList[0] + "\n" + timeList[0]+  "\n" + legList[0]
	return markAsResult(resultList)


def regexFindAllIn(textToSearch, regexString, resultName):
    intermidiarryVariable = regexString
    regex  = re.compile(intermidiarryVariable)
    resultList = regex.findall(textToSearch)
    if resultList:
        resultList[0] = f"---{resultName}---\n{resultList[0]}"
        return resultList
    else: print(f'{resultName} was empty')
    return []
