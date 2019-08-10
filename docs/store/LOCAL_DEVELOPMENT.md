# Local Development

## Table of Contents

- [Local Development](#Local-Development)
  - [Table of Contents](#Table-of-Contents)
  - [Setup](#Setup)
    - [System Resources](#System-Resources)
      - [.NET Core SDK](#NET-Core-SDK)
      - [Node.js/NPM](#NodejsNPM)
      - [PostgreSQL](#PostgreSQL)
      - [pgAdmin (Optional)](#pgAdmin-Optional)
    - [Installing Code Dependencies](#Installing-Code-Dependencies)
  - [Building and Running the project](#Building-and-Running-the-project)
    - [Building the Frontend](#Building-the-Frontend)
    - [Starting the server](#Starting-the-server)
    - [Running in Watch mode](#Running-in-Watch-mode)
    - [Local Settings](#local-settings)

The main project readme describes the recommended approach for development that
utilizes docker to ensure a consistent environment across machines. Using docker
for development does have some potential down-sides however, most notably it becomes
significantly more work to get the debugger working from within docker, an as such
isn't ideal for all situations.

With that in mind this document will describe the steps necessary to get your local
machine up and running with all needed tooling to support development without
using docker.

## Setup

### System Resources

#### [.NET Core SDK](https://dotnet.microsoft.com/)

First, we'll be needing to install the .NET core SDK and command line tool. Follow the steps [here][dotnet-dl] to install .NET core 2.2 if you do not already have these tools installed. **NOTE:** Most IDEs (Visual Studio and Rider included) will do this step for you during install.

#### [Node.js/NPM](https://nodejs.org/en/)

We also need `npm` (or `yarn`) to be installed on your system for compiling the frontend for the store. Instructions for installing `npm` can be found [here][npm-dl].

#### [PostgreSQL](https://www.postgresql.org/)

The server uses postgres as it's backing database so we need to install and configure a postgres server on our local machine. Follow the instructions [here][postgres-dl] to get a postgres server installed on your machine.

Unfortunately, postgres requires a bit of configuration before we can use it for our project.

First we need to tell postgres to trust our local machine so we don't have to use a password to connect to ourselves. To do this, open `pg_hba.conf` in you favorite editor. This file is usually located in `/etc/postgresql/<version>/main/` on linux/macos or `C:\Program Files\Postgres\<version>\` on Windows. Scroll down until you see the two entries that look like:

```sh
# IPv4 local connections:
host    all             all             127.0.0.1/32            md5
# IPv6 local connections:
host    all             all             ::1/128                 md5
```

Change the `md5` in both entries to `trust`. Once you have done that, we need to create a postgres user for the disunity store to use. This can most easily be done by opening pgAdmin, connecting to your local database and right-clicking on `Login/Group Roles` and hitting create. The new user should be called `disunity` and have at least login and create database priviliges.

If you don't have pgAdmin and/or would rather use SQL to create the user, the following command will create a users with the proper permissions:
```sh
psql -c "CREATE USER disunity WITH
	LOGIN
	NOSUPERUSER
	CREATEDB
	NOCREATEROLE
	INHERIT
	NOREPLICATION
	CONNECTION LIMIT -1";
```

#### [pgAdmin (Optional)](https://www.pgadmin.org)

pgAdmin is a highly usefully graphical interface for querying/editing a postgres database. It is a recommended, but by no means necessary. If desired, follow the instructions [here][pgadmin-dl].


### Installing Code Dependencies

Once both of those tools are installed the real fun can begin. We need to use both of the cli tools we installed to download and install the dependencies for the .NET server as well as the Frontend. Run the following commands to install all needed dependencies for dotnet:

```sh
.paket/paket install # Downloads and installs dotnet deps
```

Then run the following to install the dependencies for the Frontend build process

```sh
cd Frontend
npm i # install frontend dependencies
```

#### A Note on Paket <!-- omit in toc -->

Paket is a tool for managing dotnet dependencies both from nuget and directly from git repositories.
We utilize paket to keep our sister projects up-to-date with each other without the need to constant pushes to nuget.

## Building and Running the project

### Building the Frontend

Building the frontend is quite simple actually. Two `npm` scripts are provided for creating the frontend bundles, `build:Debug` and `build:Release`.
`build:Debug` will generate an un-minified, source-mapped bundle. `build:Release` does essentially the same thing, but skips the source-map and minifies the bundle to reduce load time.

For example, to build the Frontend in with the dev configuration, run:

```sh
cd Frontend
npm run build:Debug
```

**NOTE**: For development convenience, `build:Debug` as been aliased to `build` (ie running `npm run build` is equivalent to running `npm run build:Debug`)

### Starting the server

Once the Frontend has been built, you are ready to start the server. Simply run:
```sh
cd Disunity.Store
dotnet run
```

### Running in Watch mode

Both the Frontend build and dotnet have a "watch" mode where they will automatically recompile if any of the input files change.

To build the Frontend in watch mode, run `npm run build:Watch`.

To start the server in watch mode, run `dotnet watch run`

### Local settings
The local appsettings can be used for an settings that you want to apply locally that you don't wish to be committed.
This can include modifications meant purely for testing, or secret values.

To create an `appsetings.Local.json` simply make a copy of `appsettings.Local.template.json` and modify as you see fit.
You may add or remove values, but if you remove and/or don't specify the secrets, then features that rely on them will be disabled 

---

#### VSCode Remote Development <!-- omit in toc -->
While VSCode now supports, as an experimental feature, developing from a remote
environment (within a docker image included), it is possible that we will transition
to utilizing such tools. However due to editing tools like ReSharper not being
available on VSCode, it is not recommended to use this approach, while technically possible.

[dotnet-dl]:https://dotnet.microsoft.com/download/dotnet-core
[npm-dl]:https://nodejs.org/en/download/
[postgres-dl]:https://www.postgresql.org/download
[pgadmin-dl]:https://www.pgadmin.org/download/
