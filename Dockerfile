FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY *.csproj ./
RUN dotnet restore

COPY . ./
WORKDIR /src
RUN dotnet add package AWSSDK.CloudWatch
RUN dotnet add package Serilog.Sinks.Console --version 4.0.1
RUN dotnet add package AWSSDK.Core --version 3.7.402.8


RUN dotnet publish -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Create log directory
RUN mkdir -p /app/logs

EXPOSE 8080

ENTRYPOINT ["dotnet", "products-api.dll"]




