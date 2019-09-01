#!/usr/bin/env bash
SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
source $SCRIPT_DIR/release-funcs.sh
source "$SOLUTION_ROOT/Disunity.Management/management-release-artifacts.sh"

MANAGEMENT_OUT="Disunity.Manangement"

# 0. Cleanup old out dir
delete "$MANAGEMENT_OUT"

# 1. Copy new files to out dir
management "$MANAGEMENT_OUT"
