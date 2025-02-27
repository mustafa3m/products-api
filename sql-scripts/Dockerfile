FROM mysql:latest

ENV MYSQL_ROOT_PASSWORD=securepass
ENV MYSQL_DATABASE=product_db
ENV MYSQL_USER=product-api
ENV MYSQL_PASSWORD=securepass

# Copier le fichier SQL pour initialiser la base de données
COPY database_setup.sql /docker-entrypoint-initdb.d/

EXPOSE 3306



