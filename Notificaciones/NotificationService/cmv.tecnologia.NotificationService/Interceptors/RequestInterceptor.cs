using cmv.tecnologia.Entidades;
using log4net;
using log4net.Config;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace cmv.tecnologia.NotificationService.Interceptors {
  public class RequestInterceptor : DelegatingHandler {
    private static ILog log;
    public RequestInterceptor() {
      //Load log4net Configuration
      XmlConfigurator.Configure();
      //Get logger
      log = LogManager.GetLogger("AllLog");
    }
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
      var logMetadata = await BuildRequestMetadataAsync(request);
      var response = await base.SendAsync(request, cancellationToken);
      logMetadata = await BuildResponseMetadata(logMetadata, response);
      await ImprimirLog(logMetadata);
      return response;
    }
    private async Task<LogMetadatos> BuildRequestMetadataAsync(HttpRequestMessage request) {
      LogMetadatos log = new LogMetadatos {
        RequestMethod = request.Method.Method,
        RequestTimestamp = DateTime.Now,
        RequestUri = request.RequestUri.ToString(),
        RequestBody = JsonConvert.DeserializeObject(await request.Content.ReadAsStringAsync())
      };
      return log;
    }
    private async Task<LogMetadatos> BuildResponseMetadata(LogMetadatos logMetadata, HttpResponseMessage response) {
      logMetadata.ResponseStatusCode = response.StatusCode;
      logMetadata.ResponseTimestamp = DateTime.Now;
      logMetadata.ResponseBody = response.Content != null && response.Content.Headers.ContentType.MediaType == "application/json" ?
        JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync()) : "";
      return logMetadata;
    }
    private async Task<bool> ImprimirLog(LogMetadatos logMetadata) {
      log.Info(JsonConvert.SerializeObject(logMetadata, Formatting.Indented));
      return true;
    }
  }
}