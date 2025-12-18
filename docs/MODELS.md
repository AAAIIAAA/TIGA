# MODELS.md - Documentación de Modelos de Datos

## 📋 Índice
1. [Introducción](#introducción)
2. [Arquitectura de Datos](#arquitectura-de-datos)
3. [Entity Framework Context](#entity-framework-context)
4. [Contenedor de Modelos](#contenedor-de-modelos)
5. [Categorías de Modelos](#categorías-de-modelos)
6. [Modelos por Módulo](#modelos-por-módulo)
7. [Stored Procedures](#stored-procedures)
8. [Modelos de Vista](#modelos-de-vista)
9. [Validaciones y Anotaciones](#validaciones-y-anotaciones)

---

## Introducción

La capa de modelos en TIGA implementa el patrón **Database First** de Entity Framework 6.1.3. Los modelos son generados automáticamente desde la base de datos SQL Server y representan tanto las entidades de tablas como los resultados de stored procedures y funciones de tabla.

## Arquitectura de Datos

### Estrategia Database First

```
Base de Datos SQL Server
        ↓
Entity Framework 6.1.3
        ↓
Modelos Generados Automáticamente (.edmx)
        ↓
Clases Parciales en C#
        ↓
Controladores y Lógica de Negocio
```

### Context de Entity Framework

**Clase Principal**: `PROYECTOSIAV2Entities1`

```csharp
namespace WebTIGA.Models
{
    public partial class PROYECTOSIAV2Entities1 : DbContext
    {
        public PROYECTOSIAV2Entities1() : base("name=PROYECTOSIAV2Entities1")
        {
        }
        
        // DbSets para entidades
        public virtual DbSet<Persona> Persona { get; set; }
        public virtual DbSet<DPA_Plan_Anual> DPA_Plan_Anual { get; set; }
        public virtual DbSet<DPA_Evaluacion> DPA_Evaluacion { get; set; }
        // ... más DbSets
        
        // Stored Procedures
        public virtual ObjectResult<SP_DPA_Mantenimiento_Plan_Result> SP_DPA_Mantenimiento_Plan(int? idPlan)
        {
            // Implementación generada
        }
    }
}
```

**Uso en Controladores:**
```csharp
PROYECTOSIAV2Entities1 db2 = new PROYECTOSIAV2Entities1();
```

---

## Contenedor de Modelos

### ContenedorModelos.cs

Clase diseñada para agrupar múltiples colecciones de datos y pasarlas a las vistas. Este patrón permite composiciones complejas de datos sin necesidad de crear ViewModels específicos para cada vista.

```csharp
public class ContenedorModelos
{
    // Plan Anual de Auditoría
    public IEnumerable<DPA_Negocio> DPA_Negocio { get; set; }
    public IEnumerable<DPA_Plan_Anual> DPA_Plan_Anual { get; set; }
    public IEnumerable<DPA_Evaluacion> DPA_Evaluacion { get; set; }
    public IEnumerable<DPA_Evaluacion_Plan> DPA_Evaluacion_Plan { get; set; }
    public IEnumerable<DPA_Log_Evaluacion_Plan> DPA_Log_Evaluacion_Plan { get; set; }
    public IEnumerable<DPA_Votacion> DPA_Votacion { get; set; }
    
    // Balance de Cuentas
    public IEnumerable<SP_BC_BG_INFORME_Result> SP_BC_BG_INFORME { get; set; }
    public IEnumerable<SP_BC_GYP_INFORME_Result> SP_BC_GYP_INFORME { get; set; }
    public IEnumerable<SP_BC_PROYECTO_PLAN_Result> SP_BC_PROYECTO_PLAN { get; set; }
    
    // Proyectos
    public IEnumerable<DD_Proyecto> DD_Proyecto { get; set; }
    public IEnumerable<fn_TG_Obtener_Proyectos_v2_Result> fn_TG_Obtener_Proyectos_v2 { get; set; }
    
    // Seguimiento de Observaciones
    public IEnumerable<SP_WT_SEGUIMIENTO_OBSERVACIONES_Result> SP_WT_SEGUIMIENTO_OBSERVACIONES { get; set; }
    
    // Reportería
    public IEnumerable<SP_RA_REPORTE_GENERAL_Result> SP_RA_REPORTE_GENERAL { get; set; }
    public IEnumerable<SP_RA_REPORTE_STATUS_ACTIVIDADES_Result> SP_RA_REPORTE_STATUS_ACTIVIDADES { get; set; }
    
    // Usuarios y Seguridad
    public IEnumerable<Persona> Persona { get; set; }
    public IEnumerable<SP_MODULOS_USUARIOS_Result> SP_MODULOS_USUARIOS_Result { get; set; }
}
```

**Ventajas:**
- ✅ Flexibilidad para pasar múltiples colecciones a una vista
- ✅ Reducción de ViewModels específicos
- ✅ Facilita la reutilización de código
- ✅ Simplifica queries complejas

**Uso Típico:**
```csharp
// En el Controlador
ContenedorModelos modelDB = new ContenedorModelos();
modelDB.DPA_Plan_Anual = db2.DPA_Plan_Anual.ToList();
modelDB.DPA_Evaluacion = db2.DPA_Evaluacion.Where(e => e.ID_Plan_Anual == id).ToList();
return View(modelDB);

// En la Vista
@model WebTIGA.Models.ContenedorModelos

@foreach (var plan in Model.DPA_Plan_Anual)
{
    // Renderizar datos
}
```

---

## Categorías de Modelos

### 1. **Modelos de Entidades (Tablas)**

Clases que representan directamente las tablas de la base de datos.

#### 1.1 Prefijo DPA_ (Datos Plan Anual)
Modelos relacionados con la planificación anual de auditoría.

| Modelo | Descripción | Propiedades Clave |
|--------|-------------|-------------------|
| `DPA_Plan_Anual` | Plan anual de auditoría | ID_Plan_Anual, Anio, Estado, Fecha_Creacion |
| `DPA_Evaluacion` | Evaluaciones dentro del plan | ID_Evaluacion, Nombre, ID_Plan_Anual, Tipo_Evaluacion |
| `DPA_Evaluacion_Plan` | Detalle de evaluaciones planificadas | ID_Evaluacion_Plan, ID_Evaluacion, Fecha_Inicio, Fecha_Fin |
| `DPA_Negocio` | Unidades de negocio | ID_Negocio, Nombre, Descripcion |
| `DPA_Auditor_Evaluacion` | Auditores asignados a evaluaciones | ID_Auditor, ID_Evaluacion_Plan, Horas_Asignadas |
| `DPA_Auditor_Tiempo_Extra` | Tiempos extras de auditores | ID_Tiempo_Extra, ID_Auditor, Horas, Motivo |
| `DPA_Equipo` | Equipos de auditoría | ID_Equipo, Nombre, Responsable |
| `DPA_Votacion` | Sistema de votación | ID_Votacion, ID_Evaluacion, Fecha_Votacion |
| `DPA_Opcion_Votacion` | Opciones de votación | ID_Opcion, Descripcion, Puntaje |
| `DPA_Criterio_Scoring` | Criterios de evaluación | ID_Criterio, Nombre, Peso |
| `DPA_Criterio_Votacion` | Criterios para votación | ID_Criterio_Votacion, Descripcion |
| `DPA_Scoring_Evaluacion` | Puntajes de evaluación | ID_Scoring, ID_Evaluacion, Puntaje_Total |
| `DPA_Riesgo_Asociado` | Riesgos asociados | ID_Riesgo, Descripcion, Nivel |
| `DPA_Log_Evaluacion` | Historial de cambios en evaluaciones | ID_Log, ID_Evaluacion, Accion, Fecha, Usuario |
| `DPA_Log_Evaluacion_Plan` | Historial de cambios en plan | ID_Log, Accion, Fecha, Usuario |
| `DPA_Tipo_Tiempo_Extra` | Tipos de tiempo extra | ID_Tipo, Descripcion |

**Ejemplo de uso:**
```csharp
// Crear un nuevo plan anual
var nuevoPlan = new DPA_Plan_Anual
{
    Anio = 2025,
    Estado = "En Elaboración",
    Fecha_Creacion = DateTime.Now,
    Usuario_Creacion = "ADMIN"
};
db2.DPA_Plan_Anual.Add(nuevoPlan);
db2.SaveChanges();
```

#### 1.2 Prefijo DD_ (Diccionario de Datos)
Tablas maestras y catálogos.

| Modelo | Descripción | Propiedades Clave |
|--------|-------------|-------------------|
| `DD_Proyecto` | Proyectos de auditoría | ID_Proyecto, Nombre, Descripcion, Estado |
| `DD_Universo` | Universo auditable | ID_Universo, Entidad, Descripcion, Riesgo |
| `DD_Cambios_Universo` | Historial de cambios del universo | ID_Cambio, ID_Universo, Fecha, Cambio |
| `DD_ProcesoEvaluado` | Procesos evaluados | ID_Proceso, Nombre, Descripcion |
| `DD_Informe` | Informes de auditoría | ID_Informe, Titulo, Fecha_Emision |
| `DD_PlanAnual` | Relación con planes anuales | ID_Plan, Año, Estado |

#### 1.3 Modelos de Usuarios y Seguridad

| Modelo | Descripción | Propiedades Clave |
|--------|-------------|-------------------|
| `Persona` | Usuarios del sistema | IdPersona, Nombre, Email, Usuario, Password |
| `Rol` | Roles del sistema | IdRol, Nombre, Descripcion |
| `Modulo` | Módulos de la aplicación | IdModulo, Nombre, Icono, Url |
| `ModuloRol` | Permisos por rol | IdModuloRol, IdModulo, IdRol |
| `UsuarioModuloRol` | Asignación de permisos | IdUsuario, IdModulo, IdRol |
| `Rotacion` | Rotación de personal | IdRotacion, IdPersona, FechaInicio, FechaFin |
| `Usuario_Log` | Log de acciones de usuario | IdLog, IdUsuario, Accion, Fecha |

**Ejemplo de validación de login:**
```csharp
var usuario = db2.Persona
    .Where(p => p.Usuario == username && p.Password == encryptedPassword)
    .FirstOrDefault();

if (usuario != null)
{
    Session["IdUser"] = usuario.IdPersona;
    Session["usuario"] = usuario.Nombre;
}
```

#### 1.4 Modelos de Seguimiento de Observaciones

| Modelo | Prefijo | Descripción |
|--------|---------|-------------|
| `TS_Observacion_Cambio` | TS_ | Cambios en observaciones |
| `TS_Rol` | TS_ | Roles en seguimiento |
| `TD_VacacionesRegistro` | TD_ | Registro de vacaciones |

#### 1.5 Modelos de TIGA General

| Modelo | Descripción |
|--------|-------------|
| `TG_Proyecto` | Proyectos generales |
| `TG_Contacto` | Contactos de áreas |
| `TG_Rol` | Roles generales |
| `TG_Aprobador` | Aprobadores de procesos |
| `TG_Riesgo_Del_Trabajo` | Riesgos del trabajo |

---

### 2. **Modelos de Stored Procedures**

Todas las clases que terminan en `_Result` representan el resultado de stored procedures.

#### 2.1 Stored Procedures de Plan Anual (SP_DPA_)

| SP | Descripción | Parámetros | Uso |
|----|-------------|-----------|-----|
| `SP_DPA_Mantenimiento_Plan` | Obtiene información para mantenimiento del plan | `@idPlan` | Edición de planes |
| `SP_DPA_Carga_Plan` | Valida y carga datos de Excel al plan | `@idPlan` | Importación masiva |
| `SP_DPA_Fechas_Auditor` | Obtiene disponibilidad de auditores | `@idAuditor, @fecha` | Asignación de recursos |
| `SP_DPA_Fechas_No_Laborables` | Lista de días no laborables | `@anio` | Calendario de trabajo |
| `SP_DPA_Log_Evaluacion` | Historial de cambios en evaluación | `@idEvaluacion` | Auditoría de cambios |
| `SP_DPA_Log_Evaluacion_Plan` | Historial de cambios en plan | `@idEvaluacionPlan` | Trazabilidad |
| `SP_DPA_Log_Plan` | Historial general del plan | `@idPlan` | Monitoreo |
| `SP_DPA_Programacion_Final` | Programación final del plan | `@idPlan` | Ejecución |
| `SP_DPA_View_Asignar_Proyecto` | Vista para asignar proyectos | `@idPlan` | Asignación |
| `SP_DPA_Votacion_Auditor` | Votación de auditores | `@idVotacion` | Priorización |
| `SP_DPA_Votacion_Auditor_v2` | Votación mejorada | `@idVotacion` | Priorización v2 |
| `SP_DPA_Votacion_Evaluacion` | Votación de evaluaciones | `@idEvaluacion` | Selección |
| `SP_DPA_Votacion_Evaluacion_v2` | Votación mejorada | `@idEvaluacion` | Selección v2 |

**Ejemplo de ejecución:**
```csharp
// Obtener datos para mantenimiento de plan
var resultado = db2.SP_DPA_Mantenimiento_Plan(14).ToList();

// Asignar al contenedor
modelDB.SP_DPA_Mantenimiento_Plan_Result = resultado;
```

#### 2.2 Stored Procedures de Balance de Cuentas (SP_BC_)

| SP | Descripción | Datos Retornados |
|----|-------------|------------------|
| `SP_BC_BG_INFORME` | Balance general para informe | Activos, Pasivos, Patrimonio |
| `SP_BC_GYP_INFORME` | Ganancias y pérdidas para informe | Ingresos, Costos, Utilidades |
| `SP_BC_CUENTAS_CONTABLES` | Lista de cuentas contables | Código, Nombre, Tipo |
| `SP_BC_CUENTAS_SIGNIFICATIVAS` | Cuentas significativas | Cuenta, Monto, % Significancia |
| `SP_BC_HOJA_PERIODO` | Hojas de trabajo por período | Período, Datos |
| `SP_BC_PLAN_DE_CUENTAS` | Plan de cuentas completo | Jerarquía de cuentas |
| `SP_BC_PROYECTO_PLAN` | Proyectos relacionados con plan | Proyecto, Plan, Estado |
| `SP_BC_RESUMEN_CCSS` | Resumen de cuentas significativas | Resumen consolidado |

#### 2.3 Stored Procedures de Reportería (SP_RA_)

| SP | Descripción |
|----|-------------|
| `SP_RA_REPORTE_GENERAL` | Reporte general de auditorías |
| `SP_RA_REPORTE_STATUS_ACTIVIDADES` | Estado de actividades |
| `SP_RA_REPORTE_EFECT_CONTROL` | Efectividad de controles |
| `SP_RA_REPORTE_MENSUAL_CREDICORP` | Reporte mensual consolidado |
| `SP_RA_REPORTE_PROYECTOS_RIESGO_ALTO` | Proyectos de alto riesgo |
| `SP_RA_REPORTE_OBS_RIESGO_ALTO` | Observaciones de alto riesgo |

#### 2.4 Stored Procedures de Web Tracking (SP_WT_)

| SP | Descripción |
|----|-------------|
| `SP_WT_SEGUIMIENTO_OBSERVACIONES` | Seguimiento de observaciones |
| `SP_WT_RA_SEGUIMIENTO_OBSERVACIONES` | Seguimiento con análisis de riesgo |
| `SP_WT_RA_RESULTADO_AUDITORIA` | Resultados de auditoría |
| `SP_WT_RA_DETALLE_CONTROLES_EVALUADOS_3` | Detalle de controles evaluados |
| `SP_WT_DETALLE_RESULTADOS_AUDITORIA_1` | Detalle de resultados |
| `SP_WT_REPORTE_GENERAL1` | Reporte general web |
| `SP_WT_CONTROL_PERIODO_DIAS1` | Control de días por período |
| `SP_WT_VACACIONES_CONSOLIDADO` | Consolidado de vacaciones |

#### 2.5 Stored Procedures de Administración (SP_AU_)

| SP | Descripción |
|----|-------------|
| `SP_AU_Rotacion_Usuario` | Rotación de usuarios |
| `SP_AU_UsuariosTM` | Usuarios team member |
| `SP_AU_Drop_ContactosTM` | Eliminar contactos |

#### 2.6 Stored Procedures de Módulo DAC (SP_DAC_)

| SP | Descripción |
|----|-------------|
| `SP_DAC_SCRIPTS_POR_ESTADOS` | Scripts por estado |

#### 2.7 Stored Procedures de Gráficos (SP_GRAF_)

| SP | Descripción |
|----|-------------|
| `SP_GRAF_HORAS_TOTAL` | Totales de horas |
| `SP_GRAF_MEDIDA_DE_TIEMPO2` | Medida de tiempos |
| `SP_GRAF_TABLA_HORAS2` | Tabla de horas detallada |

#### 2.8 Stored Procedures de Time Tracking (SP_TT_)

| SP | Descripción |
|----|-------------|
| `SP_TT_DETALLE_PROYECTO_ACTIVIDAD` | Detalle de actividades por proyecto |
| `SP_TT_DETALLE_ETAPAS_FASES_ACTIVIDADES` | Etapas, fases y actividades |
| `SP_TT_EQUIPO_ETAPAS` | Equipos por etapa |
| `SP_TT_DIAS_CONTROL` | Días de control |
| `SP_TT_ERROR_FASES_DISTRIBUCION` | Errores en distribución de fases |
| `SP_TT_DISTR_DIAS_AUDITOR_POR_CONTROL_EQUIPO` | Distribución de días por auditor |

#### 2.9 Stored Procedures de Evolutivos (SP_RE_)

| SP | Descripción |
|----|-------------|
| `SP_RE_EVOLUTIVO_VENCIDAS_EQUIPO_BASE` | Vencidas por equipo base |
| `SP_RE_EVOLUTIVO_VENCIDAS2` | Evolutivo de vencidas v2 |
| `SP_RE_EVOLUTIVO_AUDIT_VENCIDAS_BASE` | Auditorías vencidas base |

#### 2.10 Otros Stored Procedures

| SP | Descripción |
|----|-------------|
| `SP_DD_Proyecto_Actividad_Informe_Obtener_v4` | Obtener actividades de proyecto |
| `SP_DD_Proyecto_Obtener_detalle_v7` | Detalle de proyecto versión 7 |
| `SP_Acciones_Observaciones` | Acciones sobre observaciones |
| `SP_CONTROLES_EVALUADOS` | Controles evaluados |
| `SP_DETALLE_CONTROLES_EVALUADOS_3` | Detalle de controles v3 |
| `SP_MODULOS_USUARIOS` | Módulos disponibles por usuario |

---

### 3. **Funciones de Tabla (fn_)**

Funciones SQL que retornan tablas. Se invocan como stored procedures.

| Función | Descripción | Retorna |
|---------|-------------|---------|
| `fn_TG_Obtener_Proyectos_v2` | Obtiene proyectos del plan anual | Lista de proyectos con detalles |
| `fn_TG_Get_Mapa_Aseguramiento_v3` | Mapa de aseguramiento | Cobertura de auditoría |
| `fn_Detalle_Acciones_Obs` | Detalle de acciones de observaciones | Acciones por observación |
| `fn_Detalle_Ampliaciones` | Detalle de ampliaciones | Ampliaciones solicitadas |
| `fn_Obtener_detalle_Observaciones` | Detalle de observaciones | Observaciones con contexto |
| `fn_Stock_Observaciones_Integrado_v7` | Stock de observaciones integrado | Stock actual de observaciones |

**Ejemplo:**
```csharp
// Obtener proyectos de un plan anual
int idPlan = 14;
var proyectos = db2.fn_TG_Obtener_Proyectos_v2(idPlan).ToList();

// Usar en la vista
modelDB.fn_TG_Obtener_Proyectos_v2 = proyectos;
return View(modelDB);
```

---

### 4. **Procedimientos Almacenados (pa_)**

| Procedimiento | Descripción |
|---------------|-------------|
| `pa_man_TG_Estructura_Contactos` | Mantenimiento de estructura de contactos |

---

## Modelos de Vista (ViewModels)

Modelos personalizados para manejo de formularios y vistas específicas.

### RecoveryViewModel
```csharp
public class RecoveryViewModel
{
    [Required(ErrorMessage = "El usuario es requerido")]
    public string Usuario { get; set; }
    
    [Required(ErrorMessage = "El email es requerido")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    public string Email { get; set; }
}
```

### RecoveryPasswordViewModel
```csharp
public class RecoveryPasswordViewModel
{
    [Required]
    public int IdUsuario { get; set; }
    
    [Required]
    [StringLength(100, MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string NuevaPassword { get; set; }
    
    [Required]
    [Compare("NuevaPassword", ErrorMessage = "Las contraseñas no coinciden")]
    [DataType(DataType.Password)]
    public string ConfirmarPassword { get; set; }
    
    public string Token { get; set; }
}
```

### UsuarioModuloRolViewModel1
```csharp
public class UsuarioModuloRolViewModel1
{
    public int IdUsuario { get; set; }
    public int IdModulo { get; set; }
    public int IdRol { get; set; }
    public string NombreModulo { get; set; }
    public string NombreRol { get; set; }
}
```

### CorreoModel
```csharp
public class CorreoModel
{
    public string Para { get; set; }
    public string Asunto { get; set; }
    public string Cuerpo { get; set; }
    public bool EsHtml { get; set; }
}
```

### SesionData
```csharp
public class SesionData
{
    private string session;
    
    public string getSession(string name)
    {
        this.session = Convert.ToString(HttpContext.Current.Session[name]);
        return session;
    }
    
    public void setSession(string name, string data)
    {
        HttpContext.Current.Session[name] = data;
    }
    
    public void destroySession()
    {
        HttpContext.Current.Session.Abandon();
    }
}
```

### Modelos para JSON

#### JsonGRAFTiempos
```csharp
public class JsonGRAFTiempos
{
    public string name { get; set; }
    public decimal y { get; set; }
}
```

#### JsonGRAFDiasxFase
```csharp
public class JsonGRAFDiasxFase
{
    public string name { get; set; }
    public int y { get; set; }
    public int drilldown { get; set; }
}
```

#### Reporte_script
```csharp
public class Reporte_script
{
    public int id_script { get; set; }
    public string nombre_script { get; set; }
    public string estado { get; set; }
}
```

---

## Validaciones y Anotaciones

### Data Annotations Comunes

```csharp
// Ejemplo de modelo con validaciones
public partial class Persona
{
    [Required(ErrorMessage = "El nombre es requerido")]
    [StringLength(100)]
    public string Nombre { get; set; }
    
    [Required]
    [EmailAddress(ErrorMessage = "Email inválido")]
    public string Email { get; set; }
    
    [Required]
    [StringLength(50, MinimumLength = 4)]
    public string Usuario { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    
    [Phone]
    public string Telefono { get; set; }
    
    [Range(1, 100)]
    public int Edad { get; set; }
}
```

### Validaciones Personalizadas

Las validaciones de negocio complejas se implementan en:
- Stored Procedures (validaciones en BD)
- Métodos de controlador (validaciones de aplicación)
- JavaScript del lado del cliente (validaciones de UX)

---

## Patrones de Uso

### 1. Consulta Simple
```csharp
var planes = db2.DPA_Plan_Anual
    .Where(p => p.Anio == 2025)
    .OrderByDescending(p => p.Fecha_Creacion)
    .ToList();
```

### 2. Consulta con Join
```csharp
var evaluacionesConPlan = from e in db2.DPA_Evaluacion
                          join p in db2.DPA_Plan_Anual on e.ID_Plan_Anual equals p.ID_Plan_Anual
                          where p.Anio == 2025
                          select new
                          {
                              Evaluacion = e.Nombre,
                              Plan = p.Anio,
                              Estado = e.Estado
                          };
```

### 3. Ejecución de SP con Parámetros
```csharp
int idPlan = 14;
var resultado = db2.SP_DPA_Mantenimiento_Plan(idPlan).ToList();
```

### 4. Transacción Compleja
```csharp
using (var transaction = db2.Database.BeginTransaction())
{
    try
    {
        // Operación 1
        var plan = new DPA_Plan_Anual { Anio = 2025 };
        db2.DPA_Plan_Anual.Add(plan);
        db2.SaveChanges();
        
        // Operación 2
        var evaluacion = new DPA_Evaluacion 
        { 
            ID_Plan_Anual = plan.ID_Plan_Anual,
            Nombre = "Evaluación Test"
        };
        db2.DPA_Evaluacion.Add(evaluacion);
        db2.SaveChanges();
        
        transaction.Commit();
    }
    catch (Exception ex)
    {
        transaction.Rollback();
        throw;
    }
}
```

### 5. Carga Eager Loading
```csharp
var planes = db2.DPA_Plan_Anual
    .Include(p => p.DPA_Evaluacion)
    .Include(p => p.DPA_Negocio)
    .Where(p => p.Anio == 2025)
    .ToList();
```

---

## Convenciones y Mejores Prácticas

### Nomenclatura
- ✅ **Prefijos consistentes**: DPA_, DD_, SP_, fn_
- ✅ **PascalCase** para nombres de propiedades
- ✅ **Sufijo _Result** para resultados de SPs

### Performance
- ⚡ Usar `.AsNoTracking()` para consultas de solo lectura
- ⚡ Proyectar solo campos necesarios con `.Select()`
- ⚡ Limitar resultados con `.Take()` cuando sea apropiado
- ⚡ Indexar correctamente en la base de datos

### Seguridad
- 🔒 Nunca exponer entidades de EF directamente en APIs públicas
- 🔒 Usar ViewModels para transferencia de datos
- 🔒 Validar siempre en el servidor, no solo en cliente
- 🔒 Sanitizar inputs antes de usar en queries

### Mantenimiento
- 📝 Documentar stored procedures complejos
- 📝 Mantener sincronizado el modelo EDMX con la BD
- 📝 Versionar cambios en el esquema de base de datos
- 📝 Usar migraciones o scripts SQL controlados

---

## Diagrama de Relaciones Principales

```
DPA_Plan_Anual (1) -----> (N) DPA_Evaluacion
                                    |
                                    | (1)
                                    |
                                    v (N)
                            DPA_Evaluacion_Plan
                                    |
                                    | (1)
                                    |
                                    v (N)
                            DPA_Auditor_Evaluacion
                                    |
                                    | (N)
                                    |
                                    v (1)
                                Persona (Auditor)

DD_Universo (N) <-----> (N) DPA_Evaluacion
DD_Proyecto (1) -----> (N) DPA_Evaluacion

Persona (1) -----> (N) UsuarioModuloRol (N) <-----> (1) Modulo
                         |
                         | (N)
                         |
                         v (1)
                        Rol
```

---

## Referencia Rápida

### Context
```csharp
PROYECTOSIAV2Entities1 db2 = new PROYECTOSIAV2Entities1();
```

### Contenedor
```csharp
ContenedorModelos modelDB = new ContenedorModelos();
```

### Sesión
```csharp
SesionData sesion = new SesionData();
sesion.setSession("nombre", "valor");
```

### Ejecución de SP
```csharp
var resultado = db2.SP_Nombre(parametro).ToList();
```

---

**Nota**: Esta documentación está basada en Entity Framework 6.1.3 con enfoque Database First. Para modificaciones en el esquema, regenerar los modelos desde Visual Studio usando "Update Model from Database".
