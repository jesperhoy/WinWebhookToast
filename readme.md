# WinWebhookToast

WinWebhookToast is a simple Windows application which sits in the system tray (toaster icon)
listening for HTTP requests and displays a Windows notification message ("toast")
with an icon, title and body text from the HTTP request.

## Why

I created this application because I needed some way for my local [n8n](https://n8n.io) instance (running as a 
Windows service via [NSSM](https://nssm.cc)) - to send me quick notifications, for example when it completes or 
fails to complete some background task (in my case - compiling web-page templates as I edit and save them).

## Use cases

- Get notifications from [n8n](https://n8n.io) or [Node-RED](https://nodered.org) instances on the
same computer or local network.

- Get notifications from other services running on the same computer or local network which can integrate via webhooks.

- Get notifications from various Internet services which can integrate via webhooks - by tunneling 
through [ngrok](https://ngrok.com), [loophole](https://loophole.cloud) or similar.  
Think [Zapier](https://zapier.com), [Make](https://www.make.com), etc. and really anything that talk webhook.

## Installing and running

A compiled version is available under "releases" (GitHub).

Download and unzip to a new folder and run `WinWebhookToast.exe`

Requires [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) to run

You can make it run automatically at Windows login using Windows Task Scheduler.

## Command line

You can optionally specify a HTTP port number as the first and only parameter.
For example:

`WinWebhookToast.exe 8080`

The default port number is 55123.

## Webhook method / URL

GET http://localhost-or-ip-address:55123/?title=title-text&body=body-text&icon=success

You can use line feeds and non-ASCII characters including emojis in the title and body using standard URL encoding.

The `icon` parameter can be `error`, `info`, `success`, `warning` - or if you put other .png
image files in the "icons" sub-folder - the name of those files (without ".png").

