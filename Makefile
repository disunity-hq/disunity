# project paths
CORE = Disunity.Core
EDITOR = Disunity.Editor
PRELOADER = Disunity.Preloader
RUNTIME = Disunity.Runtime
MANAGEMENT = Disunity.Management
MANAGER_UI = Disunity.Management.Ui
CLI = Disunity.Management.Cli
CLIENT = Disunity.Client
STORE = Disunity.Store

# makefile boilerplate
DIR := ${CURDIR}
TAG ?= ${TRAVIS_TAG}

UNITY_EDITOR ?= /mnt/c/Program\ Files/Unity/Editor/Unity.exe
COMPOSE = docker-compose --project-directory ${DIR} -f docker/docker-compose.yml

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
	dotnet build -p:SolutionDir=$(DIR) $(ARGS)

build-core:
	dotnet build -p:SolutionDir=$(DIR) $(CORE) $(ARGS)

build-editor:
	dotnet build -p:SolutionDir=$(DIR) $(EDITOR) $(ARGS)

build-preloader:
	dotnet build -p:SolutionDir=$(DIR) $(PRELOADER) $(ARGS)

build-runtime:
	dotnet build -p:SolutionDir=$(DIR) $(RUNTIME) $(ARGS)

build-management:
	dotnet build -p:SolutionDir=$(DIR) $(MANAGEMENT) $(ARGS)

build-cli:
	dotnet build -p:SolutionDir=$(DIR) $(CLI) $(ARGS)

build-store:
	dotnet build -p:SolutionDir=$(DIR) $(STORE) $(ARGS)

build-client:
	dotnet build -p:SolutionDir=$(DIR) $(CLIENT) $(ARGS)


# Clean commands

clean-release:
	rm -rf ./Release

clean: clean-release
	rm -fr **/obj **/bin **/publish **/nupkg

# Publish commands

publish-store:
	dotnet publish -p:SolutionDir=$(DIR) $(STORE) $(ARGS)

publish-client:
	dotnet publish -p:SolutionDir=$(DIR) $(CLIENT) $(ARGS)

publish-management:
	dotnet publish -p:SolutionDir=$(DIR) $(MANAGEMENT) $(ARGS)

publish-cli:
	dotnet publish -p:SolutionDir=$(DIR) $(CLI) $(ARGS)

publish-manager-ui:
	dotnet publish -p:SolutionDir=$(DIR) $(MANAGER_UI) $(ARGS)

publish-editor:
	dotnet publish -p:SolutionDir=$(DIR) $(EDITOR) $(ARGS)

publish-core:
	dotnet publish -p:SolutionDir=$(DIR) -f net471 $(CORE) $(ARGS)

publish-runtime:
	dotnet publish -p:SolutionDir=$(DIR) -f net471 $(RUNTIME) $(ARGS)

publish-preloader:
	dotnet publish -p:SolutionDir=$(DIR) -f net471 $(PRELOADER) $(ARGS)

# release commands

release-all: release-core release-client release-cli release-management release-manager-ui \
						 release-editor release-distro

release-core: clean-release publish-core
	./scripts/release-core.sh

release-client: clean-release publish-client
	./scripts/release-client.sh

release-cli: clean-release publish-cli
	./scripts/release-cli.sh

release-management: clean-release publish-management
	./scripts/release-management.sh

release-manager-ui: clean-release publish-manager-ui
	./scripts/release-manager-ui.sh

release-editor: clean-release publish-editor
	./scripts/release-editor.sh

release-distro: clean-release publish-core publish-runtime publish-preloader
	./scripts/release-distro.sh $(TAG)

release-mod: WINDIR = $(shell wslpath -w -a $(DIR))
release-mod:
	./scripts/release-example-mod.sh
	$(UNITY_EDITOR) -batchmode -nographics -projectPath "$(WINDIR)\ExampleMod" -exportPackage "Assets" "$(WINDIR)\Release\ExampleMod.unitypackage" -quit

deps-and-release-distro: install-deps release-distro

travis-release:
	$(COMPOSE) -f docker/docker-compose.travis.yml build release
	$(COMPOSE) -f docker/docker-compose.travis.yml run release


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
	$(COMPOSE) build release
	$(COMPOSE) run --entrypoint /app/Disunity.Tests/start.sh release

watcher:
	docker-volume-watcher -v --debounce 0.1 disunitystore_* ${DIR}/*
