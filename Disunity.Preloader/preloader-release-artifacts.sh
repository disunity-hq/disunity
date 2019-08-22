#!/usr/bin/env bash
PRELOADER_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
source $PRELOADER_DIR/../scripts/release-funcs.sh
source "$SOLUTION_DIR/Disunity.Core/core-release-artifacts.sh"


preloader-outs() {
    local base=$PRELOADER_DIR/publish
    copy "$base/Disunity.Preloader.dll" $1
}

preloader-deps() {
    local base=$PRELOADER_DIR/publish
    copy "$base/Mono.Cecil.dll" $1
}

preloader() {
    CORE_DIR="$RUNTIME_DIR" core $1
    preloader-deps $1
    preloader-outs $1
}
