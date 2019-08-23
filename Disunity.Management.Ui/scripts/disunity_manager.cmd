@echo off

SET DIR=%~dp0
SET DLL="%DIR%Disunity.Management.Ui.dll"

dotnet %DLL% %*
