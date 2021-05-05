using cmv.tecnologia.DAL;
using cmv.tecnologia.NotificationService.Interceptors;
using cmv.tecnologia.NotificationService.Tools;
using Swashbuckle.Application;
using System.Web.Http;
using Unity;

namespace cmv.tecnologia.NotificationService {
  public static class WebApiConfig {
    public static void Register(HttpConfiguration config) {
      // Configuración y servicios de API web
      config.EnableCors();

      config.MessageHandlers.Add(new RequestInterceptor());
      config.MessageHandlers.Add(new SecurityInterceptor());
      // Rutas de API web
      config.MapHttpAttributeRoutes();

      var container = new UnityContainer();
      container.RegisterInstance(new NotificationDAO());
      config.DependencyResolver = new UnityResolver(container);


            // Reedirige a Swagger
        config.Routes.MapHttpRoute(
                name: "swagger_root",
                routeTemplate: "",
                defaults: null,
                constraints: null,
                handler: new RedirectHandler((message => message.RequestUri.ToString()), "swagger"));

        config.Routes.MapHttpRoute(
          name: "DefaultApi",
          routeTemplate: "api/{controller}/{id}",
          defaults: new { id = RouteParameter.Optional }
      );
    }
  }
}
