#!/usr/bin/env bash
CLIENT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
source $CLIENT_DIR/../scripts/release-funcs.sh

client-out() {
  local base=$CLIENT_DIR/publish
  copy "$base/Disunity.Client.dll" $1
}

client-deps() {
  local base=$CLIENT_DIR/publish
}

client() {
  client-deps $1
  client-out $1
}
