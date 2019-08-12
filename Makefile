# project paths
CORE = Disunity.Core/Disunity.Core.csproj
EDITOR = Disunity.Editor/Disunity.Editor.csproj
PRELOADER = Disunity.Preloader/Disunity.Preloader.csproj
RUNTIME = Disunity.Runtime/Disunity.Runtime.csproj
MANAGEMENT = Disunity.Management/Disunity.Management.csproj
CLI = Disunity.Cli/Disunity.Cli.csproj
STORE = Disunity.Store/Disunity.Store.csproj

# makefile boilerplate
COMPOSE = docker-compose --project-directory $(shell pwd) -f docker/docker-compose.yml

OSFLAG :=
ifeq ($(OS),Windows_NT)
	OSFLAG += WIN32
else
	UNAME_S := $(shell uname -s)
	ifeq ($(UNAME_S),Linux)
		OSFLAG +=$(strip LINUX)
	endif
	ifeq ($(UNAME_S),Darwin)
		OSFLAG +=$(strip OSX)
	endif
endif
OSFLAG := $(strip $(OSFLAG))

PAKET :=
ifeq ($(OSFLAG),LINUX)
	PAKET += mono .paket/paket.exe
else
	PAKET += .paket/paket.exe
endif
PAKET := $(strip $(PAKET))


# Paket commands

paket:
	$(PAKET) $(ARGS)

install-deps:
	$(PAKET) install $(ARGS)

update-deps:
	$(PAKET) update $(ARGS)


# Build commands

build:
	dotnet build $(ARGS)

build-core:
	dotnet build $(CORE) $(ARGS)

build-editor:
	dotnet build $(EDITOR) $(ARGS)

build-preloader:
	dotnet build $(PRELOADER) $(ARGS)

build-runtime:
	dotnet build $(RUNTIME) $(ARGS)

build-management:
	dotnet build $(MANAGEMENT) $(ARGS)

build-cli:
	dotnet build $(CLI) $(ARGS)

build-store:
	dotnet build $(STORE) $(ARGS)


# Release commands

release:
	dotnet publish $(ARGS)
	./release.sh


# Store commands

store-run:
# TODO need some way to set the db ip
	dotnet run -p Disunity.Store/Disunity.Store.csproj

store-build:
	$(COMPOSE) build $(ARGS)

store-up: store-build
	$(COMPOSE) up db cache frontend web

store-up-quick:
	$(COMPOSE) build web
	$(COMPOSE) up db cache frontend web

store-db:
	dotnet ef --project Disunity.Store $(ARGS)

store-db-migrate:
	dotnet ef --project Disunity.Store migrations add -o Entities/Migrations $(ARGS)

store-db-init:
	dotnet ef --project Disunity.Store migrations add Initial -o Entities/Migrations

store-db-drop:
	docker rm --project-directory $(shell pwd) -f store_db_1
	docker volume rm store_db-data


# Db commands

db-up:
	$(COMPOSE) up db cache

test:
	$(COMPOSE) run -w /app/Disunity.Store.Tests --entrypoint /app/Disunity.Tests/start.sh dotnet

watcher:
	docker-volume-watcher -v --debounce 0.1 disunitystore_* ${CURDIR}/*
