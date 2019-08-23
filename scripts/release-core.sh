#!/usr/bin/env bash
SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
source $SCRIPT_DIR/release-funcs.sh
source "$SOLUTION_ROOT/Disunity.Core/core-release-artifacts.sh"

CORE_OUT="Disunity.Manangement.Cli"

# 0. Cleanup old out dir
rm -rf "$CORE_OUT"

# 1. Copy new files to out dir
core "$CORE_OUT"
