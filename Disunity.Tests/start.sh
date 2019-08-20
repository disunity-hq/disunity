#!/usr/bin/env bash
cp -r /app /stage

cd /stage

export FrameworkPathOverride=/usr/lib/mono/4.7.1-api/

dotnet test
