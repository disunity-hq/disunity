# Getting Started (Win)

We try to make getting started with Disunity development as easy as possible. That said, you'll need to go through this document in order to get going.

## Bootstrapping WSL2

WSL2 is the second version of the Windows Subsystem for Linux. If you'd like to set it up manually, you can read the procedure at [linux-on-windows.md](linux-on-windows.md).

Otherwise you can run `init.bat` from an **administrator** command-line or powershell terminal.

```
> .\init.bat C:
```

The `init.bat` script will do the following:

- Install [Chocolatey](https://chocolatey.org/)
- Install [LxRunOffline](https://github.com/DDoSolitary/LxRunOffline)
- Download an Ubuntu Xenial image
- Create a WSL2 distro at `wsl\DisunityTemplate\`
- Create a shortcut at `wsl\DisunityTemplate.lnk`
- Create a `disunity` user with the distro.

At this point you should use the shortcut to start the `DisunityTemplate` WSL2 distro.

Navigate to wherever you have the Disunity repo on the Window's filesystem:

```
> cd /mnt/c/disunity
```

Then execute the `init.sh` script to fully initalize the `DisunityTemplate` distro:

```
> ./init.sh
```

The `init.sh` script will do the following:

- Update Ubuntu package repositories
- Install Docker and Docker Compose
- Install the Dotnet Core SDK and Mono
- Copy `.env.template` to `.env`

At this point your `DisunityTemplate` WSL2 distro is setup and copies can be made from it.

## Duplicating DisunityTemplate

In order to duplicate `DisunityTemplate` you'll need to first shutdown the WSL2 virtual-machine.

Run the following from an **administrator** powershell terminal:

```
> wsl.exe --shutdown
```

Shutting down the WSL2 virtual-machine currently borks the VM's networking. Restore it by running the `restore-network.ps1` powerscript script:

```
> .\restore-network.ps1
```

You can now duplicate the `DisunityTemplate` environment:

```
> .\duplicate.bat C: TestEnv
```

You should now see a shortcut for starting the new distro at `C:\wsl\TestEnv.lnk`


## Starting Docker

Since WSL2 environments do not have an init system, you'll need to manually start the Docker daemon when you want to use it:

```
sudo service docker start
```