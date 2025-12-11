# TIGA - Sistema de Gestión de Auditoría Interna

## 📋 Descripción General

TIGA (Sistema de Gestión de Auditoría Interna) es una aplicación web empresarial tipo ERP desarrollada para gestionar los procesos de auditoría interna de la organización. El sistema permite la planificación, seguimiento y control de proyectos de auditoría, gestión de observaciones, evaluaciones de riesgos y generación de informes ejecutivos.

## 🏗️ Arquitectura y Tecnologías

### Framework Principal
- **ASP.NET MVC 5.2.4** (.NET Framework 4.6.1)
  - Patrón de diseño Modelo-Vista-Controlador
  - Separación clara de responsabilidades
  - Routing basado en convenciones
  - Razor como motor de vistas

### Tecnologías Backend

#### ORM y Acceso a Datos
- **Entity Framework 6.1.3**
  - Database First approach
  - Stored Procedures como fuentes de datos principales
  - Context: `PROYECTOSIAV2Entities1`
  - Soporte para transacciones complejas

#### Librerías de Procesamiento
- **EPPlus 5.8.2**: Generación y manipulación de archivos Excel (XLSX)
- **LinqToExcel 1.11.0**: Lectura de archivos Excel para importación de datos
- **DocumentFormat.OpenXml 2.15.0**: Procesamiento avanzado de documentos Office
- **Newtonsoft.Json**: Serialización/deserialización JSON

#### Seguridad y Autenticación
- **Microsoft.AspNetCore.Authentication**: Gestión de autenticación
- **Microsoft.AspNetCore.Authorization**: Sistema de autorización basado en roles
- **Microsoft.AspNetCore.DataProtection**: Protección de datos sensibles
- **Atributo personalizado**: `[Logueado]` para autorización a nivel de controlador

### Tecnologías Frontend

#### UI Framework
- **Bootstrap 3.3.7**: Framework CSS responsivo
- **jQuery 3.3.1**: Manipulación del DOM y AJAX
- **jQuery Validation 1.17.0**: Validación de formularios del lado del cliente

#### Optimización
- **Microsoft.AspNet.Web.Optimization 1.1.3**: Bundling y minificación de recursos CSS/JS

## 📁 Estructura del Proyecto

```
WebTIGA/
│
├── App_Start/              # Configuración de inicio de la aplicación
│   ├── BundleConfig.cs     # Configuración de bundles CSS/JS
│   ├── FilterConfig.cs     # Filtros globales
│   └── RouteConfig.cs      # Configuración de rutas
│
├── Autorizacion/           # Sistema de autorización personalizado
│   └── Logueado.cs         # Atributo de autorización para controladores
│
├── Controllers/            # Controladores MVC
│   ├── HomeController.cs
│   ├── LoginController.cs
│   ├── AdministradorUsuariosController.cs
│   ├── WebPlanAnualController.cs
│   ├── WebSeguimientoObservacionesController.cs
│   └── Encriptar/
│       └── Encrypt.cs      # Utilidades de encriptación
│
├── Models/                 # Modelos de datos y entidades
│   ├── ContenedorModelos.cs        # Contenedor de colecciones
│   ├── PROYECTOSIAV2Entities1.cs   # Contexto Entity Framework
│   ├── [Entidades de BD].cs        # Modelos generados por EF
│   └── [SP_Results].cs             # Resultados de Stored Procedures
│
├── Views/                  # Vistas Razor
│   ├── Shared/             # Vistas compartidas y layouts
│   ├── Home/               # Vistas del módulo principal
│   ├── Login/              # Vistas de autenticación
│   ├── AdministradorUsuarios/      # Vistas de administración
│   ├── WebPlanAnual/               # Vistas de planificación anual
│   └── WebSeguimientoObservaciones/ # Vistas de seguimiento
│
├── Content/                # Recursos estáticos
│   ├── css/                # Hojas de estilo
│   ├── js/                 # JavaScript
│   └── scss/               # Archivos SASS
│
├── App_Data/               # Datos de aplicación
│
├── bin/                    # Binarios compilados
│
└── Web.config              # Configuración principal de la aplicación
```

## 🎯 Módulos Funcionales

