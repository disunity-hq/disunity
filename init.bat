@echo off

if [%1]==[] goto usage

WHERE choco
IF %ERRORLEVEL% NEQ 0 (
    @"%SystemRoot%\System32\WindowsPowerShell\v1.0\powershell.exe" -NoProfile -InputFormat None -ExecutionPolicy Bypass -Command "iex ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))" && SET "PATH=%PATH%;%ALLUSERSPROFILE%\chocolatey\bin"
) ELSE (
    @echo Chocolatey already installed. Skiping.
)

IF NOT EXIST C:\tools\lxrunoffline\LxRunOffline.exe (
    choco install lxrunoffline
) ELSE (
    @echo LxRunOffline alread installed. Skipping.
)

IF NOT EXIST C:\ubuntu-bionic-core-cloudimg-amd64-root.tar.gz (
    bitsadmin.exe /transfer "DownloadUbuntu" https://lxrunoffline.apphb.com/download/Ubuntu/Bionic C:\ubuntu-bionic-core-cloudimg-amd64-root.tar.gz
) ELSE (
    @echo Ubuntu distribution already downloaded. Skipping.
)

IF NOT EXIST C:\wsl\DisunityTemplate\ (
    C:\tools\lxrunoffline\LxRunOffline.exe install -n DisunityTemplate -d C:\wsl\DisunityTemplate -f C:\ubuntu-bionic-core-cloudimg-amd64-root.tar.gz
) ELSE (
    @echo DisunityTemplate environment exists. Skipping.
)

IF NOT EXIST C:\wsl\DisunityTemplate.lnk (
    C:\tools\lxrunoffline\LxRunOffline.exe s -n DisunityTemplate -f C:\wsl\DisunityTemplate.lnk
) ELSE (
    @echo DisunityTemplate.lnk exists. Skipping.
)

C:\tools\lxrunoffline\LxRunOffline.exe su -n DisunityTemplate -v 0
C:\tools\lxrunoffline\LxRunOffline.exe run -n DisunityTemplate -c "adduser disunity"
C:\tools\lxrunoffline\LxRunOffline.exe run -n DisunityTemplate -c "usermod -a -G adm,cdrom,sudo,dip,plugdev disunity"
C:\tools\lxrunoffline\LxRunOffline.exe su -n DisunityTemplate -v 1000

goto :eof
:usage
@echo Usage: %0 ^<install-path^>
exit /B 1


