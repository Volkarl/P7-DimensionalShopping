FROM library/ubuntu
# Base image from hub.docker.com

# -------------------------------------------------------------------

# Default user is root

ARG username=sw706
ARG DEBIAN_FRONTEND=noninteractive
# This makes a number of warnings go away

RUN useradd -m $username
# Add user and create home directory

# -------------------------------------------------------------------
# Package manager and things that really should be installed by default in Ubuntu

RUN apt-get update -y
# Update package manager

RUN apt-get install apt-utils -y 
# Install utility for reconfiguring missed options (mostly to avoid warnings during installation)

RUN apt-get install tzdata -y
# Install time zone package, which manages our time zone information, which is a dependency for 
# other packages. This is installed here to avoid the problem of future apt-get commands 
# installing this package and prompting us for user input

# -------------------------------------------------------------------

RUN apt-get install git -y

WORKDIR /home/$username

RUN git clone "https://github.com/Volkarl/P7-DimensionalShopping.git" && cd P7-DimensionalShopping && git checkout dockerfile-backend
RUN chmod +x P7-DimensionalShopping/Backend/AutomatedSetup/setup.sh
RUN ./P7-DimensionalShopping/Backend/AutomatedSetup/setup.sh $username