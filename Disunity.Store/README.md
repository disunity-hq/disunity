# Disunity Store
This repo contains the source for the disunity.io store.

## Setup guide for development
 * (Windows only see [below](#setup-for-windows-users)): Run `pip install docker-windows-volume-watcher`
 * Copy `.env.template` to `.env` and modify as necessary
 * Run `make up`
 * (Windows only) After all services have started, run `make watcher`

#### Setup for Windows Users
 Currently docker containers do not properly receive file change notifications from
 Windows, as such [docker-windows-volume-watcher](https://github.com/merofeev/docker-windows-volume-watcher) is necessary for file watching in development mode to work.


## Environment Variables
The Disunity Store is mainly configured through `appsettings.json`, however for
changes that need to be provided from the environment `appsettings.json` (namely secrets)
environment variables are available.

|        App Setting        |        `.env` Entry        | Description                                                   |
|:--------------------------|----------------------------|---------------------------------------------------------------|
| Syncfusion:Liscense       | SYNCFUSION_LICENSE         | The license key for [Syncfusion](https://www.syncfusion.com)  |
| AdminUser:Email           | ADMINUSER_EMAIL            | The email address to be used for the generated admin user     |
| AdminUser:Password        | ADMINUSER_PASSWORD         | The password to be used for the generated admin user          |
| Auth:Github:ClientId      | AUTH_GITHUB_CLIENT_ID      | The OAUTH public id to be used during Github authentiaion     |
| Auth:Github:ClientSecret  | AUTH_GITHUB_CLIENT_SECRET  | The OAUTH secret to be used during Github authentiaion        |
| Auth:Discord:ClientId     | AUTH_DISCORD_CLIENT_ID     | The OAUTH public id to be used during Discord authentiaion    |
| Auth:Discord:ClientSecret | AUTH_DISCORD_CLIENT_SECRET | The OAUTH secret to be used during Discord authentiaion       |

### A Note on the Admin User
If either `ADMINUSER_EMAIL` or `ADMINUSER_PASSWORD` aren't specified, an admin user won't be created

### A Note on external login
If either the client id or secret are not specified for any external login provider,
logins using that provider will be disabled to prevent potential errors.

