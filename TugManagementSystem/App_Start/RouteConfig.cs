﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TugManagementSystem
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",

                //url: "{controller}/{action}/{id}",
                //defaults: new { controller = "OrderManage", action = "OrderManage", id = UrlParameter.Optional }

                url: "{controller}/{action}/{lan}/{id}",
                defaults: new { controller = "OrderManage", action = "OrderManage", lan = "zh-HK", id = UrlParameter.Optional }
            );
        }
    }
}
