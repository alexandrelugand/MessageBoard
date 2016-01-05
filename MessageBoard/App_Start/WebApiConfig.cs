using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MessageBoard
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            var json = config.Formatters.JsonFormatter; //Autre façon de trouver le JSON formatter: var json = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            //json.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
            //json.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
#if DEBUG
            json.Indent = true;
#endif

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/v1/topics/{id}",
                defaults: new { controller = "topics", id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
              name: "RepliesRoute",
              routeTemplate: "api/v1/topics/{topicid}/replies/{id}",
              defaults: new { controller = "replies", id = RouteParameter.Optional }
          );
        }
    }
}
