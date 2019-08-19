FROM mcr.microsoft.com/dotnet/core/sdk:2.2

# Install Mono
RUN apt update && apt install -y apt-transport-https dirmngr gnupg ca-certificates && \
        apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF && \
        echo "deb https://download.mono-project.com/repo/debian stable-stretch main" | tee /etc/apt/sources.list.d/mono-official-stable.list && \
        apt update && apt install -y mono-devel

# Install extra dependencies
RUN apt update && apt install -y make zip

# Enable building for net 471
ENV FrameworkPathOverride /usr/lib/mono/4.7.1-api/

WORKDIR /app

ENTRYPOINT ["make", "deps-and-release-distro"]
