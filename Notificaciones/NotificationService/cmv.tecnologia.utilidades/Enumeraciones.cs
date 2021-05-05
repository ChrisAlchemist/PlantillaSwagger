using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cmv.tecnologia.utilidades
{
    public class Enumeraciones
    {
        public enum CodigoEnviaMensajeOL
        {
            Enviado = 3,
            Cancelado = 4,
            Error = 5,
            No_móvil = 6,
            Número_inválido = 10,
            No_se_encuentra_la_plantilla = 16,
            En_lista_negra = 19,
            Lista_Negra_no_disponible = 22,
            No_registrado = 49,
            En_registro = 50,
            Eliminado = 51,
            Error_en_registro = 52,
            Falta_de_saldo_en_el_servicio = 101,
            //Falta_de_saldo_en_el_servicio = 199,
            Error_general = -1,
            Error_de_autenticación_Usuario, _contraseña_o_id_Cliente = -3,
            Error_al_intentar_enviar_el_mensaje, _es_un_error_temporal, _se_recomienda_reintentar = -200,
            Tipo_de_envío_no_permitido = -201,
            Error_al_procesar_el_tamaño_del_contenido_multimedia_de_la_url_proporcionada = -202,
            La_multimedia_es_mayor_a_el_máximo = -203,
            Error_al_consultar_la_configuración_de_la_ruta_de_origen_Reintentar = -204,
            Usuario_o_contraseña_invalida = -101,
            Usuario_no_valido = -100,

        }

        public enum CodigoEnviaEmail
        {
            Los_correos_de_TO_o_FROM_son_inválidos = -10,
            Lista_Negra = -19,
            Usuario_no_valido = -100,
            No_se_tiene_saldo_suficiente = -141,
            No_se_pudo_obtener_el_id_de_Nodejs = -200 ,
            No_se_pudo_insertar_el_ID_SMPP = -300,
            Error_general = -400,
            Timeout =-424,
            //idenvio Id_único_que_hace_referencia_al_envió
        }

        public enum CodigoEstadoEnvioMail
        {
            El_estado_de_este_correo_ya_no_está_disponible = 0  ,
            En_progreso                                    = 2  ,
            Enviado                                        = 3  ,
            Cancelado                                      = 4  ,
            Error                                          = 5  ,
            Máximo_número_de_intentos                      = 11 ,
            Formato_inválido                               = 16 ,
            Lista_Negra                                    = 19 ,
            Lista_negra_no_disponible                      = 22 ,
            Enviado_sin_archivo_adjunto                    = 30 ,
            No_enviado_por_archivo_adjunto_faltante        = 31 ,
            Rebotado                                       = 32 ,
            R_permanente                                   = 33 ,
            R_No_existe_la_dirección                       = 34 ,
            R_Temporal                                     = 35 ,
            R_Dominio_no_encontrado                        = 36 ,
            R_Bandeja_llena                                = 37 ,
            R_Mensaje_muy_largo                            = 38 ,
            R_Contenido_rechazado                          = 39 ,
            R_Adjunto_rechazado                            = 40 ,
            R_Max_intentos                                 = 41 ,
            Spam                                           = 42 ,
            S_Correo_no_solicitado_Si                      = 43 ,
            S_Autenticación_errónea                        = 44 ,
            S_Fraude_phishing                              = 45 ,
            No_es_spam                                     = 46 ,
            S_Virus                                        = 47 ,
            Falta_de_saldo_en_el_servicio                  = 101
            //Falta_de_saldo_en_el_servicio                  = 199
            

        }
    }
}