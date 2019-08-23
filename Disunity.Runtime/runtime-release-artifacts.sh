#!/usr/bin/env bash
RUNTIME_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
source "$RUNTIME_DIR/../scripts/release-funcs.sh"
source "$SOLUTION_ROOT/Disunity.Core/core-release-artifacts.sh"

runtime-outs() {
    local base=$RUNTIME_DIR/publish
    copy "$base/Disunity.Runtime.dll" $1
}

runtime-deps() {
    local base=$RUNTIME_DIR/publish
}


runtime() {
    CORE_DIR="$RUNTIME_DIR" core $1
    runtime-outs $1
    runtime-deps $1
}
