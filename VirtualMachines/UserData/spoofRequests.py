#from lxml import html
#import urllib.request

#url = "https://www.whatsmyua.info"

#page = requests.get(url)
#tree = html.fromstring(page.content)
#element = tree.xpath('//*[@id="rawUa"]/text()')
#element = tree.xpath('//*[@id="uas_textfeld"]/text()')
#print(element)

#t = r.text
#for line in t:
#    if re.search(r'[a-z]', line) is None:
#        print(line)



#try:
#    with urllib.request.urlopen(url) as response:
#        page = response.read().decode('utf-8')#use whatever encoding as per the webpage
#        tree = html.fromstring(page)
#        element = tree.xpath('//*[@id="rawUa"]/text()')
#        print(element)
#except urllib.request.HTTPError as e:
#    if e.code==404:
#        print(f"{url} is not found")
#    elif e.code==503:
#        print(f'{url} base webservices are not available')
#        ## can add authentication here 
#    else:
#        print('http error',e)







import time
from selenium import webdriver
from selenium.webdriver.chrome.options import Options
from fake_useragent import UserAgent

url = "https://www.whatsmyua.info"

print(url)

# Options
options = Options()
options.headless = True

# User Agent
ua = UserAgent()
userAgent = ua.random
options.add_argument(f'user-agent={userAgent}')
#options.add_argument('user_agent=Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2272.101 Safari/537.36')

driver = webdriver.Chrome(options = options)  # Optional argument, if not specified it will search the path environment var
driver.get(url)
uainfo = driver.find_element_by_xpath('//*[@id="rawUa"]').text
uainfo = uainfo.replace('rawUa: ','')
print(uainfo)

driver.quit()
