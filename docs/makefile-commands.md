# Makefile Commands

Building and running the components during development is done with the project's `Makefile` located within the repository.

## Executing commands

Executing commands is done with the `make` command, followed by a rule name:

    > make build-cli

Some commands take an optional `ARGS` parameter which is specified as so:

    > make build-cli ARGS="-v detailed"

These arguments are passed to the underlying commands.

## Paket commands

Paket is the tool we use to install our C# dependencies. It is included within the repository at `.paket/paket.exe` See [paket.md](paket.md) for more information.

- `paket [ARGS=""]` Run Paket with the provided arguments.
- `install-deps [ARGS=""]` Install all dependencies and update the `paket.lock` file.
- `update-deps [ARGS=""]` Update all dependencies or those provided as arguments.


## Build commands

The following commands build the Disunity projects.

- `build [ARGS=""]` Build all of the projects.
- `build-core [ARGS=""]` Build `Disunity.Core`
- `build-editor [ARGS=""]` Build `Disunity.Editor`
- `build-preloader [ARGS=""]` Build `Disunity.Preloader`
- `build-runtime [ARGS=""]` Build `Disunity.Runtime`
- `build-management [ARGS=""]` Build `Disunity.Management`
- `build-cli [ARGS=""]` Build `Disunity.Cli`
- `build-store [ARGS=""]` Build `Disunity.Store`

## Clean commands

- `clean` Remove all `bin/`, `obj/` and `publish/` directories.


## Release commands

The following commands publish a release of the Disunity projects. Publishing produces a `Release/` directory in the repository root containing the built projects ready for distribution.

- `release` Publish a Disunity release.

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
