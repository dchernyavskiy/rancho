FROM mcr.microsoft.com/dotnet/sdk:6.0 AS builder

# Setup working directory for the project
WORKDIR /src

COPY ./src/Directory.Build.props ./
COPY ./src/Directory.Build.targets ./
COPY ./src/Directory.Packages.props ./
COPY ./src/Packages.props ./

COPY ./src/ApiGateway/Directory.Build.props ./ApiGateway/
COPY ./src/ApiGateway/ECommerce.ApiGateway/ECommerce.ApiGateway.csproj ./ApiGateway/ECommerce.ApiGateway/

# Restore nuget packages
RUN dotnet restore ./ApiGateway/ECommerce.ApiGateway/ECommerce.ApiGateway.csproj

# Copy project files
COPY ./src/ApiGateway/ECommerce.ApiGateway/  ./ApiGateway/ECommerce.ApiGateway/

RUN ls

WORKDIR /src/ApiGateway/ECommerce.ApiGateway/

#https://andrewlock.net/5-ways-to-set-the-urls-for-an-aspnetcore-app/
#https://swimburger.net/blog/dotnet/how-to-get-aspdotnet-core-server-urls
#https://tymisko.hashnode.dev/developing-aspnet-core-apps-in-docker-live-recompilation

RUN dotnet watch run  ECommerce.ApiGateway.csproj --launch-profile ApiGateway.LiveRecompilation
