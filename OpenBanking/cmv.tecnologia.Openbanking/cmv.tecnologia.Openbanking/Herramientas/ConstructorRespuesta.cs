using cmv.tecnologia.Entidades;
using cmv.tecnologia.Entidades.Catalogos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cmv.tecnologia.Openbanking.Herramientas
{
    public static class ConstructorRespuesta<T>
    {
        /// <summary>
        /// Método para obtener una respuesta genérica
        /// </summary>
        /// <param name="Modelo"></param>
        /// <param name="Codigo"></param>
        /// <param name="Mensaje"></param>
        /// <returns></returns>
        public static WsRespuesta<T> CrearRespuesta(T Modelo, int Codigo, string Mensaje)
        {
            return new WsRespuesta<T>
            {
                Codigo = Codigo,
                Mensaje = Mensaje,
                Modelo = Modelo
            };
        }
        /// <summary>
        /// Método para obtener una respuesta correcta
        /// </summary>
        /// <param name="Modelo"></param>
        /// <returns></returns>
        public static WsRespuesta<T> CrearRespuestaCorrecta(T Modelo)
        {
            return new WsRespuesta<T>
            {
                Codigo = CatalogoRespuestas.SOLICITUD_CORRECTA.Codigo,
                Mensaje = CatalogoRespuestas.SOLICITUD_CORRECTA.Mensaje,
                Modelo = Modelo
            };
        }
        /// <summary>
        /// Método para la obtener una respuesta incorrecta
        /// </summary>
        /// <returns></returns>
        public static WsRespuesta<dynamic> CrearRespuestaIncorrecta()
        {
            return new WsRespuesta<dynamic>
            {
                Codigo = CatalogoRespuestas.SOLICITUD_INCORRECTA.Codigo,
                Mensaje = CatalogoRespuestas.SOLICITUD_INCORRECTA.Mensaje,
                Modelo = null
            };
        }
        /// <summary>
        /// Método para la obtener una respuesta errónea
        /// </summary>
        /// <param name="Modelo"></param>
        /// <param name="Mensaje"></param>
        /// <returns></returns>
        public static WsRespuesta<T> CrearRespuestaError(T Modelo, string Mensaje)
        {
            return new WsRespuesta<T>
            {
                Codigo = CatalogoRespuestas.ERROR_INTERNO.Codigo,
                Mensaje = Mensaje,
                Modelo = Modelo
            };
        }
    }
}