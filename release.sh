#!/usr/bin/env bash

OUT=Release

copy() {
    mkdir -p "$OUT/$2" && cp $1 "$_"
}

core-outs() {
    local base="Disunity.Core/publish/"
    copy "$base/Disunity.Core.dll" $1
}
core-deps() {
    local base="Disunity.Core/publish/"
    copy "$base/Commons.Json.dll" $1
    copy "$base/Commons.Utils.dll" $1
    copy "$base/Newtonsoft.Json.Schema.dll" $1
    copy "$base/SemanticVersion.dll" $1
}

core() {
    core-deps $1
    core-outs $1
}

editor-outs() {
    local base="Disunity.Editor/publish/"
    copy "$base/Disunity.Editor.dll" $1
}

editor() {
    editor-outs $1
}

preloader-outs() {
    local base="Disunity.Preloader/publish/"
    copy "$base/Disunity.Preloader.dll" $1
}

preloader-deps() {
    local base="Disunity.Preloader/publish/"
    copy "$base/Mono.Cecil.dll" $1
}

preloader() {
    preloader-deps $1
    preloader-outs $1
}

runtime-outs() {
    local base="Disunity.Runtime/publish/"
    copy "$base/Disunity.Runtime.dll" $1
}


runtime() {
    runtime-outs $1
}

zip-distro() {
    local zip_name=distro.zip
    local old_dir=$(pwd)
    [ ! -z "$TRAVIS_TAG" ] && zip_name=distro_$TRAVIS_TAG.zip
    rm -f $zip_name
    cd "$OUT/Distro"
    zip -r "$old_dir/$zip_name" .
}

management-outs() {
    local base="Disunity.Management/publish/"
    copy "$base/Disunity.Management.dll" $1
}

management-deps() {
    local base="Disunity.Management/publish/"
}

management() {
    core $1
    management-deps $1
    management-outs $1
}

cli-outs() {
    local base="Disunity.Cli/publish/"
    copy "Disunity.Cli/scripts/disunity" $1
    copy "$base/Disunity.Cli.dll" $1
    copy "$base/Disunity.Cli.runtimeconfig.json" $1
}

cli-deps() {
    local base="Disunity.Cli/publish/"
}

cli() {
    management $1
    cli-deps $1
    cli-outs $1
}

# 0. Clean up
rm -fr $OUT
mkdir $OUT

# 1. Create Disunity Distribution
core "Distro/core/"
preloader-outs "Distro/patchers/"
runtime-outs "Distro/plugins/"
zip-distro

# 2. Create Mod Editor
editor "Editor/Assets/Disunity/Editor/"
core-outs "Editor/Assets/Disunity/"
core-deps "Editor/Assets/Disunity/Dependencies/"
preloader-outs "Editor/Assets/Disunity/"
preloader-deps "Editor/Assets/Disunity/Dependencies/"
runtime-outs "Editor/Assets/Disunity/"
rm -f ExampleMod/Assets/Disunity/Dependencies/Microsoft.CSharp.dll
rm -f ExampleMod/Assets/Disunity/Dependencies/SemanticVersion.dll
rm -f ExampleMod/Assets/Disunity/Dependencies/System.Dynamic.Runtime.dll
rm -f Release/Editor/Assets/Disunity/Dependencies/Microsoft.CSharp.dll
rm -f Release/Editor/Assets/Disunity/Dependencies/SemanticVersion.dll
rm -f Release/Editor/Assets/Disunity/Dependencies/System.Dynamic.Runtime.dll

# 3. Create Disunity.Management Release
management "Disunity.Management/"

# 4. Create Disunity.Cli Release
cli "Disunity.Cli/"

# Copy Mod Editor to ExampleMod
cp -r Release/Editor/Assets/ ExampleMod/
