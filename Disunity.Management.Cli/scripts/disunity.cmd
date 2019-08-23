@echo off

SET DIR=%~dp0
SET DLL="%DIR%Disunity.Management.Cli.dll"

dotnet %DLL% %*

