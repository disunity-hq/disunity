#!/usr/bin/env bash
CLI_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
source $CLI_DIR/../scripts/release-funcs.sh
source "$SOLUTION_ROOT/Disunity.Management/management-release-artifacts.sh"

cli-outs() {
    local base=$CLI_DIR/publish
    copy "$base/../scripts/disunity" $1
    copy "$base/../scripts/disunity.cmd" $1
    copy "$base/Disunity.Management.Cli.dll" $1
    copy "$base/Disunity.Management.Cli.runtimeconfig.json" $1
}

cli-deps() {
    local base=$CLI_DIR/publish
    copy "$base/CommandLine.dll" $1
    copy "$base/Microsoft.Extensions.Configuration.dll" $1
    copy "$base/Microsoft.Extensions.Configuration.EnvironmentVariables.dll" $1
    copy "$base/Microsoft.Extensions.Configuration.Abstractions.dll" $1
    copy "$base/Microsoft.Extensions.DependencyInjection.dll" $1
    copy "$base/Microsoft.Extensions.DependencyInjection.Abstractions.dll" $1
    copy "$base/Microsoft.Extensions.Primitives.dll" $1
    copy "$base/Newtonsoft.Json.dll" $1
    copy "$base/System.IO.Abstractions.dll" $1
}

cli() {
    MANAGEMENT_DIR="$CLI_DIR" management $1
    cli-deps $1
    cli-outs $1
}

