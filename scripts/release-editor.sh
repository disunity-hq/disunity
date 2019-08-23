#!/usr/bin/env bash
SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
source $SCRIPT_DIR/release-funcs.sh
source "$SOLUTION_ROOT/Disunity.Core/core-release-artifacts.sh"
source "$SOLUTION_ROOT/Disunity.Editor/editor-release-artifacts.sh"
source "$SOLUTION_ROOT/Disunity.Preloader/preloader-release-artifacts.sh"
source "$SOLUTION_ROOT/Disunity.Runtime/runtime-release-artifacts.sh"

EDITOR_OUT_BASE="Editor/Assets/Disunity"
EDITOR_DEPS="$EDITOR_OUT_BASE/Dependencies"

# 0. Cleanup out directory
rm -rf "$EDITOR_OUT_BASE"

# 1. Create Mod Editor
editor-outs "$EDITOR_OUT_BASE/Editor/"
editor-deps "$EDITOR_DEPS"
core-outs "$EDITOR_OUT_BASE"
core-deps "$EDITOR_DEPS"
preloader-outs "$EDITOR_OUT_BASE"
preloader-deps "$EDITOR_DEPS"
runtime-outs "$EDITOR_OUT_BASE"
runtime-deps "$EDITOR_DEPS"

# 2. Cleanup assemblies already loaded by Unity
remove-unity-duplicates "$EDITOR_OUT_BASE"
remove-unity-duplicates "$EDITOR_DEPS"

