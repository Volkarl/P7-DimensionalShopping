# P7-DimensionalShopping
There are 2 scripts both fulfulling the same functionality.
Installing the dependencies for the data retrival script.
The two differ in what system its intended for.
ps1 is for windows
sh is for linux, specificly ubuntu
when runnning the linux version, there is one thing that must be considered.
It does not set the environment variable. 
So before the script can be run the following command needs to be run.
export PATH=$PATH:/home/sw706e18/webdriver 
Or the location of the webdriver.