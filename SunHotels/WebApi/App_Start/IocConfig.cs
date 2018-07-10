using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.App_Start
{
    public static class IocConfig
    {
        public static void Initialize()
        {
            CrossCutting.Application.RegisterTypes();
        }
    }
}