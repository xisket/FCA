﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "GetPaged",
                routeTemplate: "api/Product/GetPaged/{requestedPage}/{pageSize}/{orderByFieldName}/{orderByDescending}/{searchString}" 
            );
        }
    }
}
