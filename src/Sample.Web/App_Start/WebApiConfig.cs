using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Sample.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            //GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings =
                new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };


            //config.Services.Replace(typeof(IDocumentationProvider), new XmlDocumentationProvider(HttpContext.Current.Server.MapPath("~/App_Data/XmlDocumentation.xml")));
            //config.Filters.Add(new HostAuthenticationFilter(Startup.OAuthOptions.AuthenticationType));


            //config.SetSampleObjects(new Dictionary<Type, object>
            //{
            //    {typeof(PagedResult<MessageResult>), new PagedResult<MessageResult>() {
            //        TotalCount = 42,
            //        Items = new List<MessageResult>() {
            //          new MessageResult(new Inofile.Messaging.Core.Model.WebPortalService.Message()),
            //        }
            //    }}
            //});
        }
    }
}