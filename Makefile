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
.PHONY: paket install-deps update-deps
paket:
	$(PAKET) $(ARGS)
install-deps:
	$(PAKET) install $(ARGS)
	
update-deps:
	$(PAKET) update $(ARGS)
	
paket-files: paket.dependencies */paket.references
	$(PAKET) update 
	
# Clean commands
.PHONY: clean clean-release %/clean %/clean-publish

clean-release:
	rm -rf ./Release

clean: clean-release
	rm -fr **/obj **/bin **/publish **/nupkg

%/clean: PROJ_DIR = %
%/clean-publish: PROJ_DIR = %
%/clean-bin: PROJ_DIR = %
%/clean: %/clean-publish %/clean-bin
	rm -rf $(PROJ_DIR)/obj
%/clean-bin:
	rm -rf $(PROJ_DIR)/bin
%/clean-publish:
	rm -rf $(PROJ_DIR)/publish

# Build commands

build: build-core build-editor build-preloader build-runtime build-management build-management-ui build-cli build-store build-client
	# dotnet build $(DOTNET_ARGS) $(ARGS)

build-core:  paket-files$(CORE)/bin

build-editor: paket-files $(EDITOR)/bin

build-preloader: paket-files $(PRELOADER)/bin

build-runtime: paket-files $(RUNTIME)/bin

build-management: paket-files $(MANAGEMENT)/bin

build-management-ui: paket-files $(MANAGER_UI)/bin

build-cli: paket-files $(CLI)/bin

build-store: paket-files $(STORE)/bin

build-client: paket-files $(CLIENT)/bin

# Publish commands

$(CORE)/publish $(RUNTIME)/publish $(PRELOADER)/publish: ARGS += -f net471
$(STORE)/publish: SRC_DIR := . 
$(MANAGER_UI)/publish: SRC_DIR := .

publish-store: paket-files $(STORE)/publish

publish-client: paket-files $(CLIENT)/publish

publish-management: paket-files $(MANAGEMENT)/publish

publish-cli: paket-files $(CLI)/publish

publish-manager-ui: paket-files $(MANAGER_UI)/publish

publish-editor: paket-files $(EDITOR)/publish

publish-core: paket-files $(CORE)/publish

publish-runtime: paket-files $(RUNTIME)/publish

publish-preloader: paket-files $(PRELOADER)/publish

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
%/bin: %/clean-bin
%/publish: %/clean-publish

%/bin %/publish: $(COMMON_DEPS) %/paket.references $$(shell find %/$$(SRC_DIR) -type f -not -path "*/obj/*" -not -path "*/bin/*") $$(shell find % -type f -name *.csproj)
	dotnet $(COMMAND) $(DOTNET_ARGS) $(shell dirname $@) $(ARGS)
