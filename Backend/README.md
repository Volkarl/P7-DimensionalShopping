# P7-DimensionalShopping

## Execution
To execute this script, ensure the file /AutomatedSetup/setup.sh has been run prior (with root privileges). Setup.sh takes as input the name of a user in whose home directory you wish to store the files. 

Query.py takes as input: (the url of the site to crawl) (the system to emulate) (whether to delete cookies or not) (the ipvanish server to connect to)
(the email of an ipvanish account) (the password of the ipvanish account) (the name of the user in whose home directory the files are stored)

Call query.py with something alike: 
- "./query.py "https://www.whatsmyua.info" "PcMacosxSafari" True "jnb-c01.ipvanish.com" "EMAIL@EMAIL.com" "PASSWORD" "sw706"
or 
- "python3 query.py "https://www.expedia.com/Flights-Search?flight-type=on&starDate=11%2F22%2F2018&endDate=11%2F25%2F2018&mode=search&trip=roundtrip&leg1=from%3ABillund%2C+Denmark+%28BLL%29%2Cto%3AMalaga%2C+Spain%2Cdeparture%3A11%2F22%2F2018TANYT&leg2=from%3AMalaga%2C+Spain%2Cto%3ABillund%2C+Denmark+%28BLL%29%2Cdeparture%3A11%2F25%2F2018TANYT&passengers=children%3A0%2Cadults%3A1%2Cseniors%3A0%2Cinfantinlap%3AY" "PhoneAndroidChrome" True "jnb-c01.ipvanish.com" "EMAIL" "PASSWORD" "sw706"

Running query.py with sudo privileges is not permitted. If you're logged in as root, you can downgrade permissions with: 'sudo -su {NON-ROOT-USER} query.py'

## Docker
To run this script on a docker container, it needs to have been built using the dockerfile in the /AutomatedSetup/ directory and run with the command: 'docker run -it --cap-add=NET\_ADMIN DOCKER_IMAGE_ID /bin/bash'
