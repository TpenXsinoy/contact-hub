![Contact Hub Logo](frontend/src/assets/images/Logo/logo.png)

## An application to manage your contacts

### Visit the website at [Contact Hub](https://contacthubapp.azurewebsites.net/login)

**Note:** The website may take some time to load and can only be accessed 60 minutes per day as it is hosted on a free tier of Azure.

# Starting the Application with Docker

1.  Clone the repository
2.  Start your Docker Desktop
3.  Open terminal and navigate to the root directory of the project
4.  Run `docker-compose up` in terminal
5.  Open your browser and go to localhost port for the frontend (default: 3000)
6.  That's it! You can now use the application

**Note:** The environment might also take some time to load since we are pulling an image from [Microsoft Sql Server](https://hub.docker.com/_/microsoft-mssql-server#!), it make some time to set up the temporary database.

## Docker Images in Docker Hub

[Backend Image Repository Link](https://hub.docker.com/repository/docker/tpenxsinoy123/contacthubapi/general)

`docker pull tpenxsinoy123/contacthubapi:tagname`

[Frontend Image Repository Link](https://hub.docker.com/repository/docker/tpenxsinoy123/contacthubapp/general)

`docker pull tpenxsinoy123/contacthubapp:tagname`

[Microsoft Sql Server Repository Link](https://hub.docker.com/_/microsoft-mssql-server#!)

`docker pull mcr.microsoft.com/mssql/server:2022-latest`
