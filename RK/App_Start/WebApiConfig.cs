using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace RK
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            
            //config.Formatters.Remove(config.Formatters.XmlFormatter);
           // config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("text/html"));
            //config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("application/xml"));
            // Adding formatter for XML   
           
           /* GlobalConfiguration.Configuration.Formatters.JsonFormatter.MediaTypeMappings
.Add(new System.Net.Http.Formatting.RequestHeaderMapping("Accept",
                              "text/html",
                              StringComparison.InvariantCultureIgnoreCase,
                              true,
                              "application/json"));*/

            // Adding formatter for Json   
            config.Formatters.JsonFormatter.MediaTypeMappings.Add(
                new QueryStringMapping("type", "json", new MediaTypeHeaderValue("application/json")));

            // Adding formatter for XML   
            config.Formatters.XmlFormatter.MediaTypeMappings.Add(
                new QueryStringMapping("type", "xml", new MediaTypeHeaderValue("application/xml")));

            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling
= Newtonsoft.Json.ReferenceLoopHandling.Ignore; 
        }
    }
}
