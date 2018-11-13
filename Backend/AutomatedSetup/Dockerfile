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
# Network device

COPY ./dockerEntrypoint.sh /
RUN chmod +x /dockerEntrypoint.sh
ENTRYPOINT ["/dockerEntrypoint.sh"]
# Creates and configures TUN/TAP device driver, necessary for creating a connection with openVPN
# See official documentation at https://www.kernel.org/doc/Documentation/networking/tuntap.txt
# Entrypoint executes the commands as the container is started
# Otherwise these changes to root directory would be lost within an intermediate container layer
CMD [$username]
# Passes the username to the entrypoint script




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
# Run setup script to install all remaining necessary dependecies
# WHY CANT WE DO IT HERE
