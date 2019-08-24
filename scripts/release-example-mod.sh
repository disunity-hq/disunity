#!/usr/bin/env bash
SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
source $SCRIPT_DIR/release-funcs.sh

disable-reference-validation() {
  local filename="$OUT/$DIR_NAME/$1.meta"

  mkdir -p $(dirname $filename) && cat > "$filename" <<EOL
PluginImporter:
  validateReferences: 0
EOL
}

DIR_NAME="ExampleMod"

# 0.  Cleanup out
delete "$OUT/$DIR_NAME"

# 1. Copy ExampleMod to release
copy "$DIR_NAME" .

# 2. Copy Editor to Example mod
copy "$OUT/Editor/Assets" "$DIR_NAME"

# 3. Disable reference validation on some assemblies
disable-reference-validation "Assets/Disunity/Dependencies/Newtonsoft.Json.Schema.dll"
disable-reference-validation "Assets/Disunity/Disunity.Runtime.dll"
disable-reference-validation "Assets/Disunity/Disunity.Preloader.dll"
disable-reference-validation "Assets/Disunity/Disunity.Core.dll"
disable-reference-validation "Assets/Disunity/Editor/Disunity.Editor.dll"
disable-reference-validation "Library/ScriptAssemblies/ExamplePreloader.dll"
disable-reference-validation "Library/ScriptAssemblies/ExampleRuntime.dll"
disable-reference-validation "Library/ScriptAssemblies/Assembly-CSharp-Editor.dll"
