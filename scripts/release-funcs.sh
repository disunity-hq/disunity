#!/usr/bin/env bash
SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
OUT="$SCRIPT_DIR/../Release"
SOLUTION_ROOT="$SCRIPT_DIR/.."

copy() {
    mkdir -p "$OUT/$2" && cp $1 "$_"
}

publish-dir() {
    echo $1/publish
}

remove-unity-duplicates() {
    rm -f $1/Microsoft.CSharp.dll
    rm -f $1/SemanticVersion.dll
    rm -f $1/System.Dynamic.Runtime.dll
}
