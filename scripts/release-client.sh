#!/usr/bin/env bash
SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
source $SCRIPT_DIR/release-funcs.sh
source "$SOLUTION_ROOT/Disunity.Client/client-release-artifacts.sh"

CLIENT_OUT="Disunity.Manangement.Client"

# 0. Cleanup old out dir
delete "$CLIENT_OUT"

# 1. Copy new files to out dir
client "$CLIENT_OUT"
