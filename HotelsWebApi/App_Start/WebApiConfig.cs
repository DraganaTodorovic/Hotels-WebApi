using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using AutoMapper;
using HotelsWebApi.Interfaces;
using HotelsWebApi.Models;
using HotelsWebApi.Repository;
using HotelsWebApi.Resolver;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Practices.Unity;
using Newtonsoft.Json.Serialization;

namespace HotelsWebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // CORS
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            // Tracing
            config.EnableSystemDiagnosticsTracing();

            // Unity
            var container = new UnityContainer();
            container.RegisterType<ILanciRepository, LanciRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IHoteliRepository, HoteliRepository>(new HierarchicalLifetimeManager());
            config.DependencyResolver = new UnityResolver(container);

            //AutoMapper
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Lanac, LanacDTO>(); // automatski će mapirati Author.Name u AuthorName
                //.ForMember(dest => dest.DrzavaIme, opt => opt.MapFrom(src => src.Zemlja.Ime)); // ako želimo eksplicitno zadati mapranje
                cfg.CreateMap<Hotel, HotelDTO>(); // automatski će mapirati Author.Name u AuthorName
                //.ForMember(dest => dest.LanacNaziv, opt => opt.MapFrom(src => src.Lanac.Naziv)); // ako želimo eksplicitno zadati mapiranje                
            });

        }
    }
}
