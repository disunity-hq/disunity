# What you need

Developing or building Disunity will require a few things.

## Dotnet Core 2.2 SDK

Download and install Dotnet Core 2.2 SDK from here: 

https://dotnet.microsoft.com/download/dotnet-core/2.2

After installation you should be able to run the `dotnet` command:

```
> dotnet

Usage: dotnet [options]
Usage: dotnet [path-to-application]

Options:
  -h|--help         Display help.
  --info            Display .NET Core information.
  --list-sdks       Display the installed SDKs.
  --list-runtimes   Display the installed runtimes.

path-to-application:
  The path to an application .dll file to execute.
```

## Git for Windows

If you're on Windows, download and install Git for Windows:

https://git-scm.com/download/win

Once installed, you should be able to use the start menu to open "Git Bash". Git Bash is an alternative terminal that emulates some Linux command-line capabilities. We utilize this so that we only need one set of tooling for Windows and Linux.

It also comes with Git.


## Docker

Docker is a container engine. Containers are a lot like Virtual Machines (VMs) in that they allow us to run software in a standard environment without having to setup additional stuff on your machine.

Docker is optional, but it's pretty cool and can make development easier in some ways. You can install it for Windows here:

https://hub.docker.com/editions/community/docker-ce-desktop-windows

Once Docker is installed, you should be able to use the `docker` command.

```
> docker version

Client:
 Version:      18.05.0-ce
 API version:  1.37
 Go version:   go1.9.5
 Git commit:   f150324782643a5268a04e7d1a675587125da20e
 Built:        Sat May 12 09:37:50 2018
 OS/Arch:      linux/amd64
 Experimental: false
 Orchestrator: swarm

Server:
 Engine:
  Version:      18.09.2
  API version:  1.39 (minimum version 1.12)
  Go version:   go1.12
  Git commit:   62479626f213818ba5b4565105a05277308587d5
  Built:        Thu Jan  1 00:00:01 1970
  OS/Arch:      linux/amd64
  Experimental: false
```

## Make

The project Makefile is basically the primary way to interact with the projects while developing. You can install it by downloading a Windows Make port here:

https://sourceforge.net/projects/ezwinports/files/make-4.2.1-without-guile-w32-bin.zip/download

To install:

- Extract the zip somewhere.
- Find your `Git\mingw64\` for your Git Bash install.
- **Tell Windows to NOT overwrite any files in the next step**
- Merge the `Git\mingw64\` folder from the zip with your install.

You should now be able to run the `make` command:

```
> make                                                                                                                                   /
make: *** No targets specified and no makefile found.  Stop.
```

Alternatively, if you have Chocolatey, just install like so:

    choco install make