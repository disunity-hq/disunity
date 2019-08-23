#!/usr/bin/env bash
SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
source $SCRIPT_DIR/release-funcs.sh
source "$SOLUTION_ROOT/Disunity.Core/core-release-artifacts.sh"
source "$SOLUTION_ROOT/Disunity.Runtime/runtime-release-artifacts.sh"
source "$SOLUTION_ROOT/Disunity.Preloader/preloader-release-artifacts.sh"

DISTRO_OUT="$OUT/Distro"

# 0. cleanup
rm -rf "$DISTRO_OUT"

# 1. Download BepInEx
extract-online-zip "https://github.com/BepInEx/BepInEx/releases/download/v5.0-RC1/BepInEx.5.0.RC1.v2017.x64.zip" "$DISTRO_OUT"

# 2. Cleanup BepInEx files
rm "$DISTRO_OUT/doorstop_config.ini" # Generated my Disunity.Managment
mv $DISTRO_OUT/BepInEx/* $DISTRO_OUT
rm -r "$DISTRO_OUT/BepInEx"

# 1. Collect distro files
core "Distro/core/"
preloader-outs "Distro/patchers/"
runtime-outs "Distro/plugins/"


# 2. Zip up the distro
ZIP_NAME=distro.zip
[ ! -z "$1" ] && ZIP_NAME=distro_$1.zip
rm -f "$OUT/$ZIP_NAME"
(cd "$DISTRO_OUT"; zip -r "$OUT/$ZIP_NAME" .)

# 3. cleanup files
# rm -rf $DISTRO_OUT
