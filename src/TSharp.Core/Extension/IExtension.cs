// Copyright © 2015 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace ClassLibrary1.Extension
{
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.DependencyInjection;

    public interface IExtension
    {
        string Name { get; }

        void ConfigureServices(IServiceCollection services);
        void RegisterRoutes(IRouteBuilder routeBuilder);
    }
}