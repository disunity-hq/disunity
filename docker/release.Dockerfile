FROM alpine as context

# copy the entire project
COPY . .

# extract files needed to install deps
RUN cp */*.csproj /deps --parents
RUN cp */paket.references /deps --parents
RUN cp paket.dependencies /deps
RUN cp -r .paket /deps

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 as build

# Install Mono
RUN apt update && apt install -y apt-transport-https dirmngr gnupg ca-certificates && \
        apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF && \
        echo "deb https://download.mono-project.com/repo/debian stable-stretch main" | tee /etc/apt/sources.list.d/mono-official-stable.list && \
        apt update && apt install -y mono-devel

# Install extra dependencies
RUN apt update && apt install -y make zip

# Enable building for net 471
ENV FrameworkPathOverride /usr/lib/mono/4.7.1-api/

# Copy project dependency files
WORKDIR /app
COPY --from=context /deps .

# install the project dependencies
RUN mono .paket/paket.exe install

# Copy rest of the project
COPY --from=context . .

CMD ["make", "deps-and-release-distro"]
