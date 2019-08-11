# Disunity

Disunity is a fullstack toolchain for modding Unity games.

![Disunity Logo](./Frontend/assets/logo_512x512.png "Disunity logo")


## Repository overview

Disunity components are separated into a number of projects within the
repository:

- `Disunity.Core/` general functionality shared by multiple components
- `Disunity.Editor/` Unity Editor extension for exporting mod archives
- `Disunity.Preloader/` MonoMod preload patcher which implements the Disunity mod-preloader
- `Disunity.Runtime/` Bepinex plugin which implements the Disunity runtime mod-loader
- `Disunity.Management/` functionality covering all aspects of local mod management
- `Disunity.Cli/` a commandline mod manager which utilizes Disunity.Management
- `Disunity.Store/` an ASP.NET Core site implementing the mod store backend
- `Frontend/` a Typescript/Vue.js frontend for the mod store

A number of other directories contain other useful things:

- `docs/` developer documentation in markdown
- `docker/` artifacts for building and running our Docker containers
- `lib/` stripped pre-compiled assemblies that some projects rely on, not available in nuget
- `ExampleMod/` a Unity project which contains a simple example Disunity mod

There are a few files of note too:

- `Makefile` contains high-level commands for building and running the site and running tests
- `paket.dependencies` declares all external dependencies for all projects
- `paket.lock` contains the exact concrete versions of our dependencies we currently use
- `Disunity.sln.DotSettings` contains the Rider/Resharper project styles and warning levels
- `.env.template` should be copied to `.env` to supply environment variables to our Docker containers during development
- `.gitignore` prevents certain files from being added to the repository


Please consult the various files within the `docs/` directory for further documentation.