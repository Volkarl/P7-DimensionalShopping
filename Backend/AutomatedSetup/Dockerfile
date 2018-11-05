FROM library/ubuntu
# Base image from hub.docker.com

RUN apt update
# Update package manager

RUN apt-get install git

WORKDIR ~
# Set workdir to /home/USER 

RUN git clone "https://github.com/Volkarl/P7-DimensionalShopping.git"
RUN chmod +x P7-DimensionalShopping/Backend/AutomatedSetup/setup.sh
RUN ./P7-DimensionalShopping/Backend/AutomatedSetup/setup.sh

### WHAT ABOUT SUDO? THESE THINGS NEED TO BE RUN WITH SUDO?
