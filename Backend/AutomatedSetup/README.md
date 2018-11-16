# P7-DimensionalShopping

Dockerfile calls the setup script and entrypoint script: query.py should work out of the gate if used on a container

If you're creating a true Ubuntu virtual machine, the setup.sh script *should* be enough. If it's not (likely if you're using a very minimalistic Ubuntu installation), then look through the dockerfile and entrypoint script and ensure that everything they mention has been done; then run the setup script again. 
