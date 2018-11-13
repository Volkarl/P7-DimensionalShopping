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

VOLUME /dev
# Creates and configures TUN/TAP device driver, necessary for creating a connection with openVPN
# See official documentation at https://www.kernel.org/doc/Documentation/networking/tuntap.txt
RUN mkdir -p /dev/net && mknod /dev/net/tun c 10 200 > /dev/null 2>&1 && chmod 666 /dev/net/tun && ls /dev && ls /dev/net
# Mknod has its stdout and stderr piped to /dev/null, so it won't terminate if the file already exist
# Then changes permissions to allow all users to read and write (not execute)
RUN ls /dev && ls /dev/net

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
RUN git clone "https://github.com/Volkarl/P7-DimensionalShopping.git" && cd P7-DimensionalShopping && git checkout dockerfile-backend
RUN chmod +x P7-DimensionalShopping/Backend/AutomatedSetup/setup.sh
RUN ./P7-DimensionalShopping/Backend/AutomatedSetup/setup.sh $username
