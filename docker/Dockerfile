##
## STAGE: frontend
##
FROM node:alpine as frontend
WORKDIR /Source

# npm clean install (installs exact versions from package-lock.json)
COPY Frontend/package.json ./
COPY Frontend/package-lock.json ./
RUN npm ci

COPY Frontend/. ./
RUN npm run build:Debug -- --output-path /Build
ENTRYPOINT npm run build:Watch -- --output-path /Build


##
## STAGE: build
##
FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build

# Install Mono
RUN apt update && apt install -y apt-transport-https dirmngr gnupg ca-certificates && \
        apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF && \
        echo "deb https://download.mono-project.com/repo/debian stable-stretch main" | tee /etc/apt/sources.list.d/mono-official-stable.list && \
        apt update && apt install -y mono-devel

WORKDIR /app

# copy csproj and restore as distinct layers
COPY .paket/ ./.paket/
COPY paket.dependencies ./
COPY paket.lock ./
COPY common.props ./
COPY Disunity.Core/Disunity.Core.csproj ./Disunity.Core/
COPY Disunity.Store/Disunity.Store.csproj ./Disunity.Store/
COPY Disunity.Client/Disunity.Client.csproj ./Disunity.Client/
COPY Disunity.Core/paket.references ./Disunity.Core/
COPY Disunity.Store/paket.references ./Disunity.Store/
COPY Disunity.Client/paket.references ./Disunity.Client/
RUN mono .paket/paket.exe install

# copy frontend
COPY --from=frontend /Build/. ../Frontend/dist

# copy everything else and build app
COPY Disunity.Core ./Disunity.Core/
COPY Disunity.Store ./Disunity.Store/
COPY Disunity.Client ./Disunity.Client/
WORKDIR /app/Disunity.Store
RUN dotnet publish -p:SolutionDir=$(pwd) -c Release -o out Disunity.Store.csproj


##
## STAGE: runtime
##
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS runtime
WORKDIR /app
COPY --from=build /app/Disunity.Store/out ./
ENTRYPOINT ["dotnet", "Disunity.Store.dll"]
