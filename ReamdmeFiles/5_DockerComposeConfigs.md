# Docker Compose Configurations

- The ports on either side of the colo is the same. This means we are using the same port from external source and from the within the application

**To Run the docker-compose file, run the following in the terminal where the docker-compose file is located**

> docker compose up -d

**Check if the postgres layer imported from Docker is running correctly.**

Navigate to Docker -> Choose Container from the left-hand side and make sure you see a postgres instance is running for the post specified in the docker compose file.

At the end you should see a message **database system is ready to accept connections**
