FROM thinhar/dimensional-shopper-backend2
# Base image from hub.docker.com, containing the backend
# The parent image can connect to VPN and query web pages, etc.
# This image on top allows the backend to create a node/nodejs/http/express/REST server, which can be queried from the web

RUN apt-get install -y curl \
  && curl -sL https://deb.nodesource.com/setup_9.x | bash - \
  && apt-get install -y nodejs \
  && curl -L https://www.npmjs.com/install.sh | sh

RUN npm install --save express
# Web framework for nodejs, a layer that we use for HTTP requests
RUN npm install --save morgan
# HTTP request logging for nodejs, useful for debugging

RUN chmod +x callQuery.sh
