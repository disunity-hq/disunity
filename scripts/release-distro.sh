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

# 3. Download the monomod loader
extract-online-zip "https://github.com/scottbot95/BepInEx.MonoMod.Loader/releases/download/v1.0.0/BepInEx.MonoMod.Loader_v1.0.0.zip" "$DISTRO_OUT/patchers"

# 4. Collect distro files
core "Distro/core/"
preloader-outs "Distro/patchers/"
runtime-outs "Distro/plugins/"


# 5. Zip up the distro
create-tagged-zip "disunity" "$DISTRO_OUT" "$1"

# 6. cleanup files
# rm -rf $DISTRO_OUT
