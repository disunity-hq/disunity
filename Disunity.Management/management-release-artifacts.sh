#!/usr/bin/env bash
MANAGEMENT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
source $MANAGEMENT_DIR/../scripts/release-funcs.sh
source "$SOLUTION_ROOT/Disunity.Core/core-release-artifacts.sh"
source "$SOLUTION_ROOT/Disunity.Client/client-release-artifacts.sh"

management-outs() {
    local base=$MANAGEMENT_DIR/publish
    copy "$base/Disunity.Management.dll" $1
}

management-deps() {
    local base=$MANAGEMENT_DIR/publish
}

management() {
    CORE_DIR="$MANAGEMENT_DIR" core $1
    CLIENT_DIR="$MANAGEMENT_DIR" client $1
    management-deps $1
    management-outs $1
}
