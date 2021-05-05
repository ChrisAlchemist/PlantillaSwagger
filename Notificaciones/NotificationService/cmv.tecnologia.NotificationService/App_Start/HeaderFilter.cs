using Swashbuckle.Swagger;
using System.Collections.Generic;
using System.Web.Http.Description;

namespace cmv.tecnologia.NotificationService.App_Start {
  public class HeaderFilter : IOperationFilter {
    public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription) {
      if (operation.parameters == null)
        operation.parameters = new List<Parameter>();

      operation.parameters.Add(new Parameter {
        name = "Authorization",
        @in = "header",
        type = "string",
        required = false
      });
    }
  }
}