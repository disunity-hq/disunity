#!/usr/bin/env bash
MANAGER_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
source $MANAGER_DIR/../scripts/release-funcs.sh


manager-ui-out() {
  local base=$MANAGER_DIR/publish
  copy "$base/../scripts/disunity_manager" $1
  copy "$base/../scripts/disunity_manager.cmd" $1
  copy "$base/*" $1
}

manager-ui-deps() {
  local base=$MANAGER_DIR/publish
}

manager-ui() {
  manager-ui-deps $1
  manager-ui-out $1
}
