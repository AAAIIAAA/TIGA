using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebTIGA.Models
{
    public class ContenedorModelos
    {
        public IEnumerable<WebTIGA.Models.SP_GRAF_TABLA_HORAS2_Result> SP_GRAF_TABLA_HORAS2 { get; set; }
        public IEnumerable<WebTIGA.Models.SP_TBL_DETALLE_GENERAL_TEC_Result> SP_TBL_DETALLE_GENERAL_TEC { get; set; }
        public IEnumerable<WebTIGA.Models.SP_TT_DETALLE_PROYECTO_ACTIVIDAD_Result> SP_TT_DETALLE_PROYECTO_ACTIVIDAD { get; set; }
        public IEnumerable<WebTIGA.Models.SP_TT_DETALLE_ETAPAS_FASES_ACTIVIDADES_Result> SP_TT_DETALLE_ETAPAS_FASES_ACTIVIDADES { get; set; }
        public IEnumerable<WebTIGA.Models.SP_TT_EQUIPO_ETAPAS_Result> SP_TT_EQUIPO_ETAPAS { get; set; }
   
        public IEnumerable<WebTIGA.Models.SP_TT_DIAS_CONTROL_Result> SP_TT_DIAS_CONTROL { get; set; }
        public IEnumerable<WebTIGA.Models.SP_TT_ERROR_FASES_DISTRIBUCION_Result> SP_TT_ERROR_FASES_DISTRIBUCION { get; set; }
        public IEnumerable<WebTIGA.Models.SP_GRAF_MEDIDA_DE_TIEMPO2_Result> SP_GRAF_MEDIDA_DE_TIEMPO2 { get; set; }

        public IEnumerable<WebTIGA.Models.basesBCHojaTrabajo> BCHojaTrabajos { get; set; }
        public IEnumerable<WebTIGA.Models.SP_BC_BG_INFORME_Result> SP_BC_BG_INFORME { get; set; }
        public IEnumerable<WebTIGA.Models.SP_BC_GYP_INFORME_Result> SP_BC_GYP_INFORME { get; set; }
        public IEnumerable<WebTIGA.Models.BC_Cuentas_Proyectos> BC_Cuentas_Proyectos { get; set; }
        public IEnumerable<WebTIGA.Models.View_BC_Cuentas_Proyectos> VIEW_BC_CUENTAS_PROYECTOS { get; set; }
        public IEnumerable<WebTIGA.Models.View_BC_Hojas_Cargadas> VIEW_BC_HOJAS_CARGADAS { get; set; }
        public IEnumerable<WebTIGA.Models.SP_BC_PROYECTO_PLAN_Result> SP_BC_PROYECTO_PLAN { get; set; }
        public IEnumerable<WebTIGA.Models.SP_BC_HOJA_PERIODO_Result> SP_BC_HOJA_PERIODO { get; set; }
        public IEnumerable<WebTIGA.Models.SP_BC_RESUMEN_CCSS_Result> SP_BC_RESUMEN_CCSS { get; set; }
        public IEnumerable<WebTIGA.Models.SP_BC_CUENTAS_CONTABLES_Result> SP_BC_CUENTAS_CONTABLES { get; set; }
        public IEnumerable<WebTIGA.Models.SP_BC_CUENTAS_SIGNIFICATIVAS_Result> SP_BC_CUENTAS_SIGNIFICATIVAS { get; set; }
        public IEnumerable<WebTIGA.Models.DD_Proyecto> DD_Proyecto { get; set; }

        //public IEnumerable<WebTIGA.Models.SP_RA_REPORTE_GENERAL_Result> SP_RA_REPORTE_GENERAL { get; set; }
       // public IEnumerable<WebTIGA.Models.SP_RA_REPORTE_CLAVE_Result> SP_RA_REPORTE_CLAVE { get; set; }
       // public IEnumerable<WebTIGA.Models.SP_RA_REPORTE_SOX_Result> SP_RA_REPORTE_SOX { get; set; }
       // public IEnumerable<WebTIGA.Models.SP_RA_REPORTE_OBS_CERRADAS_Result> SP_RA_REPORTE_OBS_CERRADAS { get; set; }
        public IEnumerable<WebTIGA.Models.SP_RA_REPORTE_EFECT_CONTROL_Result> SP_RA_REPORTE_EFECT_CONTROL { get; set; }
        public IEnumerable<WebTIGA.Models.SP_RA_REPORTE_STATUS_ACTIVIDADES_Result> SP_RA_REPORTE_STATUS_ACTIVIDADES { get; set; }
        public IEnumerable<WebTIGA.Models.SP_RA_REPORTE_MENSUAL_CREDICORP_Result> SP_RA_REPORTE_MENSUAL_CREDICORP { get; set; }
        public IEnumerable<WebTIGA.Models.SP_RA_REPORTE_PROYECTOS_RIESGO_ALTO_Result> SP_RA_REPORTE_PROYECTOS_RIESGO_ALTO { get; set; }
        public IEnumerable<WebTIGA.Models.SP_RA_REPORTE_OBS_RIESGO_ALTO_Result> SP_RA_REPORTE_OBS_RIESGO_ALTO { get; set; }



        public IEnumerable<WebTIGA.Models.DPA_Negocio> DPA_Negocio { get; set; }
        public IEnumerable<WebTIGA.Models.DPA_Plan_Anual> DPA_Plan_Anual { get; set; }
        public IEnumerable<WebTIGA.Models.DPA_Evaluacion> DPA_Evaluacion { get; set; }
        public IEnumerable<WebTIGA.Models.DPA_Evaluacion_Plan> DPA_Evaluacion_Plan { get; set; }
        public IEnumerable<WebTIGA.Models.DPA_Log_Evaluacion_Plan> DPA_Log_Evaluacion_Plan { get; set; }
        public IEnumerable<WebTIGA.Models.DPA_Votacion> DPA_Votacion { get; set; }
        public IEnumerable<WebTIGA.Models.DPA_Opcion_Votacion> DPA_Opcion_Votacion { get; set; }
        public IEnumerable<WebTIGA.Models.DPA_Criterio_Votacion> DPA_Criterio_Votacion { get; set; }
        public IEnumerable<WebTIGA.Models.DPA_Scoring_Evaluacion> DPA_Scoring_Evaluacion { get; set; }
        public IEnumerable<WebTIGA.Models.DPA_Criterio_Scoring> DPA_Criterio_Scoring { get; set; }
        public IEnumerable<WebTIGA.Models.DD_ProcesoEvaluado> DD_ProcesoEvaluado { get; set; }
        public IEnumerable<WebTIGA.Models.DPA_Auditor_Evaluacion> DPA_Auditor_Evaluacion { get; set; }
        public IEnumerable<WebTIGA.Models.DPA_Auditor_Tiempo_Extra> DPA_Auditor_Tiempo_Extra { get; set; }
        public IEnumerable<WebTIGA.Models.DPA_Tipo_Tiempo_Extra> DPA_Tipo_Tiempo_Extra { get; set; }
        public IEnumerable<WebTIGA.Models.DPA_Equipo> DPA_Equipo { get; set; }
        public IEnumerable<WebTIGA.Models.DPA_Riesgo_Asociado> DPA_Riesgo_Asociado { get; set; }

        public IEnumerable<WebTIGA.Models.SP_DPA_Carga_Plan_Result> SP_DPA_Carga_Plan { get; set; }
        public IEnumerable<WebTIGA.Models.SP_DPA_Mantenimiento_Plan_Result> SP_DPA_Mantenimiento_Plan { get; set; }





        public IEnumerable<WebTIGA.Models.fn_Stock_Observaciones_Integrado_v7_Result> fn_Stock_Observaciones_Integrado_v7 { get; set; }

        public IEnumerable<WebTIGA.Models.fn_Detalle_Ampliaciones_Result> fn_Detalle_Ampliaciones { get; set; }
        public IEnumerable<WebTIGA.Models.fn_Detalle_Acciones_Obs_Result> fn_Detalle_Acciones_Obs { get; set; }
        public IEnumerable<WebTIGA.Models.TG_DD_Proyecto_Lis_Result> TG_DD_Proyecto_Lis { get; set; }
        public IEnumerable<WebTIGA.Models.TG_Proyecto> TG_Proyecto { get; set; }
        public IEnumerable<WebTIGA.Models.TG_Proyecto_Riesgo_Control> TG_Proyecto_Riesgo_Control { get; set; }
        //public IEnumerable<WebTIGA.Models.fn_TG_Obtener_Proyectos_v1_Result> fn_TG_Obtener_Proyectos_v1 { get; set; }
        public IEnumerable<WebTIGA.Models.fn_TG_Obtener_Proyectos_v2_Result> fn_TG_Obtener_Proyectos_v2 { get; set; }

        //public IEnumerable<WebTIGA.Models.SP_DD_Proyecto_Obtener_detalle_v2_Result> SP_DD_Proyecto_Obtener_detalle_v2 { get; set; }
        public IEnumerable<WebTIGA.Models.SP_DD_Proyecto_Obtener_detalle_v3_Result> SP_DD_Proyecto_Obtener_detalle_v3 { get; set; }
        //public IEnumerable<WebTIGA.Models.SP_DD_Proyecto_Obtener_detalle_v5_Result> SP_DD_Proyecto_Obtener_detalle_v5 { get; set; }
        public IEnumerable<WebTIGA.Models.SP_DD_Proyecto_Obtener_detalle_v7_Result> SP_DD_Proyecto_Obtener_detalle_v7 { get; set; }
        public IEnumerable<WebTIGA.Models.SP_DPA_Log_Plan_Result> SP_DPA_Log_Plan { get; set; }
        public IEnumerable<WebTIGA.Models.SP_DPA_Log_Evaluacion_Result> SP_DPA_Log_Evaluacion { get; set; }

        public IEnumerable<WebTIGA.Models.SP_DPA_Log_Evaluacion_Plan_Result> SP_DPA_Log_Evaluacion_Plan { get; set; }
        public IEnumerable<WebTIGA.Models.SP_DPA_Votacion_Evaluacion_Result> SP_DPA_Votacion_Evaluacion { get; set; }

        public IEnumerable<WebTIGA.Models.SP_DPA_Votacion_Evaluacion_v2_Result> SP_DPA_Votacion_Evaluacion_v2 { get; set; }
        public IEnumerable<WebTIGA.Models.SP_DPA_Votacion_Evaluacion_v31_Result> SP_DPA_Votacion_Evaluacion_v3 { get; set; }

        public IEnumerable<WebTIGA.Models.SP_DPA_Votacion_Auditor_Result> SP_DPA_Votacion_Auditor { get; set; }
        public IEnumerable<WebTIGA.Models.SP_DPA_Votacion_Auditor_v2_Result> SP_DPA_Votacion_Auditor_v2 { get; set; }

        public IEnumerable<WebTIGA.Models.SP_DPA_Fechas_Auditor_Result> SP_DPA_Fechas_Auditor { get; set; }
        public IEnumerable<WebTIGA.Models.SP_DPA_Fechas_No_Laborables_Result> SP_DPA_Fechas_No_Laborables { get; set; }
        public IEnumerable<WebTIGA.Models.SP_DPA_View_Asignar_Proyecto_Result> SP_DPA_View_Asignar_Proyecto { get; set; }
        public IEnumerable<WebTIGA.Models.SP_DPA_Programacion_Final_Result> SP_DPA_Programacion_Final { get; set; }



        public IEnumerable<WebTIGA.Models.Persona> Persona { get; set; }
        public IEnumerable<WebTIGA.Models.TG_Estados_Proyecto> TG_Estados_Proyecto { get; set; }
        public IEnumerable<WebTIGA.Models.Usuario> Usuario { get; set; }
        public IEnumerable<WebTIGA.Models.Usuario_Log> UsuarioLog { get; set; }
        public IEnumerable<WebTIGA.Models.UsuarioModuloRol> UsuarioModuloRol { get; set; }
        public IEnumerable<WebTIGA.Models.Rol> Rol { get; set; }
        public IEnumerable<WebTIGA.Models.Modulo> Modulo1 { get; set; }
        public IEnumerable<WebTIGA.Models.ModuloRol> ModuloRol { get; set; }
        public IEnumerable<WebTIGA.Models.VIEW_USUARIOS_FORGOT_PASSWORD> VIEW_USUARIOS_FORGOT_PASSWORD { get; set; }
        public IEnumerable<WebTIGA.Models.SP_MODULOS_USUARIOS_Result> SP_MODULOS_USUARIOS_Result { get; set; }
        public IEnumerable<WebTIGA.Models.SP_WT_ADMINISTRADOR_USUARIOS_Result> SP_WT_ADMINISTRADOR_USUARIOS_Result { get; set; }
        public IEnumerable<WebTIGA.Models.VIEW_WT_USUARIOS> VIEW_WT_USUARIOS { get; set; }
        public IEnumerable<WebTIGA.Models.SP_WT_ROLES_MODULO_Result> SP_WT_ROLES_MODULO_Result { get; set; }
        public IEnumerable<WebTIGA.Models.TS_Estructura> TS_Estructura { get; set; }
        public IEnumerable<WebTIGA.Models.TS_Observacion_Cambio> TS_Observacion_Cambio { get; set; }


        public IEnumerable<WebTIGA.Models.TG_Observacion_Cambio> TG_Observacion_Cambio { get; set; }
        public IEnumerable<WebTIGA.Models.TG_Aprobador> TG_Aprobador { get; set; }
        public IEnumerable<WebTIGA.Models.TG_Ampliación> TG_Ampliación { get; set; }
      //  public IEnumerable<WebTIGA.Models.VIEW_NEGOCIOS> VIEW_NEGOCIOS { get; set; }
        public IEnumerable<WebTIGA.Models.TG_Actividad_Justificacion> TG_Actividad_Justificacion { get; set; }
        public IEnumerable<WebTIGA.Models.TG_RiesgoBCP> TG_RiesgoBCP { get; set; }
        public IEnumerable<WebTIGA.Models.TG_Proceso_Evaluado> TG_Proceso_Evaluado { get; set; }
        public IEnumerable<WebTIGA.Models.TG_RiesgoInherente> TG_RiesgoInherente { get; set; }
        public IEnumerable<WebTIGA.Models.TG_AdministraciónDelRiesgo> TG_AdministraciónDelRiesgo { get; set; }
        public IEnumerable<WebTIGA.Models.TG_Persona> TG_Persona { get; set; }
        public IEnumerable<WebTIGA.Models.TG_Riesgo_Del_Trabajo> TG_Riesgo_Del_Trabajo { get; set; }
        public IEnumerable<WebTIGA.Models.TG_Informe> TG_Informe { get; set; }

        //public IEnumerable<WebTIGA.Models.SP_DD_Proyecto_Actividad_Informe_Obtener_v2_Result> SP_DD_Proyecto_Actividad_Informe_Obtener_v2 { get; set; }
        //public IEnumerable<WebTIGA.Models.SP_DD_Proyecto_Actividad_Informe_Obtener_v3_Result> SP_DD_Proyecto_Actividad_Informe_Obtener_v3 { get; set; }
        public IEnumerable<WebTIGA.Models.SP_DD_Proyecto_Actividad_Informe_Obtener_v4_Result> SP_DD_Proyecto_Actividad_Informe_Obtener_v4 { get; set; }

      
        public IEnumerable<WebTIGA.Models.Compañia> Compañia { get; set; }
        public IEnumerable<WebTIGA.Models.Rotacion> Rotacion { get; set; }
        public IEnumerable<WebTIGA.Models.View_AU_Persona_Usuario_Rotacion> View_AU_Persona_Usuario_Rotacion { get; set; }
        public IEnumerable<WebTIGA.Models.View_AU_Administrador_Usuarios> View_AU_Administrador_Usuarios { get; set; }
        public IEnumerable<WebTIGA.Models.View_AU_Drop_UsuariosTM> View_AU_Drop_UsuariosTM { get; set; }
        public IEnumerable<WebTIGA.Models.View_AU_Drop_JefeDirecto> View_AU_Drop_JefeDirectos { get; set; }
        public IEnumerable<WebTIGA.Models.SP_AU_UsuariosTM_Result> SP_AU_UsuariosTM { get; set; }
        public IEnumerable<WebTIGA.Models.SP_AU_Rotacion_Usuario_Result> SP_AU_Rotacion_Usuario { get; set; }

        public IEnumerable<WebTIGA.Models.TS_Rol> TS_Rol { get; set; }
        public IEnumerable<WebTIGA.Models.TG_Rol> TG_Rol { get; set; }
        public IEnumerable<WebTIGA.Models.TM_User> TM_User { get; set; }
        public IEnumerable<WebTIGA.Models.pa_man_DS_Estructura_Contactos_Result> DS_Estructura_Contactos { get; set; }

        public IEnumerable<WebTIGA.Models.pa_man_TG_Estructura_Contactos_Result> TG_Estructura_Contactos { get; set; }


        public IEnumerable<WebTIGA.Models.SP_AU_Drop_ContactosTM_Result> sP_AU_Drop_Contactos { get; set; }

        public IEnumerable<WebTIGA.Models.TG_AU_Drop_ContactosTM4_Result> TG_AU_Drop_Contactos { get; set; }









        public IEnumerable<WebTIGA.Models.View_DAC_Vista_List_Scripts> VIEW_LISTA_SCRIPTS { get; set; }


     





        public IEnumerable<WebTIGA.Models.SP_WT_REPORTES_Result1> SP_WT_REPORTES { get; set; }
     
        public IEnumerable<WebTIGA.Models.SP_WT_REPORTE_SOX_Result> SP_WT_REPORTE_SOX { get; set; }
        public IEnumerable<WebTIGA.Models.SP_WT_VACACIONES_CONSOLIDADO_Result> SP_WT_VACACIONES_CONSOLIDADO { get; set; }
        public IEnumerable<WebTIGA.Models.SP_WT_GANTT_VACACIONES_Result> SP_WT_GANTT_VACACIONES { get; set; }

        public IEnumerable<WebTIGA.Models.Usuarios_Objetivos> Usuarios_Objetivos { get; set; }
        public IEnumerable<WebTIGA.Models.SP_WT_USUARIOS_ID_Result> SP_WT_USUARIOS_ID { get; set; }
        public IEnumerable<WebTIGA.Models.SP_VACACIONES_REGISTRO_Result> SP_VACACIONES_REGISTRO { get; set; }
        public IEnumerable<WebTIGA.Models.SP_VACACIONES_REGISTRO_Result> SP_VACACIONES_EJECUTADA { get; set; }
        public IEnumerable<WebTIGA.Models.SP_VACACIONES_REGISTRO_Result> SP_VACACIONES_SISTEMA { get; set; }
        public IEnumerable<WebTIGA.Models.TD_VacacionesRegistro> TD_VacacionesRegistro { get; set; }

        public IEnumerable<WebTIGA.Models.SP_VACACIONES_REGISTRO2_Result> SP_VACACIONES_REGISTRO2 { get; set; }
        public IEnumerable<WebTIGA.Models.SP_VACACIONES_REGISTRO2_Result> SP_VACACIONES_EJECUTADA2 { get; set; }
        public IEnumerable<WebTIGA.Models.SP_VACACIONES_REGISTRO2_Result> SP_VACACIONES_SISTEMA2 { get; set; }
        public IEnumerable<WebTIGA.Models.SP_WT_CONTROL_PERIODO_DIAS_NUEVO_Result> SP_WT_CONTROL_PERIODO_DIAS_NUEVO { get; set; }
        public IEnumerable<WebTIGA.Models.SP_WT_VACACIONES_CONSOLIDADO_DETALLE_Result> SP_WT_VACACIONES_CONSOLIDADO_DETALLE { get; set; }

        public IEnumerable<WebTIGA.Models.SP_RE_EVOLUTIVO_VENCIDAS2_Result> SP_RE_EVOLUTIVO_VENCIDAS2 { get; set; }
        public IEnumerable<WebTIGA.Models.SP_RE_EVOLUTIVO_AUDIT_VENCIDAS_BASE_Result> SP_RE_EVOLUTIVO_AUDIT_VENCIDAS_BASE { get; set; }
        public IEnumerable<WebTIGA.Models.SP_RE_EVOLUTIVO_VENCIDAS_EQUIPO_BASE_Result> SP_RE_EVOLUTIVO_VENCIDAS_EQUIPO_BASE { get; set; }
        public IEnumerable<WebTIGA.Models.SP_RE_EVOLUTIVO_TOP_Result> SP_RE_EVOLUTIVO_TOP { get; set; }
   
        public IEnumerable<WebTIGA.Models.SP_WT_RA_SEGUIMIENTO_OBSERVACIONES_Result> SP_WT_SEGUIMIENTO_OBSERVACIONES { get; set; }
        public IEnumerable<WebTIGA.Models.SP_WT_RA_SEGUIMIENTO_OBSERVACIONES_2_Result> SP_WT_SEGUIMIENTO_OBSERVACIONES_2 { get; set; }


        public IEnumerable<WebTIGA.Models.SP_WT_RA_DETALLE_SEGUIMIENTO_OBSERVACIONES_1_Result> SP_WT_DETALLE_SEGUIMIENTO_OBSERVACIONES_1 { get; set; }
        public IEnumerable<WebTIGA.Models.SP_WT_RA_DETALLE_SEGUIMIENTO_OBSERVACIONES_2_Result> SP_WT_DETALLE_SEGUIMIENTO_OBSERVACIONES_2 { get; set; }


        public IEnumerable<WebTIGA.Models.SP_WT_RA_DETALLE_SEGUIMIENTO_OBSERVACIONES_DISTRIBUCION_EMISOR_2_Result> SP_WT_DETALLE_SEGUIMIENTO_OBSERVACIONES_DISTRIBUCION_EMISOR_2 { get; set; }
        public IEnumerable<WebTIGA.Models.SP_WT_RA_DETALLE_SEGUIMIENTO_OBSERVACIONES_DISTRIBUCION_ESTADO_RIESGO_Result> SP_WT_DETALLE_SEGUIMIENTO_OBSERVACIONES_DISTRIBUCION_ESTADO_RIESGO { get; set; }

        public IEnumerable<WebTIGA.Models.SP_WT_RA_DETALLE_SEGUIMIENTO_OBSERVACIONES_STOCK_ANTIGUEDAD_EMISION_Result> SP_WT_DETALLE_SEGUIMIENTO_OBSERVACIONES_STOCK_ANTIGUEDAD_EMISION { get; set; }

        public IEnumerable<WebTIGA.Models.SP_WT_RA_DETALLE_SEGUIMIENTO_OBSERVACIONES_STOCK_ANTIGUEDAD_CRITICIDAD_Result> SP_WT_DETALLE_SEGUIMIENTO_OBSERVACIONES_STOCK_ANTIGUEDAD_CRITICIDAD { get; set; }

        public IEnumerable<WebTIGA.Models.SP_WT_RA_DETALLE_SEGUIMIENTO_OBSERVACIONES_STOCK_ANTIGUEDAD_EMISOR_2_Result> SP_WT_DETALLE_SEGUIMIENTO_OBSERVACIONES_STOCK_ANTIGUEDAD_EMISOR_2 { get; set; }


        public IEnumerable<WebTIGA.Models.DAC_Scripts> DAC_Scripts { get; set; }

        public IEnumerable<WebTIGA.Models.SP_DAC_SCRIPTS_POR_ESTADOS_Result> SP_DAC_SCRIPTS_POR_ESTADOS { get; set; }
        public IEnumerable<WebTIGA.Models.R1_DupNAB_> R1_DupNAB_ { get; set; }


        public IEnumerable<WebTIGA.Models.DAC_HistorialCambios> DAC_HistorialCambios { get; set; }

        public IEnumerable<WebTIGA.Models.DD_PlanAnual> DD_PlanAnual { get; set; }
        public IEnumerable<WebTIGA.Models.DD_Universo> DD_Universo { get; set; }
        public IEnumerable<WebTIGA.Models.DD_Cambios_Universo> DD_Cambios_Universo { get; set; }
        public IEnumerable<WebTIGA.Models.SP_WT_WAC_SELECT_SCRIPTS_Result> SP_WT_WAC_SELECT_SCRIPTS_Result { get; set; }
        public IEnumerable<WebTIGA.Models.DAC_HistorialReportesScript> DAC_HistorialReportesScript { get; set; }

        public IEnumerable<WebTIGA.Models.DAC_ScriptsContinua> DAC_ScriptsContinua { get; set; }
        public IEnumerable<WebTIGA.Models.TS_Unidad_Responsable> TS_Unidad_Responsable { get; set; }

        public IEnumerable<WebTIGA.Models.TG_Unidad_Responsable> TG_Unidad_Responsable { get; set; }

   

        public IEnumerable<WebTIGA.Models.TS_Unidad_Responsable_Contacto> TS_Unidad_Responsable_Contacto { get; set; }
        public IEnumerable<WebTIGA.Models.TS_Contacto> TS_Contacto { get; set; }
        public IEnumerable<WebTIGA.Models.TG_Contacto> TG_Contacto { get; set; }


        public IEnumerable<WebTIGA.Models.fn_TG_Get_Mapa_Aseguramiento_v3_Result> fn_TG_Get_Mapa_Aseguramiento_v3_Result { get; set; }


    }




    public class JsonGRAFTiempos
    {
        public Int32? Porcentaje { get; set; }
    }
    public class JsonGRAFDiasxFase
    {
        public string Equipo { get; set; }

    }

    public class Reporte_script
    {
        public string nombre { get; set; }
        public Nullable<int> total { get; set; }
    }


    public class RecoveryViewModel
    {
        [Required]
        public string Usuario { get; set; }

        PROYECTOSIAV2Entities1 user = new PROYECTOSIAV2Entities1();

        public bool validar_usuario()

        {
            //var ContraseñaEncrypt = Encrypt.Base64_Encode(Contraseña);

            var query = from u in user.Usuario
                        where u.Usuario1 == Usuario
                        select u;



            if (query.Count() > 0)
            {

                //var query2 = from u in user.DACW_Usuario_Login where u.Usuario == Usuario select u;
                var datos = query.ToList();
                foreach (var Data in datos)
                {

                    Usuario = Data.Usuario1;
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class RecoveryPasswordViewModel
    {
        public string token { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "El número de caracteres de {0} debe ser al menos de 6 caracteres")]
        public string Password { get; set; }

        [Compare("Password")]
        [Required]
        [StringLength(100, ErrorMessage = "El número de caracteres de {0} debe ser al menos de 6 caracteres")]
        public string Password2 { get; set; }
    }

    public class UsuarioModuloRolViewModel1
    {

        public int IdUsuario { get; set; }
        public int IdModulo { get; set; }
        public int IdRol { get; set; }
        public int Estado { get; set; }

        public virtual ModuloRol ModuloRol { get; set; }
        public virtual Usuario Usuario { get; set; }

    }

    /*prueba de correo de notificaciones*/
    public class CorreoModel
    {
        [Required, Display(Name = "Correo Destinatario"), EmailAddress]
        public string Para { get; set; }
        [Required]
        public string Asunto { get; set; }
        public string Cuerpo { get; set; }
        //public HttpPostedFileBase Adjunto { get; set; }

        [Required, Display(Name = "Correo Remitente"), EmailAddress]
        public string Correo { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Pass { get; set; }

        public string emailsol { get; set; }

        public string emialsup { get; set; }
    }

}