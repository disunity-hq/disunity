#!/usr/bin/env bash
SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
OUT=$(readlink -f "$SCRIPT_DIR/../Release")
SOLUTION_ROOT=$(readlink -f "$SCRIPT_DIR/..")

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

extract-online-zip() {
    [ -z "$1" ] && return
    local tmpfile=$(mktemp)

    wget "$1" -O "$tmpfile"
    if [ $? -ne 0 ]; then
        echo "Failed to download bepinex"
        exit 1
    fi

    if [ -z "$2" ]; then
        local outdir=$(pwd)
    else
        local outdir="$2"
    fi

    mkdir -p "$outdir" && unzip -d "$outdir" "$tmpfile"
    rm -f "$tmpfile"
}
