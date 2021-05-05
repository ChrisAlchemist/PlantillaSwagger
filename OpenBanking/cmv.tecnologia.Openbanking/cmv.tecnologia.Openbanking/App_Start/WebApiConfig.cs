using cmv.tecnologia.DAO;
using cmv.tecnologia.Openbanking.Herramientas;
using cmv.tecnologia.Openbanking.Interseptores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Unity;

namespace cmv.tecnologia.Openbanking
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Configuración y servicios de API web

            config.MessageHandlers.Add(new InterseptorPeticiones());
            config.MessageHandlers.Add(new InterseptorSeguridad());

            // Rutas de API web
            config.MapHttpAttributeRoutes();

            // Inyeccion de dependencias
            var container = new UnityContainer();
            container.RegisterInstance(new LoginDAO());
            container.RegisterInstance(new CatalogoDAO());
            config.DependencyResolver = new UnityResolver(container);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
