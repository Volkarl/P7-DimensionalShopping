import time
import sys
from selenium import webdriver
url = sys.argv[1]

def valuta(x):
    return {
        'dk'   : 'DKK ',
        'us'   : '$',
        'uk'   : '£',
        'euro' : 'euro '
    }.get(x, 'DKK')    


#set webdriver
driver = webdriver.Chrome()  # Optional argument, if not specified will search path.
driver.get(url)
#time.sleep(5) # Let the user actually see something! why no need for waiting for callback
#price = driver.find_element_by_xpath('//*[@id="flightModuleList"]').text.split('\n')[0].split(' ')[10]
#print(price)
price = driver.find_element_by_xpath('//*[@id="flightModuleList"]').text.split('\n')[0].split(valuta(sys.argv[2]))[1]
print(price)

driver.quit()