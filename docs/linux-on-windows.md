# Linux on Windows

There are numerous options for running Linux software on Windows. By doing so, we can maintain a single set of development procedures and tooling.


## WSL 2

Windows Subystem for Linux (WSL) is the most robust option for running Linux software on Windows today.

WSL2 introduces full kernel support which pushes the quality of the Linux environment provided by WSL even further. It is the Windows-Linux platform of choice for the Disunity project.

### Installing WSL2

WSL2 is currently **only available via the Windows Insider program**. This means that you'll need to enable the Insider Program on your machine, and upgrade your Windows build.

The build required is at least `18917` and is only available on the `Fast Track` of the Insider Program.

#### Microsoft Identity

In order to participate in the Insider Program, you'll need a Microsoft Identity account. 

- [Create account here](https://insider.windows.com/oauthinsider?error=msa_signup&state=12345)


#### Enabling Windows Insider

From the Start Menu: 

- **Settings** > **Update & Security** > **Windows Insider Program**
- Click **Get Started**
- Enter your account
- Follow the prompts to install. 

**Be sure to select the Fast Track option.**

#### Updating Windows

From the Start Menu:

- **Settings** > **Update & Security** > **Windows Update**
- Click **Check for Updates**
- Wait for installation to complete

Updating will actually take quite a while, and you'll reboot several times. You can check the same Windows Update settings page to verify you've been updated.

#### Installing Ubuntu

Type `Ubuntu` into the Start Menu, and select the option to install it from the store.

#### Enable the Virtual Machine Platform

Open an **Administrator** PowerShell and run:

```
Enable-WindowsOptionalFeature -Online -FeatureName VirtualMachinePlatform
```

You'll have to restart once it is complete.

#### Set Ubuntu to use WSL2

Open an **Administrator** PowerShell. First check that Ubuntu is properly installed:

```
wsl -l -v
```

Then set the version to WSL2:

```
wsl --set-version Ubuntu 2
```

If this fails, you did not select the **Fast Track** for the Windows Insider Program and your Windows build is not bleeding-edge enough.

Verify that Ubuntu was updated to WSL2:

```
wsl -l -v
```

#### Updating Ubuntu

Open Ubuntu from the Start Menu and run the following:

```
sudo apt update && sudo apt upgrade
```

#### Fixing Networking

If you find that networking within Ubuntu is borked, then try the following.

##### Routing your WSL IP

Open `cmd` and run:

```
ipconfig
```

Find the section for the WSL adapter and note the IP address. In this case it is `192.168.240.1`:

```
Ethernet adapter vEthernet (**WSL**):

   Connection-specific DNS Suffix  . :
   Link-local IPv6 Address . . . . . : fe80::1c1:eb0b:ebe2:2e01%30
   IPv4 Address. . . . . . . . . . . : **192.168.240.1**
   Subnet Mask . . . . . . . . . . . : 255.255.240.0
   Default Gateway . . . . . . . . . :
```

From within Ubuntu:

```
sudo ifconfig eth0 netmask 255.255.240.0
sudo ip route add default via YOUR_WSL_IP_ADDRESS
```

##### Setting DNS Nameservers

If you are still having troubles, see if you're able to ping IP addresses:

```
ping 8.8.8.8
```

But not domain names:

```
ping google.com
````

If you're able to ping `8.8.8.8` but not `google.com` then DNS is screwy. A quick fix is to edit `/etc/resolv.conf` to point to the Google nameservers.

Open `/etc/resolv.conf`:

```
sudo nano /etc/resolv.conf
```

Edit to read:

```
nameserver 8.8.8.8
nameserver 8.8.4.4
```

Press `Ctrl-o` then `Enter` to save.

Press `Ctrl-x` to quit.

Try to ping `google.com`

```
ping google.com
```

##### Run some random code off the internet

If all else fails, try this PowerShell script:

https://gist.github.com/andrewvc/fe22397c554ac3e6255681bfc864e62e


## LxRunOffline

`LxRunOffline` is a Windows commandline utility for managing WSL environments. With it, you can have multiple Ubuntu environments for example. It also supports things like duplicating environments and other neat things.

### Installing LxRunOffline

The easiest way to install LxRunOffline and do all the other steps in this section is to run `init.bat` from the Disunity repo.

Alternatively install LxRunOffline with Chocolatey:

```
sudo choco install lxrunoffline
```

### Installing a Linux Distro

Visit the following link and download Ubuntu:

https://lxrunoffline.apphb.com/download/Ubuntu/Bionic

Then you can create an environment from the distribution:

```
LxRunOffline install -n ANameYouPick -d where/to/install -f path/to/downloaded/distro
```

### Starting an Environment

Use the following command to start an environment:

```
LxRunOffline.exe run -w -n SomeName
```

You can create a symlink for easily running an environment:

```
LxRunOffline.exe s -n SomeName -f where/to/install/SomeName.lnk
```

### Duplicating an Environment

LxRunOffline makes it super easy to duplicate an environment:

```
LxRunOffline.exe d -n SomeSourceName -d where/to/install -N SomeNewName
```

### Creating a template

It can be useful to create a template environment from which you duplicate other environments as needed.

Start by following the steps above to create a new Ubuntu environment. Unlike WSL environments created by the Windows Store, the only user created is root.

#### Creating a non-root user

```
adduser disunity
usermod -a -G adm,cdrom,sudo,dip,plugdev
```

#### Set user as default login

By default, the environment will login as root. To login as your own user by default run the following command:

```
LxRunOffline.exe su -n SomeName -v 1000
```

You can now copy this environment as many times as you'd like.