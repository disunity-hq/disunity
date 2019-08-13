# Linux on Windows

There are numerous options for running Linux software on Windows. By doing so, we can maintain a single set of development procedures and tooling.


## WSL 2

Windows Subystem for Linux (WSL) is the most robust option for running Linux software on Windows today.

WSL2 introduces full kernel support which pushes the quality of the Linux environment provided by WSL even further. It is the Windows-Linux platform of choice for the Disunity project.

### Installing WSL2

WSL2 is currently **only available via the Windows Insider program**. This means that you'll need to enable the Insider Program on your machine, and upgrade your Windows build.

The build required is at least `1903` and is only available on the `Fast Track` of the Insider Program.

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

