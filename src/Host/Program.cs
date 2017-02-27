// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.IO;
using Microsoft.AspNetCore.Hosting;
using System;

namespace RelativityAuthenticationBridge
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = RelativityAuthenticationBridgeConstants.RelativityAuthenticationBridgeName;

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}