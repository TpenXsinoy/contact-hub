![Contact Hub Logo](frontend/src/assets/images/Logo/logo.png)

## An application to manage your contacts

### Visit the website at [Contact Hub](https://contacthubapp.azurewebsites.net/login)

**Note:** The website may take some time to load as it is hosted on a free tier of Azure.

# Starting the Application with Docker

run `docker-compose up` in terminal

**Note:** The environment might also take some time to load since we are pulling an image from [Microsoft Sql Server](https://hub.docker.com/_/microsoft-mssql-server#!), it make some time to set up the temporary database.

## Docker Images in Docker Hub

[Backend Image Repository Link](https://hub.docker.com/repository/docker/tpenxsinoy123/contacthubapi/general)

```docker
docker push tpenxsinoy123/contacthubapi:tagname
docker pull tpenxsinoy123/contacthubapi:tagname
```

[Frontend Image Repository Link](https://hub.docker.com/repository/docker/tpenxsinoy123/contacthubapp/general)

```docker
docker push tpenxsinoy123/contacthubapp:tagname
docker pull tpenxsinoy123/contacthubapp:tagname
```

[Microsoft Sql Server Repository Link](https://hub.docker.com/_/microsoft-mssql-server#!)

```docker
docker pull mcr.microsoft.com/mssql/server:2022-latest
```

# Deployment

The application is hosted on Azure, utilizing Azure App Service for deployment and Azure SQL Database for data storage. Additionally, both the Backend and Frontend components are consistently deployed through the Azure Container Registry.

### Docker Container Registry Credentials

```docker
Registry name: contacthubdocker
Login server: contacthubdocker.azurecr.io
Username: contacthubdocker
Password: iJLHkcUUKfE9QYz7tsVhZlUY/P0EUNt1ncMGeMxo33+ACRAi/AaI
```

### Backend Image

```docker
docker push contacthubdocker.azurecr.io/contacthubapi:latest
docker pull contacthubdocker.azurecr.io/contacthubapi:latest
```

### Frontend Image

```docker
docker push contacthubdocker.azurecr.io/contacthub-app:latest
docker pull contacthubdocker.azurecr.io/contacthub-app:latest
```
