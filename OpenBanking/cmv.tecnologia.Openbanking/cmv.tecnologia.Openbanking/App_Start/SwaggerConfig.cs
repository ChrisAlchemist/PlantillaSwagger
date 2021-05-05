using System.Web.Http;
using WebActivatorEx;
using cmv.tecnologia.Openbanking;
using Swashbuckle.Application;
using cmv.tecnologia.Openbanking.App_Start;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace cmv.tecnologia.Openbanking
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                    {
                        // By default, the service root url is inferred from the request used to access the docs.
                        // However, there may be situations (e.g. proxy and load-balanced environments) where this does not
                        // resolve correctly. You can workaround this by providing your own code to determine the root URL.
                        //
                        //c.RootUrl(req => GetRootUrlFromAppConfig());

                        // If schemes are not explicitly provided in a Swagger 2.0 document, then the scheme used to access
                        // the docs is taken as the default. If your API supports multiple schemes and you want to be explicit
                        // about them, you can use the "Schemes" option as shown below.
                        //
                        //c.Schemes(new[] { "http", "https" });

                        // Use "SingleApiVersion" to describe a single version API. Swagger 2.0 includes an "Info" object to
                        // hold additional metadata for an API. Version and title are required but you can also provide
                        // additional fields by chaining methods off SingleApiVersion.
                        //
                        c.SingleApiVersion("v1", "cmv.tecnologia.Openbanking")
                        .Description("Modeulo para OpenBanking")
                        .TermsOfService("Terminos del Servicio")
                        .Contact(x => x.Name("Jose Adrian Coria").Email("jcoria@cajamorelia.com.mx"));
                        c.IncludeXmlComments(string.Format(@"{0}\bin\cmv.tecnologia.Openbanking.xml", System.AppDomain.CurrentDomain.BaseDirectory));
                        c.OperationFilter<HeaderFilter>();

                    })
                .EnableSwaggerUi(c =>
                    {
                        c.CustomAsset("index", thisAssembly, "cmv.tecnologia.Openbanking.Resourses.index.html");
                        c.CustomAsset("logo_cmv", thisAssembly, "cmv.tecnologia.Openbanking.Resourses.logo_trans.png");
                        c.CustomAsset("favicon_cmv", thisAssembly, "cmv.tecnologia.Openbanking.Resourses.logo_banco.png");
                    });
        }
    }
}
