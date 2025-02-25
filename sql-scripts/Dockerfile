FROM mysql:latest

ENV MYSQL_ROOT_PASSWORD=securepass
ENV MYSQL_DATABASE=product_db
ENV MYSQL_USER=product-api
ENV MYSQL_PASSWORD=securepass

# Copier le fichier SQL pour initialiser la base de données
COPY database_setup.sql /docker-entrypoint-initdb.d/

EXPOSE 3306



#
#Api
#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#WORKDIR /src
#COPY *.csproj ./
#RUN dotnet restore
#
#COPY . ./
#RUN dotnet publish -c Release -o /app/out
#
#FROM mcr.microsoft.com/dotnet/aspnet:8.0
#WORKDIR /app
#COPY --from=build /app/out .
#
#EXPOSE 80
#
#ENTRYPOINT ["dotnet", "products-api.dll"]
#


#FROM mysql:latest
 #
#ENV MYSQL_ROOT_PASSWORD=securepass
#ENV MYSQL_DATABASE=product_db
#ENV MYSQL_USER=product-api
#ENV MYSQL_PASSWORD=securepass
 #
## copy from to
#COPY product_db.sql /docker-entrypoint-initdb.d/
 #
#EXPOSE 3306 



## Use an official Node.js runtime as a parent image
#FROM node:14
#
## Set the working directory in the container
#WORKDIR /usr/src/app
#
## Copy the package.json and package-lock.json files to the container
#COPY package*.json ./
#
## Install dependencies
#RUN npm install
#
## Copy the rest of the application code to the container
#COPY . .
#
## Expose the port the app runs on
#EXPOSE 8080
#
## Command to run the application
#CMD [ "node", "index.js" ]
