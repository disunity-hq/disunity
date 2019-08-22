#!/usr/bin/env bash
CORE_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
source $CORE_DIR/../scripts/release-funcs.sh

core-outs() {
    local base=$CORE_DIR/publish
    copy "$base/Disunity.Core.dll" $1
}

core-deps() {
    local base=$CORE_DIR/publish
    copy "$base/Commons.Json.dll" $1
    copy "$base/Commons.Utils.dll" $1
    copy "$base/Newtonsoft.Json.Schema.dll" $1
    copy "$base/SemanticVersion.dll" $1
}

core() {
    core-deps $1
    core-outs $1
}
