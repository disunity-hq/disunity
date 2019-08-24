# Makefile Commands

Building and running the components during development is done with the project's `Makefile` located within the repository.

## Executing commands

Executing commands is done with the `make` command, followed by a rule name:

    > make build-cli

Some commands take an optional `ARGS` parameter which is specified as so:

    > make build-cli ARGS="-v detailed"

Or as environemnt variables like:
```
> export ARGS="-v detailed"
> make build-cli
or
> ARGS="-v detailed" make build-cli
```
These arguments are passed to the underlying commands.

## Paket commands

Paket is the tool we use to install our C# dependencies. It is included within the repository at `.paket/paket.exe` See [paket.md](paket.md) for more information.

- `paket [ARGS=""]` Run Paket with the provided arguments.
- `install-deps [ARGS=""]` Install all dependencies and update the `paket.lock` file.
- `update-deps [ARGS=""]` Update all dependencies or those provided as arguments.

## Clean commands

- `clean` Remove all `bin/`, `obj/`, `publish/`, and `nupkg/` directories as well as the `Release/` directory.
- `clean-release` Removes the `Release/` directory in the root of the solution
- `<project>/clean` Clean only specified project (eg `make Disunity.Core/clean`)


## Build commands

The following commands build the Disunity projects. The build command will all call `paket update` as needed.

- `build [ARGS=""]` Build all of the projects.
- `build-core [ARGS=""]` Build `Disunity.Core`
- `build-editor [ARGS=""]` Build `Disunity.Editor`
- `build-preloader [ARGS=""]` Build `Disunity.Preloader`
- `build-runtime [ARGS=""]` Build `Disunity.Runtime`
- `build-management [ARGS=""]` Build `Disunity.Management`
- `build-manager-ui [ARGS=""]` Build `Disunity.Management.Ui`
- `build-cli [ARGS=""]` Build `Disunity.Managment.Cli`
- `build-store [ARGS=""]` Build `Disunity.Store`

## Publish commands

While the rules for publish exist as requirements for the release process, it is not recommended to use them as the release targets should output the desired results.
If, however, you need to run a publish manually, all the rules follow this form

- `<project>/publish [ARGS=""]` - Run the publish script on the given project (eg `make Disunity.Core/publish`)

## Release commands

The following commands publish a release of the Disunity projects. Publishing produces a `Release/` directory in the repository root containing the built projects ready for distribution.

- `release-all [ARGS=""]` Create a release for all Disunity components
- `release-cli [ARGS=""]` Create a release for the cli manager
- `release-client [ARGS=""]` Create a release for api client classlib
- `release-core [ARGS=""]` Create a release for the core classlib
- `release-distro [ARGS=""]` Create a release for Disunity distro (including BepInEx)
- `release-editor [ARGS=""]` Create a release for the Unity Editor plugin
- `release-managemnt [ARGS=""]` Create a release for the management classlib
- `release-manager-ui [ARGS=""]` Create a release for the desktop GUI manager
- `release-example-mod [ARGS=""]` Create a release for the example mod

## Store commands

These commands are for working with the store. A number of the commands are for building and running the store within Docker. See [docker.md](docker.md) for more information.

- `store-run` Run the store locally
- `store-build` Build the Docker images required for running the store in Docker
- `store-up` Run the store and other supporting components in Docker.
- `store-up-quick` Will only rebuild the web container before running everything in Docker.
- `store-db [ARGS=""]` Interact with the store's EntityFramework Core DB backend
- `store-db-migrate [ARGS=""]` Manage the store's EF Core migrations
- `store-db-init` Create the initial migrations for the store.
- `store-db-drop` Remove the volumes backing the DB container, effectively resetting the DB.

## Misc commands

Here are various additional commands.

- `test` Run the test suite.
- `watcher` Workaround for filesystem event propagtion to docker containers on windows
