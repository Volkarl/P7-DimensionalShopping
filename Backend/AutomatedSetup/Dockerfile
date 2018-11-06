FROM library/ubuntu
# Base image from hub.docker.com

# -------------------------------------------------------------------

# Default user is root

ARG username=sw706

RUN useradd -m $username
# Add user and create home directory

# -------------------------------------------------------------------

RUN apt-get update -y
# Update package manager

# -------------------------------------------------------------------

RUN apt-get install git -y

WORKDIR /home/$username

RUN git clone "https://github.com/Volkarl/P7-DimensionalShopping.git" && cd P7-DimensionalShopping && git checkout dockerfile-backend
RUN chmod +x P7-DimensionalShopping/Backend/AutomatedSetup/setup.sh
RUN ./P7-DimensionalShopping/Backend/AutomatedSetup/setup.sh $username