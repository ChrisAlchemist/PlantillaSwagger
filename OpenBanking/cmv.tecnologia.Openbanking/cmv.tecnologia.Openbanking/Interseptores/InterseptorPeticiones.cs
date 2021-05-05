using cmv.tecnologia.Entidades;
using log4net;
using log4net.Config;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

using System.Text;

namespace cmv.tecnologia.Openbanking.Interseptores
{
    public class InterseptorPeticiones : DelegatingHandler
    {
        private readonly ILog Log;

        public InterseptorPeticiones()
        {
            XmlConfigurator.Configure();
            Log = LogManager.GetLogger("All");
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken  cancellationToken) {
            var logMetadata = await BuildRequestMetadataAsync(request);
            var response = await base.SendAsync(request, cancellationToken);
            logMetadata = await BuildResponseMetadata(logMetadata, response);

            try
            {
                if (response.Content != null && response.Content.Headers.ContentType.MediaType == "application/json")
                {
                    WsRespuesta<dynamic> content = JsonConvert.DeserializeObject<WsRespuesta<dynamic>>(await response.Content.ReadAsStringAsync());
                    if(content.Codigo !=200 && content.Codigo != 0)
                    {
                        ImprimirLogError(logMetadata);
                        content.Modelo = null;
                        response.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

                    }
                    else
                    {
                        ImprimirLog(logMetadata);
                    }

                }
            }
            catch (Exception)
            {
                //Puede ignorarse el catch debido a que las peticiones del swagger tambien son interceptadas.
            }
            return response;
        }

        private async Task<LogMetadatos> BuildRequestMetadataAsync(HttpRequestMessage request)
        {
            LogMetadatos meta;
            try
            {
                meta = new LogMetadatos
                {
                    RequestMethod = request.Method.Method,
                    RequestTimestamp = DateTime.Now,
                    RequestUri = request.RequestUri.ToString(),
                    RequestBody = JsonConvert.DeserializeObject(await request.Content.ReadAsStringAsync())
                };
            }
            catch (Exception)
            {
                meta = new LogMetadatos
                {
                    RequestMethod = request.Method.Method,
                    RequestTimestamp = DateTime.Now,
                    RequestUri = request.RequestUri.ToString(),
                    RequestBody = JsonConvert.DeserializeObject(await request.Content.ReadAsStringAsync())
                };               
            }
            return meta;
        }

        private async Task<LogMetadatos> BuildResponseMetadata(LogMetadatos meta, HttpResponseMessage response)
        {
            meta.ResponseStatusCode = response.StatusCode;
            meta.ResponseTimestamp = DateTime.Now;
            meta.ResponseBody = response.Content != null && response.Content.Headers.ContentType.MediaType == "application/json" ?
                JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync()) :"";

            return meta;

        }
        private void ImprimirLogError(LogMetadatos logMetadatos)
        {
            Log.Error(JsonConvert.SerializeObject(logMetadatos, Formatting.Indented));
        }

        private void ImprimirLog(LogMetadatos logMetadatos)
        {
            Log.Info(JsonConvert.SerializeObject(logMetadatos, Formatting.Indented));
        }
    }
}