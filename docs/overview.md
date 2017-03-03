# kCura Relativity Authentication Bridge

The kCura Relativity Authentication Bridge allows you to determine how users authenticate to Relativity.
This is typically needed when you already have a custom authentication solution for your users or have some other custom authentication requirement that is beyong what Relativity supports.

## Overview

The kCura Relativity Authentication Bridge is designed as a sample application that you can clone and then modify according to your requirements.
It is based on [ASP.NET Core and MVC](https://docs.microsoft.com/en-us/aspnet/core/). 
It will be hosted and execute as its own web application (just as any other ASP.NET Core application would; [see here for more details on hosting ASP.NET Core web application](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/servers/)).
You program the ASP.NET Core application logic to integrate with your custom authentication solution.
The outcome of this authentication must then be conveyed to Relativity using the OpenID Connect authentication protocol.
To provide the OpenID Connect support, the Relativity Authentication Bridge uses an open source framework called [IdentityServer](http://identityserver.io/).
The IdentityServer framework is responsible for flowing the identity of the user to Relativity.

## Workflow

The authentication workflow follows these steps:

1. An unauthenticated user vists Relativity.
1. Relativity sends the user to login (TODO: how does HRD work, or is the bridge the only IdP configured?).
1. Relativity determines that the user must be authenticated by the Relativity Authentication Bridge.
1. Relativity makes an OpenID Connect authentication request to the Relativity Authentication Bridge.
1. Internally at the Relativity Authentication Bridge, IdentityServer processes the OpenID Connect request and must have the user authenticate with one of the authentication scenarios described below.
1. The custom code in the Relativity Authentication Bridge authenticates the user and obtains the user id.
1. The user id is conveyed to IdentityServer.
1. IdentityServer generates an OpenID Connect authentication response to Relativity.
1. Relativity receives the OpenID Connect authentication response from the Relativity Authentication Bridge and maps the user id to the corresponding user in Relativity.
1. If successful, the user is now logged in at Relativity.

## User Id

The user id that the Relativity Authentication Bridge uses must be unique, consistent, and never reused for any other user.

## Authentication Scenarios

There are two custom authentication scenarios that the Relativity Authentication Bridge is designed to support:

* Interactive Login
* HTTP-based Login

### Interactive Login

Interactive login is designed to display a custom login form to the user. 
This custom login form will then accept and validate the user's credentials.
If successful, then the login form will issue a cookie that contains the user's id. 
IdentityServer uses this cookie to then send a OpenID Connect authentication response to Relativity.

### HTTP-based Login

The HTTP-based login is designed for the scenario when the user authentication has already been performed elsewhere and a value in the HTTP request is used to identify the user's id.
Custom logic is then written to extract the user's id from the HTTP request.
This user id is then presented to IdentityServer to send a OpenID Connect authentication response to Relativity.
