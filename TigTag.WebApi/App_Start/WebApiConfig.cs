using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using Swashbuckle;


namespace TigTag.WebApi
{

    public static class WebApiConfig
    {

        public static void Register(HttpConfiguration config)
        {
          //to enable the CORS (cross original ...)
            var cors = new EnableCorsAttribute("*", "*", "*");
            //    config.EnableCors(cors);
           config.Routes.IgnoreRoute("webform","Views/*/{resource}.aspx/{*pathInfo}");

            config.Routes.MapHttpRoute(
                name: "ApiById",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional },
                constraints: new { id = @"^[0-9]+$" }
            );

            config.Routes.MapHttpRoute(
                name: "ApiByName",
                routeTemplate: "api/{controller}/{action}/{name}",
                defaults: null,
                constraints: new { name = @"^[a-z]+$" }
            );

            config.Routes.MapHttpRoute(
                name: "ApiByAction",
                routeTemplate: "api/{controller}/{action}",
                defaults: new { action = "Get" }
            );

        }
    }
}
