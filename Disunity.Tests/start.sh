#!/usr/bin/env bash

cp -r /app /stage

cd /stage

dotnet test
