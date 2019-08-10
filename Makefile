
all:
	docker-compose run -w /app/Disunity.Store dotnet $(args)

build:
	docker-compose build

up: build
	docker-compose up db cache frontend web

ef:
	dotnet ef --project Disunity.Store $(args)

migration:
	dotnet ef --project Disunity.Store migrations add -o Entities/Migrations $(args)

initdb:
	dotnet ef --project Disunity.Store migrations add Initial -o Entities/Migrations

dropdb:
	docker rm -f store_db_1
	docker volume rm store_db-data

test:
	docker-compose run -w /app/Disunity.Store.Tests --entrypoint /app/Disunity.Store.Tests/start.sh dotnet

watcher:
	docker-volume-watcher -v --debounce 0.1 disunitystore_* ${CURDIR}/*
