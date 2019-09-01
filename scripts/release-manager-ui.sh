#!/usr/bin/env bash
SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
source $SCRIPT_DIR/release-funcs.sh
source "$SOLUTION_ROOT/Disunity.Management.Ui/manager-release-artifacts.sh"

MANAGER_UI_OUT="Disunity.Manangement.Ui"

# 0. Cleanup old out dir
delete "$MANAGER_UI_OUT"

# 1. Copy new files to out dir
manager-ui "$MANAGER_UI_OUT"
