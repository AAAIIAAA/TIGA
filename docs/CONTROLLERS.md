# CONTROLLERS.md - Documentación de Controladores

## 📋 Índice
1. [Introducción](#introducción)
2. [Arquitectura de Controladores](#arquitectura-de-controladores)
3. [Controladores Principales](#controladores-principales)
4. [Flujos de Trabajo](#flujos-de-trabajo)
5. [Patrones y Convenciones](#patrones-y-convenciones)
6. [Autorización y Seguridad](#autorización-y-seguridad)
7. [Manejo de Errores](#manejo-de-errores)

---

## Introducción

Los controladores en TIGA implementan el patrón MVC de ASP.NET, gestionando las solicitudes HTTP, coordinando la lógica de negocio y retornando las vistas apropiadas. Todos los controladores heredan de `Controller` y están organizados por módulos funcionales.

## Arquitectura de Controladores

### Estructura Base

```csharp
[Logueado]  // Atributo de autorización personalizado
public class NombreController : Controller
{
    // Instancias comunes
    SesionData session = new SesionData();
    PROYECTOSIAV2Entities1 db2 = new PROYECTOSIAV2Entities1();
    ContenedorModelos modelDB = new ContenedorModelos();
    
    // Métodos de acción
    public ActionResult Index() { }
}
```

### Componentes Comunes

Todos los controladores utilizan:
- **SesionData**: Gestión de sesiones de usuario
- **PROYECTOSIAV2Entities1**: Context de Entity Framework
- **ContenedorModelos**: Contenedor para múltiples modelos
- **ViewBag**: Datos adicionales para vistas

---

## Controladores Principales

### 1. LoginController

**Responsabilidad**: Autenticación de usuarios y recuperación de contraseñas.

**Ubicación**: `Controllers/LoginController.cs`

**No requiere autorización**: `[AllowAnonymous]` en sus acciones principales.

#### Métodos Principales

##### 1.1 Login_ (GET)
```csharp
[AllowAnonymous]
public ActionResult Login_(string comunicado, string returnUrl)
```
**Propósito**: Muestra el formulario de inicio de sesión.

**Parámetros**:
- `comunicado`: Mensaje opcional para mostrar al usuario
- `returnUrl`: URL para redireccionar después del login

**Lógica**:
```csharp
ViewBag.ReturnUrl = returnUrl;
ViewBag.Comunicado = comunicado ?? "Bienvenido, para acceder ingresar su usuario y contraseña...";
return View("Login_");
```

##### 1.2 Login_ (POST)
```csharp
[HttpPost]
[AllowAnonymous]
public async Task<ActionResult> Login_(UserLogin datos, string returnUrl)
```
**Propósito**: Procesa el inicio de sesión.

**Flujo de trabajo**:
```
1. Validar ModelState
   ↓
2. Validar credenciales (datos.logeo())
   ↓
3. Crear sesión (setSession)
   ↓
4. Registrar log de acceso (Usuario_Log)
   ↓
5. Redireccionar a Home o returnUrl
```

**Código clave**:
```csharp
if (datos.logeo() == true)
{
    session.setSession("usuario", datos.Nombres + " " + datos.Apellidos);
    session.setSession("rol", datos.Rol.ToString());
    session.setSession("IdUser", datos.ID.ToString());
    session.setSession("Modulo", datos.Modulo.ToString());
    
    // Registrar acceso
    Usuario_Log usuarioLog = new Usuario_Log()
    {
        ID_Usuario = Convert.ToInt32(Session["IdUser"]),
        Timestamp = DateTime.Now
    };
    db2.Usuario_Log.Add(usuarioLog);
    db2.SaveChanges();
    
    return RedirectToAction("Index", "Home");
}
```

##### 1.3 Forgot_Password (GET)
```csharp
[HttpGet]
public ActionResult Forgot_Password()
```
**Propósito**: Muestra formulario de recuperación de contraseña.

##### 1.4 Forgot_Password (POST)
```csharp
[HttpPost]
public ActionResult Forgot_Password(RecoveryViewModel model)
```
**Propósito**: Procesa solicitud de recuperación de contraseña.

**Flujo**:
```
1. Validar usuario y email
   ↓
2. Generar token único (SHA256)
   ↓
3. Guardar token en BD
   ↓
4. Enviar email con enlace de recuperación
   ↓
5. Redireccionar con mensaje de confirmación
```

**Generación de Token**:
```csharp
string token = GetSha256(Guid.NewGuid().ToString());
var oUser = db2.Usuario.Where(z => z.Usuario1 == model.Usuario).FirstOrDefault();

if (oUser != null)
{
    oUser.token_recovery = token;
    db2.Entry(oUser).State = System.Data.Entity.EntityState.Modified;
    db2.SaveChanges();
    SendEmail(datoemail, token, nombre);
}
```

##### 1.5 Recovery (GET)
```csharp
[HttpGet]
public ActionResult Recovery(string token)
```
**Propósito**: Valida token y muestra formulario de nueva contraseña.

**Validación de Token**:
```csharp
var oUser = db2.Usuario.Where(d => d.token_recovery == model.token).FirstOrDefault();
if (oUser == null)
{
    ViewBag.Error = "Tu token ha expirado";
    return RedirectToAction("Notificacion_token", "Login");
}
```

##### 1.6 SendEmail
```csharp
private void SendEmail(string email, string token, string nombre)
```
**Propósito**: Envía correo de recuperación de contraseña.

**Configuración de Email**:
```csharp
MailMessage mail = new MailMessage();
mail.To.Add(email);
mail.From = new MailAddress("correo@empresa.com");
mail.Subject = "Recuperación de Contraseña";
mail.Body = $"Hola {nombre}, haz clic en el siguiente enlace: {urlDomain}Login/Recovery?token={token}";
mail.IsBodyHtml = true;

SmtpClient smtp = new SmtpClient();
smtp.Host = "smtp.office365.com";
smtp.Port = 587;
smtp.UseDefaultCredentials = false;
smtp.Credentials = new NetworkCredential("usuario", "password");
smtp.EnableSsl = true;
smtp.Send(mail);
```

---

### 2. HomeController

**Responsabilidad**: Pantalla principal y navegación del sistema.

**Ubicación**: `Controllers/HomeController.cs`

**Requiere autorización**: `[Logueado]`

#### Métodos Principales

##### 2.1 Index
```csharp
[Logueado]
public ActionResult Index()
```
**Propósito**: Dashboard principal del usuario autenticado.

**Lógica**:
```csharp
var usuario = Convert.ToInt32(Session["IdUser"]);
var nombre = Convert.ToString(Session["usuario"]);

ViewBag.Bienvenido = "Bienvenid@";
ViewBag.nombre = nombre;

// Obtener módulos del usuario
modelDB.SP_MODULOS_USUARIOS_Result = db2.SP_MODULOS_USUARIOS(usuario);

// Obtener datos del usuario
var usuarioN = from a in db2.Persona
               where a.IdPersona == usuario
               select a;
modelDB.Persona = usuarioN.ToList();

return View(modelDB);
```

**Datos mostrados**:
- Nombre del usuario
- Módulos disponibles según permisos
- Información personal del usuario

##### 2.2 CerrarSesion
```csharp
public ActionResult CerrarSesion()
```
**Propósito**: Cierra la sesión del usuario.

**Implementación**:
```csharp
sesion.destroySession();  // Session.Abandon()
return RedirectToAction("Login_", "Login");
```

---

### 3. WebPlanAnualController

**Responsabilidad**: Gestión completa del Plan Anual de Auditoría.

**Ubicación**: `Controllers/WebPlanAnualController.cs`

**Requiere autorización**: `[Logueado]`

#### Propiedades y Configuración

```csharp
PROYECTOSIAV2Entities1 db2 = new PROYECTOSIAV2Entities1();
ContenedorModelos modelDB = new ContenedorModelos();

// Listas estáticas para dropdowns
private readonly List<string> listaSN = new List<string> { "SI", "NO" };
private readonly List<string> listaTipoEvaluacion = new List<string> 
    { "COLABORATIVO", "MESA", "TRADICIONAL" };
private readonly List<string> listaUltimoCalificativo = new List<string> 
    { "Sin Calificativo", "Aceptable", "Regular", "Satisfactorio" };
private readonly List<string> listaFaseSOX = new List<string> 
    { "No Aplica", "I", "II", "III" };
private readonly List<string> listaEquipoResponsable = new List<string> 
    { "Auditoría de Procesos", "Auditoría de Procesos de Seguros", 
      "Auditoría de Prestación de Salud", "Auditoría de Procesos de Tecnología", 
      "Calidad" };
```

#### Métodos Principales

##### 3.1 Inicio
```csharp
public ActionResult Inicio()
```
**Propósito**: Página de inicio del módulo de Plan Anual.

**Lógica**:
```csharp
ViewBag.nombre = Convert.ToString(Session["usuario"]);
var idUsuario = Convert.ToInt32(Session["IdUser"]);

// Obtener rol del usuario en este módulo
Session["idRol"] = (from u in db2.UsuarioModuloRol 
                    where u.IdModulo == 12 && u.IdUsuario == idUsuario 
                    select u.IdRol).First();
ViewBag.idRol = Session["idRol"];

return View();
```

##### 3.2 ElaboracionPlanAnual
```csharp
public ActionResult ElaboracionPlanAnual()
```
**Propósito**: Vista para crear y elaborar el plan anual.

**Características**:
- Permite creación de nuevo plan
- Sistema de votación para priorización
- Scoring de riesgos
- Asignación de evaluaciones

##### 3.3 ElaboracionPlanDinamico
```csharp
public ActionResult ElaboracionPlanDinamico()
```
**Propósito**: Ajustes dinámicos al plan durante el año.

**Uso**: Modificaciones y actualizaciones del plan ya aprobado.

##### 3.4 EjecucionPlanAnual
```csharp
public ActionResult EjecucionPlanAnual(int? pa = null)
```
**Propósito**: Seguimiento de la ejecución del plan anual.

**Lógica compleja**:
```csharp
// Obtener todos los planes
var planAnual = (from p in db2.DPA_Plan_Anual.Where(x => x.ID_Plan_Anual >= 4) 
                 select p)
                 .OrderByDescending(p => p.ID_Plan_Anual);
modelDB.DPA_Plan_Anual = planAnual.ToList();

// Plan por defecto si no se especifica
pa = pa ?? 14;

// Obtener proyectos del plan (función de tabla)
modelDB.fn_TG_Obtener_Proyectos_v2 = db2.fn_TG_Obtener_Proyectos_v2(pa);

// Plan seleccionado
DPA_Plan_Anual planAnualSeleccionado = pa != null 
    ? db2.DPA_Plan_Anual.Find(pa) 
    : planAnual.First();

ViewBag.PlanAnualEjecucion = planAnualSeleccionado;

return View(modelDB);
```

##### 3.5 GestionEvaluaciones
```csharp
public ActionResult GestionEvaluaciones()
```
**Propósito**: CRUD de evaluaciones dentro del plan.

**Operaciones**:
- Crear evaluaciones
- Editar evaluaciones
- Eliminar evaluaciones
- Asignar auditores
- Gestionar scoring

##### 3.6 CargaPlanAnual
```csharp
public ActionResult CargaPlanAnual()
```
**Propósito**: Carga masiva de datos desde Excel.

**Proceso**:
```
1. Upload de archivo Excel
   ↓
2. Validación de estructura (LinqToExcel)
   ↓
3. Validación de datos de negocio (SP)
   ↓
4. Transacción de inserción masiva
   ↓
5. Reporte de errores/éxitos
```

##### 3.7 CalendarioTrabajo
```csharp
public ActionResult CalendarioTrabajo()
```
**Propósito**: Gestión del calendario de trabajo y disponibilidad de auditores.

**Funcionalidades**:
- Días no laborables
- Disponibilidad de auditores
- Asignación de fechas
- Visualización tipo scheduler

##### 3.8 ProgramacionFinal
```csharp
public ActionResult ProgramacionFinal()
```
**Propósito**: Cierre y programación definitiva del plan anual.

**Stored Procedure usado**:
```csharp
var programacion = db2.SP_DPA_Programacion_Final(idPlan).ToList();
```

##### 3.9 VotacionAuditor
```csharp
public ActionResult VotacionAuditor()
```
**Propósito**: Sistema de votación para priorización de evaluaciones.

**Funcionalidades**:
- Creación de votaciones
- Registro de votos por auditor
- Cálculo de puntajes
- Priorización automática

##### 3.10 Universo_Plan_Anual
```csharp
public ActionResult Universo_Plan_Anual()
```
**Propósito**: Gestión del universo auditable.

**Operaciones CRUD**:
```csharp
// Obtener universo
var universo = db2.DD_Universo.Where(u => u.Activo == true).ToList();
modelDB.DD_Universo = universo;

// Cambios en el universo
var cambios = db2.DD_Cambios_Universo.ToList();
modelDB.DD_Cambios_Universo = cambios;
```

##### 3.11 MantenimientoPlan
```csharp
public ActionResult MantenimientoPlan(int idPlan)
```
**Propósito**: Edición y mantenimiento de planes existentes.

**SP Principal**:
```csharp
modelDB.SP_DPA_Mantenimiento_Plan_Result = 
    db2.SP_DPA_Mantenimiento_Plan(idPlan).ToList();
```

#### Vistas Parciales (PV_)

El controlador maneja múltiples vistas parciales para modals:

| Vista Parcial | Propósito |
|---------------|-----------|
| `PV_CrearPlan` | Modal para crear plan anual |
| `PV_CrearEvaluacion` | Modal para crear evaluación |
| `PV_CrearEvaluacionPlan` | Modal para evaluación de plan |
| `PV_EditarEvaluacion` | Edición de evaluación |
| `PV_EditarEvaluacionPlan` | Edición de evaluación de plan |
| `PV_CrearProyecto` | Creación de proyecto |
| `PV_DetalleProyecto` | Detalle completo de proyecto |
| `PV_DetalleActividad` | Detalle de actividad |
| `PV_CrearVotacion` | Sistema de votación |
| `PV_EditarVotacion` | Edición de votación |
| `PV_VerVotacionEvaluacionPlan` | Resultados de votación |
| `PV_CrearAuditorEvaluacion` | Asignar auditor |
| `PV_CrearAuditorTiempoExtra` | Tiempo extra de auditor |
| `PV_EditarAuditorTiempoExtra` | Editar tiempo extra |
| `PV_EditarScoring` | Edición de scoring |
| `PV_LogEvaluacion` | Historial de evaluación |
| `PV_LogEvaluacionPlan` | Historial de plan |
| `PV_DividirEvaluacionPlan` | Dividir evaluación |
| `PV_FusionarEvaluacionPlan` | Fusionar evaluaciones |
| `PV_ReemplazarEvaluacionPlan` | Reemplazar evaluación |
| `PV_AgregarEvalPlan` | Agregar al plan |

**Patrón de uso**:
```csharp
public ActionResult PV_CrearEvaluacion(int idPlan)
{
    // Preparar datos para el modal
    ViewBag.IdPlan = idPlan;
    ViewBag.TiposEvaluacion = listaTipoEvaluacion;
    
    var negocios = db2.DPA_Negocio.ToList();
    ViewBag.Negocios = new SelectList(negocios, "ID_Negocio", "Nombre");
    
    return PartialView();
}

[HttpPost]
public ActionResult PV_CrearEvaluacion(DPA_Evaluacion model)
{
    if (ModelState.IsValid)
    {
        db2.DPA_Evaluacion.Add(model);
        db2.SaveChanges();
        return Json(new { success = true });
    }
    return Json(new { success = false, errors = ModelState });
}
```

---

### 4. WebSeguimientoObservacionesController

**Responsabilidad**: Seguimiento y gestión de observaciones de auditoría.

**Ubicación**: `Controllers/WebSeguimientoObservacionesController.cs`

**Requiere autorización**: `[Logueado]`

#### Métodos Principales

##### 4.1 Inicio
```csharp
public ActionResult Inicio(string fechaCorte = null, 
                           int? prima_check = null, 
                           int? seguros_check = null,
                           int? o_check = null, 
                           int? o_m_check = null,
                           int? en_f_check = null, 
                           int? v_check = null, 
                           int? p_check = null,
                           int? c_check = null, 
                           int? i_check = null, 
                           string filtroAnio = null, 
                           string id = null)
```
**Propósito**: Vista principal de seguimiento con filtros avanzados.

**Parámetros de filtrado**:
- `prima_check`: Filtro Prima AFP
- `seguros_check`: Filtro Seguros
- `o_check`: Observaciones abiertas
- `o_m_check`: Observaciones con modificaciones
- `en_f_check`: En fecha
- `v_check`: Vencidas
- `p_check`: Próximas a vencer
- `c_check`: Cerradas
- `i_check`: Implementadas
- `filtroAnio`: Año de observaciones
- `fechaCorte`: Fecha de corte para el reporte

**Lógica de fechas**:
```csharp
if (string.IsNullOrEmpty(fechaCorte))
{
    DateTime fechaAct = DateTime.Now;
    fechaCorte = fechaAct.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
}
else
{
    DateTime fechaParametro = DateTime.ParseExact(fechaCorte, "dd/MM/yyyy", 
                                                   CultureInfo.InvariantCulture);
    fechaParametro = fechaParametro.AddDays(1);
    fechaCorte = fechaParametro.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
}
```

**Llamada a función de tabla**:
```csharp
modelDB.fn_Stock_Observaciones_Integrado_v7 = 
    db.fn_Stock_Observaciones_Integrado_v7(fechaCorte, prima_check, seguros_check, 
                                           o_check, o_m_check, en_f_check, 
                                           v_check, p_check, c_check, i_check, 
                                           filtroAnio, id);
```

##### 4.2 ObtenerSubgerencias
```csharp
public ActionResult ObtenerSubgerencias(int idGerenciaAnterior)
```
**Propósito**: Obtener subgerencias basadas en la gerencia padre (AJAX).

**Retorno JSON**:
```csharp
List<TG_Unidad_Responsable> unidadesResponsables = 
    db.TG_Unidad_Responsable
      .Where(ur => (ur.ID_Padre == idGerenciaAnterior || 
                    ur.ID == idGerenciaAnterior) && 
                    ur.flagActivo == 1)
      .ToList();

var subgerencias = unidadesResponsables
    .Select(u => new { Value = u.ID, Text = u.Nombre })
    .ToList();

return Json(subgerencias, JsonRequestBehavior.AllowGet);
```

##### 4.3 DescargarReporteObservaciones
```csharp
public void DescargarReporteObservaciones(int? prima = null, 
                                          int? seguros = null, 
                                          string fechaCorte = null,
                                          int? o_check = null, 
                                          int? o_m_check = null,
                                          int? en_f_check = null, 
                                          int? v_check = null, 
                                          int? p_check = null,
                                          int? c_check = null, 
                                          int? i_check = null, 
                                          string filtroAnio = null, 
                                          string id = null)
```
**Propósito**: Genera y descarga reporte de observaciones en Excel.

**Proceso con EPPlus**:
```csharp
// Obtener datos
List<fn_Stock_Observaciones_Integrado_v7_Result> reporteObs = 
    db.fn_Stock_Observaciones_Integrado_v7(fechaCorte, prima, seguros, 
                                           o_check, o_m_check, en_f_check, 
                                           v_check, p_check, c_check, i_check, 
                                           filtroAnio, id).ToList();

// Crear Excel
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
ExcelPackage pck = new ExcelPackage();
ExcelWorksheet ws = pck.Workbook.Worksheets.Add("ReporteObservaciones");

// Encabezados
ws.Cells["A1"].Value = "Fuente";
ws.Cells["B1"].Value = "ID";
ws.Cells["C1"].Value = "Proyecto";
// ... más columnas

// Estilos de encabezado
using (ExcelRange rng = ws.Cells["A1:AD1"])
{
    rng.Style.Font.Bold = true;
    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
    rng.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
    rng.Style.Font.Color.SetColor(System.Drawing.Color.Black);
}

// Llenar datos
int row = 2;
foreach (var obs in reporteObs)
{
    ws.Cells[$"A{row}"].Value = obs.Fuente;
    ws.Cells[$"B{row}"].Value = obs.ID;
    ws.Cells[$"C{row}"].Value = obs.Proyecto;
    // ... más campos
    row++;
}

// Auto-ajustar columnas
ws.Cells[ws.Dimension.Address].AutoFitColumns();

// Descargar
Response.Clear();
Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
Response.AddHeader("content-disposition", 
                   $"attachment; filename=ReporteObservaciones_{DateTime.Now:yyyyMMdd}.xlsx");
Response.BinaryWrite(pck.GetAsByteArray());
Response.End();
```

##### 4.4 DetalleIndividual
```csharp
public ActionResult DetalleIndividual(int id)
```
**Propósito**: Muestra detalle completo de una observación.

**Datos mostrados**:
- Información de la observación
- Plan de acción
- Respuestas del responsable
- Implementaciones
- Historial de cambios
- Documentos adjuntos

##### 4.5 EstructuraSeguimiento
```csharp
public ActionResult EstructuraSeguimiento()
```
**Propósito**: Gestión de la estructura organizacional para seguimiento.

**Funcionalidades**:
- CRUD de unidades responsables
- Jerarquía organizacional
- Contactos por área
- Responsables de observaciones

---

### 5. AdministradorUsuariosController

**Responsabilidad**: Administración de usuarios, roles y permisos.

**Ubicación**: `Controllers/AdministradorUsuariosController.cs`

**Requiere autorización**: `[Logueado]`

#### Contextos Utilizados

```csharp
PROYECTOSIAV2Entities1 db = new PROYECTOSIAV2Entities1();  // Context principal
TeamMateR12Entities db2 = new TeamMateR12Entities();        // Context TeamMate
```

#### Métodos Principales

##### 5.1 Administrador_U (GET)
```csharp
public ActionResult Administrador_U(string mensaje_editar, 
                                    string mensaje_eliminar, 
                                    string mensaje_crear)
```
**Propósito**: Lista de asignaciones usuario-módulo-rol.

**Lógica**:
```csharp
// Validar permisos del usuario actual
var idUsuarioValidacion = Convert.ToInt32(Session["IdUser"]);
ViewBag.idRolValidacion = (from u in db.UsuarioModuloRol 
                           where u.IdModulo == 9 && u.IdUsuario == idUsuarioValidacion 
                           select u.IdRol).First();

// Preparar dropdown de usuarios activos
var PersonaVista = from c in db.Persona 
                   where c.Activo == 1 
                   select new { 
                       c.IdPersona, 
                       Nombre_Ape = c.Nombres + " " + c.Apellidos 
                   };
ViewBag.IdUsuario = new SelectList(PersonaVista.ToList(), "IdPersona", "Nombre_Ape");

// Obtener asignaciones
var usuarioModuloRol = db.UsuarioModuloRol
    .Include(u => u.ModuloRol)
    .Include(u => u.Usuario)
    .OrderBy(u => u.Usuario.Persona.Nombres);

return View(usuarioModuloRol.ToList());
```

##### 5.2 Create (GET)
```csharp
public ActionResult Create(string mensaje_crear_falla)
```
**Propósito**: Formulario para asignar módulo y rol a usuario.

**Preparación de datos**:
```csharp
Session["IdModulo"] = 1;
int IdModulo = Convert.ToInt32(Session["IdModulo"]);

// Usuarios activos
var PersonaVista = from c in db.Persona 
                   where c.Activo == 1 
                   select new { 
                       c.IdPersona, 
                       Nombre_Ape = c.Nombres + " " + c.Apellidos 
                   };
ViewBag.IdUsuario = new SelectList(PersonaVista.ToList(), "IdPersona", "Nombre_Ape");

// Módulos
ViewBag.IdModulo = new SelectList(db.Modulo, "IdModulo", "Nombre");

// Roles por módulo (SP)
ViewBag.IdRol = new SelectList(db.SP_WT_ROLES_MODULO(IdModulo), "IdRol", "Tiporol");
```

##### 5.3 Create (POST)
```csharp
[HttpPost]
[ValidateAntiForgeryToken]
public ActionResult Create([Bind(Include = "IdUsuario,IdModulo,IdRol,ID")] 
                           UsuarioModuloRol usuarioModuloRol)
```
**Propósito**: Procesa la asignación.

**Validación de duplicados**:
```csharp
var query = from u in db.UsuarioModuloRol
            where u.IdUsuario == usuarioModuloRol.IdUsuario && 
                  u.IdModulo == usuarioModuloRol.IdModulo && 
                  u.IdRol == usuarioModuloRol.IdRol
            select u;

if (query.Count() > 0)
{
    return RedirectToAction("Create", "AdministradorUsuarios", 
        new { @mensaje_crear_falla = "Ya fue creado el registro con este rol." });
}

usuarioModuloRol.Estado = 1;
db.UsuarioModuloRol.Add(usuarioModuloRol);
db.SaveChanges();

return RedirectToAction("Administrador_U", "AdministradorUsuarios", 
    new { @mensaje_crear = "El Registro se creó con éxito." });
```

##### 5.4 Edit (GET)
```csharp
public ActionResult Edit(int? id, int? idusuario, int? idmodulo, 
                         int? idrol, int? Estadoid, string mensaje_editar_falla)
```
**Propósito**: Formulario de edición de asignación.

**Carga de datos**:
```csharp
UsuarioModuloRol usuarioModuloRol = db.UsuarioModuloRol.Find(id);
ViewBag.userN = usuarioModuloRol.Usuario.Persona.Nombres + " " + 
                usuarioModuloRol.Usuario.Persona.Apellidos;

// Roles disponibles para el módulo
ViewBag.IdRol = new SelectList(db.SP_WT_ROLES_MODULO(idmodulo), "IdRol", "Tiporol");
```

##### 5.5 Edit (POST)
```csharp
[HttpPost]
[ValidateAntiForgeryToken]
public ActionResult Edit([Bind(Include = "IdUsuario,IdModulo,IdRol,ID")] 
                         UsuarioModuloRol usuarioModuloRol)
```
**Propósito**: Actualiza la asignación.

**Validación de duplicados antes de actualizar**:
```csharp
var query = from u in db.UsuarioModuloRol
            where u.IdUsuario == usuarioModuloRol.IdUsuario && 
                  u.IdModulo == usuarioModuloRol.IdModulo && 
                  u.IdRol == usuarioModuloRol.IdRol && 
                  u.ID != usuarioModuloRol.ID
            select u;

if (query.Count() > 0)
{
    // Ya existe
    return RedirectToAction("Edit", ...);
}

db.Entry(usuarioModuloRol).State = EntityState.Modified;
db.SaveChanges();
```

##### 5.6 Delete (GET y POST)
```csharp
public ActionResult Delete(int? id)
[HttpPost, ActionName("Delete")]
[ValidateAntiForgeryToken]
public ActionResult DeleteConfirmed(int id)
```
**Propósito**: Eliminación de asignación usuario-módulo-rol.

##### 5.7 CrearUsuario
```csharp
public ActionResult CrearUsuario()
[HttpPost]
public ActionResult CrearUsuario(Persona persona, Usuario usuario)
```
**Propósito**: Creación completa de un nuevo usuario.

**Proceso**:
```
1. Crear registro en Persona
   ↓
2. Crear registro en Usuario (con IdPersona)
   ↓
3. Encriptar contraseña
   ↓
4. SaveChanges en transacción
```

##### 5.8 Administrador_Rotacion
```csharp
public ActionResult Administrador_Rotacion()
```
**Propósito**: Gestión de rotación de personal.

**Funcionalidades**:
- Ver rotaciones activas
- Crear nueva rotación
- Editar rotación
- Finalizar rotación

##### 5.9 Administrador_Modulos
```csharp
public ActionResult Administrador_Modulos()
```
**Propósito**: Gestión de módulos del sistema.

##### 5.10 EstructuraSeguimiento
```csharp
public ActionResult EstructuraSeguimiento()
```
**Propósito**: Gestión de estructura organizacional.

**Operaciones CRUD**:
- Unidades responsables
- Contactos
- Jerarquía organizacional

---

## Flujos de Trabajo

### Flujo de Autenticación

```
Usuario → Login (GET)
           ↓
       Formulario Login
           ↓
   Usuario ingresa credenciales
           ↓
       Login (POST)
           ↓
    Validar credenciales
           ↓
    ¿Válidas? → NO → Mostrar error
           ↓ SI
    Crear sesión
           ↓
    Registrar log
           ↓
    Redireccionar a Home/Index
           ↓
    Mostrar módulos disponibles
```

### Flujo de Creación de Plan Anual

```
ElaboracionPlanAnual (GET)
           ↓
    Vista principal
           ↓
    Usuario clic "Crear Plan"
           ↓
    PV_CrearPlan (Modal)
           ↓
    Usuario llena formulario
           ↓
    PV_CrearPlan (POST)
           ↓
    Validar datos
           ↓
    Insertar en DPA_Plan_Anual
           ↓
    Retornar JSON success
           ↓
    Cerrar modal y refrescar grid
```

### Flujo de Carga Masiva Excel

```
Usuario selecciona archivo Excel
           ↓
    Upload a servidor
           ↓
    LinqToExcel lee archivo
           ↓
    Validar estructura
           ↓
    ¿Estructura correcta? → NO → Retornar errores
           ↓ SI
    Ejecutar SP de validación de datos
           ↓
    ¿Datos válidos? → NO → Retornar errores de negocio
           ↓ SI
    Iniciar transacción
           ↓
    Insertar registros en lote
           ↓
    ¿Éxito? → NO → Rollback
           ↓ SI
    Commit transacción
           ↓
    Retornar resumen de carga
```

### Flujo de Seguimiento de Observaciones

```
Inicio (GET) con filtros
           ↓
    Aplicar valores default a filtros
           ↓
    Ejecutar fn_Stock_Observaciones_Integrado_v7
           ↓
    Cargar grid de observaciones
           ↓
    Usuario clic en observación
           ↓
    DetalleIndividual (GET)
           ↓
    Mostrar detalle completo
           ↓
    Usuario actualiza respuesta
           ↓
    ActualizarRespuesta (POST)
           ↓
    Guardar en BD
           ↓
    Refrescar detalle
```

---

## Patrones y Convenciones

### 1. Patrón de Acción GET/POST

**GET**: Muestra el formulario
```csharp
[HttpGet]
public ActionResult Create()
{
    // Preparar datos para dropdowns
    ViewBag.Categorias = new SelectList(db.Categorias, "Id", "Nombre");
    return View();
}
```

**POST**: Procesa el formulario
```csharp
[HttpPost]
[ValidateAntiForgeryToken]
public ActionResult Create(Modelo modelo)
{
    if (ModelState.IsValid)
    {
        db.Modelos.Add(modelo);
        db.SaveChanges();
        return RedirectToAction("Index");
    }
    return View(modelo);
}
```

### 2. Manejo de Sesiones

**Establecer sesión**:
```csharp
session.setSession("nombre", valor);
// o directamente
Session["nombre"] = valor;
```

**Obtener sesión**:
```csharp
var valor = Convert.ToString(Session["nombre"]);
// o con SesionData
var valor = session.getSession("nombre");
```

**Validar sesión**:
```csharp
if (Session["IdUser"] == null)
{
    return RedirectToAction("Login_", "Login");
}
```

### 3. Preparación de ViewBag

**Para dropdowns (SelectList)**:
```csharp
ViewBag.Categorias = new SelectList(db.Categorias, "Id", "Nombre");
// En la vista: @Html.DropDownList("CategoriaId", (SelectList)ViewBag.Categorias)
```

**Para datos simples**:
```csharp
ViewBag.Titulo = "Crear Usuario";
ViewBag.MensajeExito = "Operación exitosa";
```

**Para datos del usuario**:
```csharp
ViewBag.nombre = Convert.ToString(Session["usuario"]);
ViewBag.idRol = Session["idRol"];
```

### 4. Uso de ContenedorModelos

```csharp
ContenedorModelos modelDB = new ContenedorModelos();
modelDB.Persona = db.Persona.ToList();
modelDB.DPA_Plan_Anual = db.DPA_Plan_Anual.Where(p => p.Activo).ToList();
modelDB.SP_RESULTADO = db.SP_RESULTADO(parametro).ToList();
return View(modelDB);
```

### 5. Respuestas JSON para AJAX

**Éxito**:
```csharp
return Json(new { success = true, data = resultado }, JsonRequestBehavior.AllowGet);
```

**Error**:
```csharp
return Json(new { success = false, message = "Error al procesar" }, JsonRequestBehavior.AllowGet);
```

**Con errores de validación**:
```csharp
return Json(new { 
    success = false, 
    errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) 
});
```

### 6. Redirecciones con Mensajes

```csharp
return RedirectToAction("Index", "Controller", 
    new { @mensaje = "Operación exitosa", @id = entidad.Id });
```

### 7. Manejo de Archivos Excel

**Lectura con LinqToExcel**:
```csharp
var excelFile = new ExcelQueryFactory(rutaArchivo);
var datos = from row in excelFile.Worksheet<ModeloExcel>("Hoja1")
            select row;
```

**Escritura con EPPlus**:
```csharp
using (ExcelPackage package = new ExcelPackage())
{
    ExcelWorksheet ws = package.Workbook.Worksheets.Add("Datos");
    ws.Cells["A1"].Value = "Encabezado";
    // ... llenar datos
    return File(package.GetAsByteArray(), 
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
                "archivo.xlsx");
}
```

---

## Autorización y Seguridad

### Atributo [Logueado]

**Implementación** (en `Autorizacion/Logueado.cs`):
```csharp
public class Logueado : AuthorizeAttribute
{
    protected override bool AuthorizeCore(HttpContextBase httpContext)
    {
        if (httpContext.Session["IdUser"] != null)
        {
            return true;
        }
        return false;
    }

    protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
    {
        filterContext.Result = new RedirectToRouteResult(
            new RouteValueDictionary(new { 
                controller = "Login", 
                action = "Login_" 
            })
        );
    }
}
```

**Uso**:
```csharp
[Logueado]  // A nivel de controlador
public class MiController : Controller
{
    // Todos los métodos requieren autenticación
}

// O a nivel de acción
public class PublicController : Controller
{
    [Logueado]
    public ActionResult AccionProtegida()
    {
        // Requiere autenticación
    }
    
    [AllowAnonymous]
    public ActionResult AccionPublica()
    {
        // No requiere autenticación
    }
}
```

### Validación de Roles

```csharp
var idRol = Convert.ToInt32(Session["idRol"]);
if (idRol != 1) // 1 = Administrador
{
    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
}
```

### Token Anti-Forgery

**En el controlador**:
```csharp
[HttpPost]
[ValidateAntiForgeryToken]
public ActionResult Action(Modelo modelo)
{
    // ...
}
```

**En la vista**:
```csharp
@using (Html.BeginForm())
{
    @Html.AntiForgeryToken()
    // ... campos del formulario
}
```

---

## Manejo de Errores

### Try-Catch en Controladores

```csharp
public ActionResult Action()
{
    try
    {
        // Lógica de negocio
        return View();
    }
    catch (DbEntityValidationException ex)
    {
        // Errores de validación de EF
        var errors = ex.EntityValidationErrors
            .SelectMany(e => e.ValidationErrors)
            .Select(e => e.ErrorMessage);
        ViewBag.Errors = errors;
        return View();
    }
    catch (Exception ex)
    {
        // Error general
        ViewBag.Error = "Ocurrió un error: " + ex.Message;
        return View("Error");
    }
}
```

### Validación de ModelState

```csharp
if (!ModelState.IsValid)
{
    // Retornar vista con errores
    return View(modelo);
}
```

### Mensajes de Error Personalizados

```csharp
ModelState.AddModelError("", "El usuario ya existe");
ModelState.AddModelError("Email", "El email no es válido");
```

---

## Mejores Prácticas

### ✅ DO

1. **Validar siempre en el servidor**: Nunca confiar solo en validación del cliente
2. **Usar transacciones**: Para operaciones que involucran múltiples tablas
3. **Cerrar sesión**: Implementar timeout y cierre manual de sesión
4. **Sanitizar inputs**: Especialmente en búsquedas y filtros
5. **Usar ViewModels**: Para operaciones complejas o vistas específicas
6. **Logging**: Registrar acciones críticas (login, cambios importantes)
7. **Manejo de errores**: Try-catch en operaciones de BD y archivos

### ❌ DON'T

1. **No almacenar contraseñas en texto plano**: Siempre encriptar
2. **No exponer IDs internos**: En URLs si no es necesario
3. **No confiar en datos del cliente**: Siempre revalidar en servidor
4. **No usar Session excesivamente**: Solo para datos esenciales
5. **No hacer queries en loops**: Usar Include o joins
6. **No retornar excepciones al cliente**: Mensajes genéricos al usuario
7. **No olvidar Dispose**: Entity Framework context

---

## Referencia Rápida

### Instancias Comunes
```csharp
PROYECTOSIAV2Entities1 db2 = new PROYECTOSIAV2Entities1();
ContenedorModelos modelDB = new ContenedorModelos();
SesionData session = new SesionData();
```

### Obtener Usuario Actual
```csharp
var idUsuario = Convert.ToInt32(Session["IdUser"]);
var nombreUsuario = Convert.ToString(Session["usuario"]);
var idRol = Convert.ToInt32(Session["idRol"]);
```

### Redirección Estándar
```csharp
return RedirectToAction("Action", "Controller", new { id = value });
```

### Retorno JSON
```csharp
return Json(data, JsonRequestBehavior.AllowGet);
```

### SelectList
```csharp
ViewBag.Lista = new SelectList(coleccion, "ValorField", "TextoField", valorSeleccionado);
```

---

**Nota**: Esta documentación cubre los controladores principales. Para funcionalidades específicas de controladores adicionales, consultar el código fuente directamente en la carpeta `Controllers/`.
