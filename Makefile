# project paths
CORE := Disunity.Core
EDITOR := Disunity.Editor
PRELOADER := Disunity.Preloader
RUNTIME := Disunity.Runtime
MANAGEMENT := Disunity.Management
MANAGEMENT_STARTUP := $(MANAGEMENT).Startup
MANAGER_UI := $(MANAGEMENT).Ui
CLI := $(MANAGEMENT).Cli
CLIENT := Disunity.Client
STORE := Disunity.Store

# makefile boilerplate
DIR := ${CURDIR}
TAG ?= ${TRAVIS_TAG}
SRC_DIR := src
PAKET_ARGS ?= $(ARGS)

UNITY_EDITOR ?= /mnt/c/Program\ Files/Unity/Editor/Unity.exe
COMPOSE = docker-compose --project-directory ${DIR} -f docker/docker-compose.yml
DOTNET_ARGS = -p:SolutionDir=$(DIR)

DOTNET :=
ifeq (, $(shell which dotnet-nix 2>/dev/null))
	DOTNET +=dotnet
else
	DOTNET +=dotnet-nix
endif

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
.PHONY: paket install-deps update-deps restore refresh-paket
paket:
	$(PAKET) $(PAKET_ARGS)

restore:
	$(DOTNET) restore $(ARGS)

paket-refresh: clean clean-paket-files install-deps

paket-hard-refresh: PAKET_ARGS := clear-cache
paket-hard-refresh: clean clean-paket-files paket-clear-cache update-deps

clean-paket-files:
	rm -rf packages paket-files

paket-clear-cache:
	$(PAKET) clear-cache

install-deps: PAKET_ARGS := install $(ARGS)
update-deps: PAKET_ARGS := update $(ARGS)
install-deps update-deps: paket restore

paket-files: paket.dependencies */paket.references
	$(PAKET) update

# Clean commands
.PHONY: clean clean-release %/clean %/clean-publish

clean-release:
	rm -rf ./Release

clean: clean-release
	rm -fr **/obj **/bin **/publish **/nupkg

%/clean: %
	rm -rf $^/obj $^/bin $^/publish

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

# Publish commands

$(CORE)/publish $(RUNTIME)/publish $(PRELOADER)/publish: ARGS += -f net471
$(STORE)/publish: SRC_DIR := .
$(MANAGER_UI)/publish: SRC_DIR := .
%/bin: COMMAND := build
%/publish: COMMAND := publish
%/bin: %/clean-bin
%/publish: %/clean-publish

# release commands

release-all: release-core release-client release-cli release-management release-manager-ui \
						 release-editor release-distro

release-cli: $(CLI)/publish
release-client: $(CLIENT)/publish
release-core: $(CORE)/publish
release-editor: $(EDITOR)/publish
release-management: $(MANAGEMENT)/publish
release-manager-ui: $(MANAGER_UI)/publish
release-%: scripts/release-%.sh clean-release
	./scripts/$@.sh $(ARGS)


release-distro: clean-release $(CORE)/publish $(RUNTIME)/publish $(PRELOADER)/publish
	./scripts/release-distro.sh $(TAG)

release-example-mod: WINDIR = $(shell wslpath -w -a $(DIR))
release-example-mod: release-editor
	./scripts/release-example-mod.sh
	# $(UNITY_EDITOR) -batchmode -nographics -projectPath "$(WINDIR)\ExampleMod" -exportPackage "Assets" "$(WINDIR)\Release\ExampleMod.unitypackage" -quit

travis-release:
	$(COMPOSE) -f docker/docker-compose.travis.yml build release
	$(COMPOSE) -f docker/docker-compose.travis.yml run release


# Store commands

store-run:
# TODO need some way to set the db ip
	$(DOTNET) run -p Disunity.Store/Disunity.Store.csproj

store-build:
	$(COMPOSE) build web $(ARGS)

store-up: store-build
	$(COMPOSE) up db cache frontend web

store-up-quick:
	$(COMPOSE) build web
	$(COMPOSE) up db cache frontend web

store-db:
	$(DOTNET) ef --project $(STORE) $(ARGS)

store-db-migrate:
	$(DOTNET) ef --project $(STORE) migrations add -o Entities/Migrations $(ARGS)

store-db-init:
	$(DOTNET) ef --project $(STORE) migrations add Initial -o Entities/Migrations

store-db-drop:
	docker rm --project-directory $(shell pwd) -f store_db_1
	docker volume rm store_db-data

# management db commands
managment-db-drop:
	$(DOTNET) ef --project $(MANAGEMENT) --startup-project $(MANAGEMENT_STARTUP) database drop
	
management-db-migrate:
	$(DOTNET) ef --project $(MANAGEMENT) --startup-project $(MANAGEMENT_STARTUP) database update
	
managment-db-migration: MIGRATION ?= Initial
managment-db-migration:
	$(DOTNET) ef --project $(MANAGEMENT) --startup-project $(MANAGEMENT_STARTUP) migrations add -o src/Data/Migrations $(MIGRATION)

management-db-clean-migrations:
	rm -rf $(MANAGEMENT)/src/Data/Migrations

management-db-reset: management-db-clean-migrations managment-db-migration management-db-migrate
	
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

%/bin %/publish: paket-files $$(shell find %/$$(SRC_DIR) -type f -not -path "*/obj/*" -not -path "*/bin/*") $$(shell find % -type f -name *.csproj)
	$(DOTNET) $(COMMAND) $(DOTNET_ARGS) $(shell dirname $@) $(ARGS)
	touch $@
