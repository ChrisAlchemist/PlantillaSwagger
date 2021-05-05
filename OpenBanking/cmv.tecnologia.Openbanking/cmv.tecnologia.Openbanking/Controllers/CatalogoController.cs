using cmv.tecnologia.DAO;
using cmv.tecnologia.Openbanking.Herramientas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace cmv.tecnologia.Openbanking.Controllers
{
    [Authorize]
    [RoutePrefix("v1/catalogo")]
    public class CatalogoController : ApiController
    {
        private readonly CatalogoDAO DAO;

        public CatalogoController(CatalogoDAO DAO)
        {
            this.DAO = DAO;

        }

        [HttpGet]
        [Route("nombre")]
        public IHttpActionResult CatalogoNombres()
        {
            try
            {
                var result = DAO.ObtenerNombres(out dynamic Estatus);
                return Json(ConstructorRespuesta<dynamic>.CrearRespuesta(result,Estatus.codigo, Estatus.Mensaje));
            }
            catch (Exception e)
            {

                return Json(ConstructorRespuesta<dynamic>.CrearRespuestaError(e.StackTrace, e.Message));
            }
        }
    }
}
