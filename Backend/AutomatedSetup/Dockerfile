FROM library/ubuntu
# Base image from hub.docker.com

# -------------------------------------------------------------------
# Startup arguments and create new user (default user is root)

ARG username=sw706
ARG DEBIAN_FRONTEND=noninteractive
# This makes a number of warnings go away

RUN useradd -m $username
# Add user and corresponding home directory

# -------------------------------------------------------------------
# Entrypoint script

COPY ./dockerEntrypoint.sh /
# Copies entrypoint script from the /P7-DimensionalShopping/Backend/AutomatedSetup directory into the root of the container
RUN chmod +x /dockerEntrypoint.sh
RUN echo $username > /etc/username
# Saves pcUsername in file, so that we can read it from the file "callQuery.sh", and know where our openVPN files are placed
# Variable cannot be passed to entrypoint script for reasons detailed in source: https://stackoverflow.com/questions/49133234/docker-entrypoint-with-env-variable-and-optional-arguments

ENTRYPOINT /dockerEntrypoint.sh
# Entrypoint executes the supplied command as the container is started
# Runs dockerEntrypoint.sh with the shell interpreter and any arguments that follow when the container is run

# -------------------------------------------------------------------
# Package manager and things that really should be installed by default in Ubuntu

RUN apt-get update -y
# Update package manager

RUN apt-get install -y apt-utils debconf-utils dialog 
RUN echo 'debconf debconf/frontend select Noninteractive' | debconf-set-selections && echo 'resolvconf resolvconf/linkify-resolvconf boolean false' | debconf-set-selections
# Install utility for reconfiguring missed options (mostly to avoid warnings during installation)

RUN apt-get install tzdata -y
# Install time zone package, which manages our time zone information, which is a dependency for 
# other packages. This is installed here to avoid the problem of future apt-get commands 
# installing this package and prompting us for user input

# -------------------------------------------------------------------
# Install git and download repository

RUN apt-get install git -y
WORKDIR /home/$username
RUN git clone "https://github.com/Volkarl/P7-DimensionalShopping.git" && cd P7-DimensionalShopping
# If we want to use a specific git branch, just add "&& git checkout BRANCH-NAME" to the previous line
RUN chmod +x P7-DimensionalShopping/Backend/AutomatedSetup/setup.sh

RUN ./P7-DimensionalShopping/Backend/AutomatedSetup/setup.sh $username
# Run setup script to install all remaining necessary dependecies
# The dependencies in this script are required to get the scripts to work on a "real" virtual machine, whereas the steps in 
# this dockerfile are required to get it to work on a docker container

# -------------------------------------------------------------------
# HTTP Requests
# Allows the backend to create a node/nodejs/http/express/REST server, which can be queried from the web

RUN apt-get install -y curl \
  && curl -sL https://deb.nodesource.com/setup_9.x | bash - \
  && apt-get install -y nodejs \
  && curl -L https://www.npmjs.com/install.sh | sh
# Install nodejs and npm

WORKDIR /home/$username/P7-DimensionalShopping/Backend/nodejs
RUN npm install morgan && npm install express
# Express: Web framework for nodejs, a layer that we use for HTTP requests 
# HTTP request logging for nodejs, useful for debugging 
RUN chmod +x callQuery.sh
