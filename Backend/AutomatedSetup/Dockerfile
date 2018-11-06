FROM library/ubuntu
# Base image from hub.docker.com

# -------------------------------------------------------------------

ARG username=sw706

RUN useradd -m $username
# Add user and create home directory

USER $username

# -------------------------------------------------------------------

RUN apt-get update -y
# Update package manager

# -------------------------------------------------------------------

RUN apt-get install git -y

WORKDIR $HOME
# Set workdir to /home/$username 

RUN git clone "https://github.com/Volkarl/P7-DimensionalShopping.git"
RUN chmod +x P7-DimensionalShopping/Backend/AutomatedSetup/setup.sh
RUN ./P7-DimensionalShopping/Backend/AutomatedSetup/setup.sh $username

# With regards to root privileges: We have switched the user from default (root) to sw706,
# but we are still allowed full access to commands that would usually require sudo
# We are not entirely sure why, but assume it has something to do with the way the 
# container setup (docker build) differs from a normal VM