### 1. **Módulo de Autenticación y Autorización**
- Login de usuarios con validación
- Recuperación de contraseña
- Sistema de sesiones
- Control de acceso basado en roles (RBAC)
- Gestión de módulos por usuario

### 2. **Plan Anual de Auditoría**
- **Elaboración del Plan Anual**: Creación y diseño del plan anual de auditoría
- **Elaboración Plan Dinámico**: Ajustes dinámicos al plan durante el año
- **Ejecución del Plan**: Seguimiento de la ejecución de proyectos planificados
- **Gestión de Evaluaciones**: Creación y seguimiento de evaluaciones
- **Calendario de Trabajo**: Visualización y gestión de cronogramas
- **Programación Final**: Cierre y ajustes finales del plan
- **Carga Masiva**: Importación de datos desde Excel
- **Universo Auditable**: Gestión de entidades auditables
- **Sistema de Votación**: Votación para priorización de proyectos
- **Scoring de Riesgos**: Evaluación cuantitativa de riesgos

### 3. **Seguimiento de Observaciones**
- Registro y seguimiento de observaciones de auditoría
- Clasificación por nivel de riesgo
- Gestión de planes de acción
- Seguimiento de compromisos
- Reportes de estado y cumplimiento
- Estructura de seguimiento organizacional

### 4. **Administración de Usuarios**
- Gestión completa de usuarios (CRUD)
- Asignación de roles y permisos
- Gestión de rotación de personal
- Administración de módulos
- Gestión de estructura organizacional:
  - Unidades de contacto
  - Unidades responsables
  - Equipos de trabajo

### 5. **Reportería y Análisis**
- Reportes ejecutivos de auditoría
- Gráficos de control y seguimiento
- Estadísticas de horas y recursos
- Análisis de efectividad de controles
- Reportes de cuentas contables (Balance General, Ganancias y Pérdidas)
- Exportación a Excel

## 🔐 Seguridad

### Autenticación
- Sistema de login centralizado
- Gestión de sesiones mediante `System.Web.SessionState`
- Recuperación de contraseña con tokens de seguridad
- Encriptación de contraseñas

### Autorización
- Atributo `[Logueado]` aplicado a nivel de controlador
- Verificación de sesión activa antes de cada acción
- Control de acceso por módulos y roles
- Validación de permisos por funcionalidad

### Protección de Datos
- Uso de Microsoft.AspNetCore.DataProtection
- Parámetros de configuración en Web.config protegidos
- Validación de entrada en formularios
- Protección CSRF mediante tokens antiforgery

## 💾 Base de Datos

### Estrategia de Acceso
- **Database First**: Los modelos se generan desde la base de datos existente
- **Stored Procedures**: La mayor parte de la lógica de negocio reside en SPs
- **Entity Framework**: Capa de abstracción para acceso a datos

### Convenciones de Nomenclatura

#### Stored Procedures
- `SP_[Módulo]_[Descripción]`: Procedimientos almacenados principales
  - Ejemplo: `SP_DPA_Mantenimiento_Plan`, `SP_BC_PROYECTO_PLAN`
  
- `fn_[Descripción]`: Funciones de tabla
  - Ejemplo: `fn_TG_Obtener_Proyectos_v2`

#### Tablas Principales (por prefijo)
- **DPA_**: Datos de Plan Anual
  - `DPA_Plan_Anual`, `DPA_Evaluacion`, `DPA_Votacion`
  
- **DD_**: Diccionario de Datos
  - `DD_Proyecto`, `DD_Universo`, `DD_Informe`
  
- **SP_**: Resultados de Stored Procedures
  - Clases generadas automáticamente terminadas en `_Result`

### Context Principal
```csharp
PROYECTOSIAV2Entities1 db2 = new PROYECTOSIAV2Entities1();
```

## 🔄 Flujo de Trabajo General

### 1. Usuario Accede al Sistema
```
Login → Validación → Creación de Sesión → Carga de Módulos Permitidos → Dashboard
```

### 2. Operación CRUD Típica
```
Vista → Acción del Controlador → Validación → 
Ejecución de SP/EF → Retorno de Datos → 
Renderización de Vista con Modelo
```

