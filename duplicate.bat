@echo off

if [%1]==[] goto usage
if [%2]==[] goto usage

IF NOT EXIST C:\tools\lxrunoffline\LxRunOffline.exe (
    @echo Couldn't find LxRunOffline at C:\tools\lxrunoffline\
    goto :eof
) ELSE (
    IF EXIST %1\wsl\%2 (
        @echo Environment %2 already exists.
        goto :eof
    )

    C:\tools\lxrunoffline\LxRunOffline.exe sm -n DisunityTemplate

    IF errorlevel 1 (
        @echo DisunityTemplate doesn't exist. Did you run init.bat?
        goto :eof
    )

    C:\tools\lxrunoffline\LxRunOffline.exe d -n DisunityTemplate -d %1\wsl\%2 -N %2
    C:\tools\lxrunoffline\LxRunOffline.exe s -n %2 -f %1\wsl\%2.lnk
)

goto :eof
:usage
@echo Usage: %0 ^<install-path^> ^<env-name^>
exit /B 1


