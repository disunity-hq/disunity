#!/usr/bin/env bash
SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
OUT="$SCRIPT_DIR/../Release"
SOLUTION_ROOT="$SCRIPT_DIR/.."

copy() {
    mkdir -p "$OUT/$2" && cp -r $1 "$_"
}

delete() {
    rm -fr "$OUT/$1"
}

publish-dir() {
    echo $1/publish
}

remove-unity-duplicates() {
    delete "$1/Microsoft.CSharp.dll"
    delete "$1/SemanticVersion.dll"
    delete "$1/System.Dynamic.Runtime.dll"
}
