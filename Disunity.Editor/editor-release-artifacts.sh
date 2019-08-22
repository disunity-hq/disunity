#!/usr/bin/env bash
EDITOR_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
source $EDITOR_DIR/../scripts/release-funcs.sh
source "$SOLUTION_DIR/Disunity.Core/core-release-artifacts.sh"

editor-outs() {
    local base=$EDITOR_DIR/publish
    copy "$base/Disunity.Editor.dll" $1
}

editor-deps() {
    local base=$EDITOR_DIR/publish
}

editor() {
    CORE_DIR=$EDITOR_DIR core $1
    editor-deps $1
    editor-outs $1
}

