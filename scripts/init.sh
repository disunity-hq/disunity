#!/usr/bin/env bash

sudo apt-get update -y
sudo apt-get upgrade -y
sudo apt-get install -y gnupg ca-certificates

# add dotnet repository
sudo add-apt-repository universe
sudo apt-get install apt-transport-https

# add mono repository
sudo apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF
echo "deb https://download.mono-project.com/repo/ubuntu stable-xenial main" | sudo tee /etc/apt/sources.list.d/mono-official-stable.list

# update packages listings
sudo apt-get update -y

# install dotnet sdk
wget -q https://packages.microsoft.com/config/ubuntu/16.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
sudo apt-get install -y dotnet-sdk-2.2

# install mono
sudo apt install -y mono-complete

# install docker-compose
sudo curl -L "https://github.com/docker/compose/releases/download/1.24.1/docker-compose-$(uname -s)-$(uname -m)" -o /usr/local/bin/docker-compose
sudo chmod +x /usr/local/bin/docker-compose

# install docker
sudo apt-get install -y docker.io
sudo service docker start

# add docker group
sudo usermod -a -G docker disunity

# setup .env
cp .env.template .env

echo "export FrameworkPathOverride=/usr/lib/mono/4.7.1-api/" >> /home/disunity/.bashrc