### 3. Procesamiento de Excel
```
Upload de Archivo → LinqToExcel/EPPlus → 
Validación de Datos → Transacción EF → 
Almacenamiento en BD → Confirmación
```

## 📊 Patrón de Diseño

### Contenedor de Modelos
La aplicación utiliza un patrón de **Contenedor de Colecciones** mediante la clase `ContenedorModelos.cs`:

```csharp
public class ContenedorModelos
{
    public IEnumerable<DPA_Plan_Anual> DPA_Plan_Anual { get; set; }
    public IEnumerable<DPA_Evaluacion> DPA_Evaluacion { get; set; }
    // ... más colecciones
}
```

**Ventajas:**
- Permite pasar múltiples colecciones a una vista
- Facilita la composición de datos complejos
- Reduce el número de llamadas a la base de datos

### Separación de Responsabilidades

#### Controladores
- Manejo de solicitudes HTTP
- Validación de entrada
- Coordinación entre modelos y vistas
- Gestión de sesiones

#### Modelos
- Representación de entidades de datos
- Lógica de negocio simple (validaciones de datos)
- Ejecución de stored procedures mediante EF

#### Vistas
- Presentación de datos
- Formularios de entrada
- Vistas parciales reutilizables
- Layouts compartidos

## 🚀 Características Técnicas Destacadas

### Transacciones
```csharp
using (var transaction = db2.Database.BeginTransaction())
{
    try 
    {
        // Operaciones
        transaction.Commit();
    }
    catch 
    {
        transaction.Rollback();
    }
}
```

### Manejo de Sesiones
```csharp
Session["IdUser"] = usuario.IdPersona;
Session["usuario"] = usuario.Nombre;
Session["idRol"] = rol.IdRol;
```

### Generación de Excel con EPPlus
```csharp
using (ExcelPackage package = new ExcelPackage())
{
    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Reporte");
    // Configuración y llenado de datos
    return File(package.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "reporte.xlsx");
}
```

### Ejecución de Stored Procedures
```csharp
modelDB.SP_DPA_Mantenimiento_Plan = db2.SP_DPA_Mantenimiento_Plan(idPlan);
```

## 📚 Documentación Adicional

Para información más detallada sobre cada componente, consulte:

- **[MODELS.md](./docs/MODELS.md)**: Documentación completa de modelos y acceso a datos
- **[CONTROLLERS.md](./docs/CONTROLLERS.md)**: Guía detallada de controladores y acciones
- **[VIEWS.md](./docs/VIEWS.md)**: Estructura de vistas y componentes de UI
- **[SETUP.md](./docs/SETUP.md)**: Guía de instalación y configuración

## 🔧 Configuración de Entorno

### Requisitos Previos
- Visual Studio 2017 o superior
- .NET Framework 4.6.1
- SQL Server 2014 o superior
- IIS 7.0 o superior (para despliegue)

### Archivos de Configuración
- **Web.config**: Configuración principal (conexión a BD, appSettings)
- **Web.Debug.config**: Transformaciones para ambiente de desarrollo
- **Web.Release.config**: Transformaciones para producción

## 👥 Roles del Sistema

1. **Administrador**: Acceso completo a todos los módulos
2. **Jefe de Auditoría**: Gestión de planes y evaluaciones
3. **Auditor**: Ejecución de auditorías y registro de observaciones
4. **Consultor**: Solo lectura de información

## 📝 Convenciones de Código

- **Nomenclatura de controladores**: `[Módulo]Controller.cs`
- **Nomenclatura de vistas**: `[Acción].cshtml`
- **Prefijo de vistas parciales**: `PV_[Nombre].cshtml`
- **ViewBag para datos simples**, modelos fuertemente tipados para datos complejos
- **Async/Await**: No implementado en la versión actual (legacy)

## 🌐 Navegadores Soportados

- Google Chrome (recomendado)
- Microsoft Edge
- Mozilla Firefox
- Safari

## 📞 Soporte

Para consultas técnicas o reportar problemas, contactar al equipo de desarrollo de auditoría interna.

---

**Versión del Sistema**: 2.0  
**Última Actualización**: 2025  
**Namespace Principal**: `WebTIGA`
