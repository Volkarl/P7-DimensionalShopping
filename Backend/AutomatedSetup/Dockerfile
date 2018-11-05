FROM library/ubuntu
# Base image from hub.docker.com

RUN apt update
RUN apt-get install git
WORKDIR /home
RUN git clone "https://github.com/Volkarl/P7-DimensionalShopping.git"
WORKDIR /P7-DimensionalShopping/Backend
RUN chmod +x query.py
RUN chmod +x /AutomatedSetup/setup.sh
RUN ./AutomatedSetup/setup.sh

### WHAT ABOUT SUDO? THESE THINGS NEED TO BE RUN WITH SUDO?


### Is sudo necessary?
# Update apt (package manager)
# apt update
# apt-get install git
# cd /home
# git clone https://github.com/Volkarl/P7-DimensionalShopping.git
# cd /P7-DimensionalShopping/Backend
# chmod +x query.py
# chmod +x /AutomatedSetup/setup.sh
# (SUDO???) ./AutomatedSetup/setup.sh

