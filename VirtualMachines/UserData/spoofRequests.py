from lxml import html
import urllib.request

url = "https://www.whatsmyua.info"

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