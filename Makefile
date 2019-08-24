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
SRC_DIR := src

COMMON_DEPS := paket.dependencies paket.lock

UNITY_EDITOR ?= /mnt/c/Program\ Files/Unity/Editor/Unity.exe
COMPOSE = docker-compose --project-directory ${DIR} -f docker/docker-compose.yml
DOTNET_ARGS = -p:SolutionDir=$(DIR)

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

	$(PAKET) install $(ARGS)

	$(PAKET) update $(ARGS)

# Build commands

build: build-core build-editor build-preloader build-runtime build-management build-management-ui build-cli build-store build-client
	# dotnet build $(DOTNET_ARGS) $(ARGS)

build-core: $(CORE)/bin

build-editor: $(EDITOR)/bin

build-preloader: $(PRELOADER)/bin

build-runtime: $(RUNTIME)/bin

build-management: $(MANAGEMENT)/bin

build-management-ui: $(MANAGER_UI)/bin

build-cli: $(CLI)/bin

build-store: $(STORE)/bin

build-client: $(CLIENT)/bin


# Clean commands
.PHONY: clean clean-release

clean-release:
	rm -rf ./Release

clean: clean-release
	rm -fr **/obj **/bin **/publish **/nupkg

# Publish commands

$(CORE)/publish $(RUNTIME)/publish $(PRELOADER)/publish: ARGS += -f net471
$(STORE)/publish:
$(MANAGER_UI)/publish: SRC_DIR := .

publish-store: $(STORE)/publish

publish-client: $(CLIENT)/publish

publish-management: $(MANAGEMENT)/publish

publish-cli: $(CLI)/publish

publish-manager-ui: $(MANAGER_UI)/publish

publish-editor: $(EDITOR)/publish

publish-core: $(CORE)/publish

publish-runtime: $(RUNTIME)/publish

publish-preloader: $(PRELOADER)/publish

# release commands

release-all: release-core release-client release-cli release-management release-manager-ui \
						 release-editor release-distro

release-%: scripts/release-%.sh clean-release publish-%
	./scripts/$@.sh $(ARGS)


release-distro: clean-release publish-core publish-runtime publish-preloader
	./scripts/release-distro.sh $(TAG)

release-mod: WINDIR = $(shell wslpath -w -a $(DIR))
release-mod:
	./scripts/release-example-mod.sh
	$(UNITY_EDITOR) -batchmode -nographics -projectPath "$(WINDIR)\ExampleMod" -exportPackage "Assets" "$(WINDIR)\Release\ExampleMod.unitypackage" -quit

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

# Secondary expansion commands
.SECONDEXPANSION:
%/bin: COMMAND := build
%/publish: COMMAND := publish
%/bin %/publish: $(COMMON_DEPS) %/paket.references $$(shell find %/$$(SRC_DIR) -type f -not -path "*/obj/*" -not -path "*/bin/*") $$(shell find % -type f -name *.csproj)
	# safer but slower than `touch $@`. Something even better would be nice
	rm -rf $@
	dotnet $(COMMAND) $(DOTNET_ARGS) $(shell dirname $@) $(ARGS)
