# Introduction

The Custom Auth Site is an authentication toolkit designed to help Relativity customers.  Relativity ships with a certain set of authentication capabilities built-in to the software.  Relativity also supports single sign-on via the OpenID and SAML2 protocols.  However, sometimes customers have specialized requirements that cannot be met with any of the built-in features.

This Custom Auth Site works with Relativity's SSO feature to extend the authentication capabilities of the product.  The site is delivered as a template - on open-sourfce project that your team can easily customize to meet the needs of your organization.

## Use Cases

The Custom Auth Site is designed to fill two primary authenication use cases:

__Replace the Relativity Login Form__

Some customers need the ability to completely customize the Relativity login form.  The Custom Auth Site can be used to completely replace the Relativity login process with one of your own design.  Administrators may want to write their own multi-factor login or connect to a specialized backend system for authentication..

__HTTP Request-based Login__

Some customers \need the ability to authenticate based on the content of the HTTP request.  This is a common enterprise scenario with HTTP application gateways/reverse proxies.


# Getting Started

## Hosting Requirements
- Windows Server 2008R2 or higher
- ASP.Net Core 
- IIS
- The custom auth site and Relativity must both be network-accessible to each other
- TLS must be configured for both Relativity and the IIS site

## Development Requirements
- ASP.Net Core SDK
- Your choice of editor (Visual Studio 2015 or Visual Studio Code is recommended)

## Running the Demo

TODO:  Describe how to download and run the solution 

# Architecture Overview

The Custom Auth Site acts as an external identity provider to the main Relativity application.  The site communicates via the OpenID Connect protocol, which Relativity supports for SSO.

OpenID Connect is a secure authentication protocol that relies on certificates and securely-signed tokens that are passed from the custom auth site to Relativity.  You can learn more about the OpenID Connect protocol here:

<http://openid.net/connect/>

In a typical customer setup, the custom auth site will be installed alongside Relativity within the same instance of Relativity.  The auth site lives in a separate IIS application pool.  Once the site has been deployed to IIS, it is configured to talk with the Relativity instance.  There are configuration settings required in both Relativity and the custom site so that both applications know about each other and can communicatge securely.

TODO:  Diagram

Under the covers, both Relativity and the custom auth site use the Identity Server open-source project.  Identity Server is a well-known authentication and secure token service product from the .NET community.  The custom auth site solution essentially consists of two secure token services talking to each other - one acting as a relying party (Relativity) and the other acting as an external identity provider (the custom auth site).

Customers who want to udnerstand more about Identity Server and how the custom auth site communicates with Relativity can learn more about Identity Server at the following link:

<https://github.com/IdentityServer>

# Configuration and Setup

These instructions describe how to get the custom auth site running and communicating with Relativity.  These steps are broken into three main areas:  Configuring IIS, Configuring Relativity, and Configuring the Custom Auth Site.

## Windows Server Setup

The recommended way to host the custom auth site is within IIS.  You can install the custom auth site in a separate IIS application within the same website as Relativity.  For the typical customer, each web server should have a copy of the cusotm auth site installed.

### .NET Core

The site is built using .NET Core, which is Microsoft's next-gen version of the .NET Framework.  Relativity runs on .NET 4.6.1 (as of Q1 2017).  Both of these frameworks can be installed side-by-side without interfering with one another.

.NET Core can be downloaded from Microsoft:

<https://www.microsoft.com/net/download/core>

### ASP.Net IIS Hosting Bundle

.NET Core applications can easily be hosted in IIS, but the configuration is a little different from traditional ASP.NET applications.  ASP.NET Core requires a hosting bundle to be installed on the server.  This hosting bundle installs a new HttpModule into IIS that will allow you to host the custom auth site properly.

The hosting bundle can be downloaded from Microsoft:

<https://www.microsoft.com/net/download/core>

### IIS Configuration

Once the framework and hosting bundle have been installed, you will need to create an IIS application with the custom site code.  These instructions assume a general understanding of IIS administration.  Here is the general procedure:

1) Download the repository from GitHub.

2) Generate a binary release for the custom auth site.  Within Visual Studio you can do this by opening the TODO solution file; creating a publish profile to publish files to disk; and running the publish command from within Visual Studio.  This will generate a set of files that can be deployed to the IIS application root on the web server (which we will create shortly).
 
3) Create a new Application Pool named "AspNetCore".  For the Managed Runtime setting choose "No managed runtime."  For now we will leave the Application Pool to run under the ApplicationPoolIdenttiy virtual user.

4) Create a folder to hold all of the custom auth site web files.  This will be the web root directory.  Copy the files from Step 2 into this folder.

5) Create a new IIS application within the same website as Relativity.  You can name the application however you like.  For the purposes of this guide we will assume the application is called "custom_auth_site".  Choose the AspNetCore application pool when creating this application.

6) Ensure the directory you created in step 2 has appropriate ACLs.  In particular, verify that the "AppPool\AspNetCore" virtual user has read permissions to the site.


### Certificate Management

The custom auth site requires an X.509 certificate in order to generate
security tokens.  The certificate can be self-signed - it does not have to 
come from a trusted certificate authority such as Verisign.  You should not
have to pay for this certificate.

The GitHub repo includes a testing cert called idsvrtest.pfx.  This
certificate is sufficient to get started with your initial testing.  Take the
following steps to configure the certificate:

1) Move the idsvrtest.pfx to another folder on the web server (do not leave it
in the web root).  Grant appropriate IIS ACLs to the target cert folder.
2) Open the appsettings.json file in the website root
3) Remove the "CertificateStoreSubjectDistinguishedName" setting
4) Uncomment the CertificateFileName and CertificatePassword settings; update
the file paths to point at the new certificate location.

*IMPORTANT NOTE:*  The testing certificate should not be used in production
scenarios.  See the "Certificates" section below for more guidance on setting
up a production certificate.


### Logging

The custom auth site has built-in logging which is very useful for
troubleshooting your connection to Relativity.  The custom auth 
site logs to disk by default.  Edit the appsettings.json file
in the website root and change the "logging.fileName" property to 
point at a location that makes sense for your environment.  Also make sure
that the AppPool\AspNetCore user has write permissions to the logging folder.


## Relativity Setup

## Custom Auth Site Setup


# Source Code Overview


# Customizing the Solution

## Login Form Customization

## HTTP Request Customization


# Additional Considerations

## Certificate Management

## Logging

## Clean Up Relativity Login Methods




