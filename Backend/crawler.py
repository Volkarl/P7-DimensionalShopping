import tldextract

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
	elif domain == 'bearsmyip':
		return crawlLocation(driver)
	else: 
		return ""

# -----------------------------------------------------------------------------------------------------
# Requires website: https://bearsmyip.com/
# The website shows our location

def crawlLocation(driver):
	return getElementText(driver, '//*[@id="location"]')

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
	return getElementText(driver, '//*[@id="flightModuleList"]').split('\n')[0].split(valuta(urlSuffix))[1]
