using cmv.tecnologia.DAO;
using cmv.tecnologia.Openbanking.Herramientas;
using cmv.tecnologia.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace cmv.tecnologia.Openbanking.Controllers
{
    [RoutePrefix("v1/login")]
    public class LoginController : ApiController
    {

        private readonly LoginDAO DAO;
        public LoginController(LoginDAO DAO)
        {
            this.DAO = DAO;

        }

        [HttpGet]

        [Route("ping")]
        public IHttpActionResult Ping()
        {
            return Json(ConstructorRespuesta<dynamic>.CrearRespuestaCorrecta("PING !!"));
        }
        [HttpPost]
        [Route("auth")]
        public IHttpActionResult Autentificar([FromBody] LoginRequest request)
        {
            if(request== null)
            {
                return Json(ConstructorRespuesta<dynamic>.CrearRespuestaIncorrecta());
            }

            try
            {
                //verificar credenciales
                var token = GeneradorTokens.GenerarToken(request.Usuario);
                return Json(ConstructorRespuesta<dynamic>.CrearRespuestaCorrecta(new { JwtToken = token}));
            }
            catch (Exception e)
            {
                return Json(ConstructorRespuesta<dynamic>.CrearRespuestaError(e.StackTrace, e.Message));
            }
        }
    }
}
