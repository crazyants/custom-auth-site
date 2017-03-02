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

## Scenarios

There are two custom authentication scenarios that the Relativity Authentication Bridge is designed to support:

* Interactive Login
* HTTP-based Logins

### Interactive Login



### HTTP-based Logins

