# VIEWS.md - Documentación de Vistas

## 📋 Índice
1. [Introducción](#introducción)
2. [Arquitectura de Vistas](#arquitectura-de-vistas)
3. [Layouts](#layouts)
4. [Organización por Módulos](#organización-por-módulos)
5. [Vistas Parciales](#vistas-parciales)
6. [Helpers y Componentes](#helpers-y-componentes)
7. [JavaScript y Estilos](#javascript-y-estilos)
8. [Patrones y Convenciones](#patrones-y-convenciones)

---

## Introducción

Las vistas en TIGA utilizan **Razor** como motor de plantillas (`.cshtml`). Implementan el patrón de diseño de layouts compartidos, vistas parciales reutilizables y una separación clara de responsabilidades entre presentación y lógica.

## Arquitectura de Vistas

### Estructura de Carpetas

```
Views/
├── Shared/                          # Vistas compartidas
│   ├── _Layout.cshtml               # Layout base (legacy)
│   ├── _Layout_Home.cshtml          # Layout dashboard principal
│   ├── _Layout_Login.cshtml         # Layout para autenticación
│   ├── _Layout_Plan_Anual.cshtml    # Layout módulo Plan Anual
│   ├── _Layout_Seguimiento_Observaciones.cshtml
│   ├── _Layout_Administrador.cshtml # Layout administración
│   ├── _Layout_Forgot_Password.cshtml
│   ├── Error.cshtml                 # Vista de error global
│   └── _ViewStart.cshtml            # Configuración de layout por defecto
│
├── Home/                            # Dashboard principal
│   └── Index.cshtml
│
├── Login/                           # Autenticación
│   ├── Login_.cshtml
│   ├── Forgot_Password.cshtml
│   ├── Recovery.cshtml
│   └── Notificacion_token.cshtml
│
├── AdministradorUsuarios/           # Administración
│   ├── Administrador_U.cshtml
│   ├── Administrador_Usuarios.cshtml
│   ├── Administrador_Modulos.cshtml
│   ├── Administrador_Rotacion.cshtml
│   ├── CrearUsuario.cshtml
│   ├── EstructuraSeguimiento.cshtml
│   ├── PV_*.cshtml                  # Vistas parciales (modals)
│   └── ...
│
├── WebPlanAnual/                    # Plan Anual
│   ├── Inicio.cshtml
│   ├── ElaboracionPlanAnual.cshtml
│   ├── ElaboracionPlanDinamico.cshtml
│   ├── EjecucionPlanAnual.cshtml
│   ├── GestionEvaluaciones.cshtml
│   ├── CargaPlanAnual.cshtml
│   ├── CalendarioTrabajo.cshtml
│   ├── ProgramacionFinal.cshtml
│   ├── VotacionAuditor.cshtml
│   ├── Universo_Plan_Anual.cshtml
│   ├── MantenimientoPlan.cshtml
│   ├── PV_*.cshtml                  # Vistas parciales (modals)
│   └── ...
│
└── WebSeguimientoObservaciones/     # Seguimiento
    ├── Inicio.cshtml
    ├── DetalleIndividual.cshtml
    ├── EstructuraSeguimiento.cshtml
    ├── PV_*.cshtml
    └── ...
```

### _ViewStart.cshtml

Configuración global del layout por defecto:

```cshtml
@{
    Layout = "~/Views/Shared/_Layout.cshtml";
}
```

**Nota**: Cada vista puede sobrescribir el layout:

```cshtml
@{
    Layout = "~/Views/Shared/_Layout_Home.cshtml";
}
```

---

## Layouts

Los layouts definen la estructura HTML común y los elementos de navegación. TIGA utiliza múltiples layouts especializados por módulo.

### 1. _Layout_Home.cshtml

**Propósito**: Dashboard principal del sistema (Home/Index).

**Estructura**:
```cshtml
@model WebTIGA.Models.ContenedorModelos

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8">
    <title>TIGA-WEB-Home</title>
    
    <!-- Google Fonts -->
    <link href="https://fonts.googleapis.com/css?family=Montserrat:400,700" rel="stylesheet">
    
    <!-- Bootstrap 4 -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css">
    
    <!-- Font Awesome -->
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.7.0/css/all.css">
    
    <!-- jQuery -->
    <script src="https://code.jquery.com/jquery-3.3.1.min.js"></script>
    
    <!-- Bundles CSS personalizados -->
    @Styles.Render("~/bundles/css")
</head>
<body id="page-top">
    <!-- Barra de navegación superior -->
    <nav class="navbar navbar-expand-lg navbar-dark fixed-top" id="mainNav">
        <!-- Logo y navegación -->
    </nav>
    
    <!-- Contenido de la vista -->
    @RenderBody()
    
    <!-- Footer -->
    <footer>
        <!-- Información del footer -->
    </footer>
    
    <!-- Scripts -->
    @Scripts.Render("~/bundles/js")
    @RenderSection("scripts", required: false)
</body>
</html>
```

**Características**:
- Header con animación masthead
- Grid de módulos disponibles
- Navegación fija superior
- Footer corporativo
- Tema Agency de Bootstrap

### 2. _Layout_Plan_Anual.cshtml

**Propósito**: Layout para el módulo de Plan Anual.

**Estructura**:
```cshtml
<!DOCTYPE html>
<html lang="en">
<head>
    <title>WEB-TIGA - Plan Anual</title>
    <link href="~/Content/css/plan-anual.css" rel="stylesheet" type="text/css" />
    @Styles.Render("~/bundles/css")
</head>
<body id="page-top">
    <div id="wrapper">
        
        <!-- Sidebar izquierdo -->
        <ul class="navbar-nav sidebar sidebar-dark accordion bg-dark" id="accordionSidebar">
            
            <!-- Brand -->
            <a class="sidebar-brand" href="@Url.Action("Inicio", "WebPlanAnual")">
                <div class="sidebar-brand-icon">
                    <i class="fas fa-tasks"></i>
                </div>
                <div class="sidebar-brand-text">PLAN ANUAL</div>
            </a>
            
            <hr class="sidebar-divider">
            
            <!-- Menú de navegación -->
            <li class="nav-item">
                <a href="@Url.Action("ElaboracionPlanAnual", "WebPlanAnual")">
                    <i class="fas fa-home"></i> Plan Anual
                </a>
            </li>
            
            <li class="nav-item">
                <a href="@Url.Action("EjecucionPlanAnual", "WebPlanAnual")">
                    <i class="fas fa-check-circle"></i> Ejecución
                </a>
            </li>
            
            <!-- Más items de menú... -->
            
            <!-- Botón regresar a Home -->
            <li class="nav-item">
                <a href="@Url.Action("Index", "Home")">
                    <i class="fas fa-arrow-left"></i> Regresar
                </a>
            </li>
        </ul>
        
        <!-- Content Wrapper -->
        <div id="content-wrapper" class="d-flex flex-column">
            
            <!-- Main Content -->
            <div id="content">
                
                <!-- Topbar -->
                <nav class="navbar navbar-expand navbar-light bg-white topbar static-top shadow">
                    
                    <!-- Sidebar Toggle -->
                    <button id="sidebarToggleTop" class="btn btn-link d-md-none">
                        <i class="fa fa-bars"></i>
                    </button>
                    
                    <!-- Usuario y opciones -->
                    <ul class="navbar-nav ml-auto">
                        <li class="nav-item dropdown">
                            <a class="nav-link dropdown-toggle" href="#" id="userDropdown">
                                <span class="mr-2 d-none d-lg-inline text-gray-600 small">
                                    @ViewBag.nombre
                                </span>
                                <i class="fas fa-user-circle fa-2x"></i>
                            </a>
                            <div class="dropdown-menu">
                                <a class="dropdown-item" href="@Url.Action("CerrarSesion", "Home")">
                                    <i class="fas fa-sign-out-alt"></i> Cerrar Sesión
                                </a>
                            </div>
                        </li>
                    </ul>
                </nav>
                
                <!-- Contenido principal -->
                <div class="container-fluid">
                    @RenderBody()
                </div>
            </div>
            
            <!-- Footer -->
            <footer class="sticky-footer bg-white">
                <div class="container my-auto">
                    <div class="copyright text-center my-auto">
                        <span>Copyright &copy; Pacífico Seguros 2025</span>
                    </div>
                </div>
            </footer>
        </div>
    </div>
    
    @Scripts.Render("~/bundles/js")
    @RenderSection("scripts", required: false)
</body>
</html>
```

**Características**:
- Sidebar colapsable con navegación del módulo
- Topbar con información del usuario
- Dropdown de opciones de usuario
- Footer fijo
- Diseño responsivo
- Toggle para dispositivos móviles

### 3. _Layout_Login.cshtml

**Propósito**: Layout minimalista para páginas de autenticación.

**Características**:
- Sin navegación
- Centrado en contenido de login
- Fondo corporativo
- Formulario centralizado
- Sin footer ni sidebar

### 4. _Layout_Seguimiento_Observaciones.cshtml

**Propósito**: Layout para el módulo de Seguimiento de Observaciones.

**Similar a Plan Anual** con:
- Menú específico de seguimiento
- Filtros avanzados en topbar
- Navegación a reportes
- Accesos a estructura organizacional

### 5. _Layout_Administrador.cshtml

**Propósito**: Layout para módulo de administración.

**Características**:
- Menú de administración (Usuarios, Roles, Módulos)
- Navegación a rotación
- Estructura de seguimiento
- Permisos visuales según rol

---

## Organización por Módulos

### Home/Index.cshtml

**Vista principal del dashboard**.

```cshtml
@model WebTIGA.Models.ContenedorModelos
@{
    Layout = "~/Views/Shared/_Layout_Home.cshtml";
}

<header class="masthead">
    <div class="container">
        <div class="intro-text">
            <div class="intro-lead-in">@ViewBag.Bienvenido</div>
            <div class="intro-heading text-uppercase">@ViewBag.nombre</div>
            <a class="btn btn-primary btn-xl text-uppercase" href="#portfolio">
                Empezar
            </a>
        </div>
    </div>
</header>

<section class="bg-light page-section" id="portfolio">
    <div class="container">
        <div class="row">
            <div class="col-lg-12 text-center">
                <h2 class="section-heading text-uppercase">MÓDULOS DE AUDITORÍA</h2>
                <h3 class="section-subheading text-muted">
                    A continuación se muestran los módulos utilizados en auditoría interna Pacífico.
                </h3>
            </div>
        </div>
        
        <div class="row">
            @foreach (var item in Model.SP_MODULOS_USUARIOS_Result)
            {
                if (item.habilitado == 1)
                {
                    string habilitar = item.activo != 1 ? "disable" : "";
                    string cursor = item.activo != 1 ? "cursor: not-allowed;" : "";
                    string url = item.activo != 1 ? "#" : item.Enlace;
                    
                    <div class="col-md-3 col-sm-6 portfolio-item">
                        <a class="portfolio-link" 
                           data-toggle="modal" 
                           title="@item.Descripcion" 
                           disabled="@habilitar" 
                           style="@cursor" 
                           href="@url"
                           onclick="window.open(this.href); return false;">
                            
                            <div class="portfolio-hover">
                                <div class="portfolio-hover-content">
                                    <i class="fa-3x"></i>
                                </div>
                            </div>
                            <img class="img-fluid" src="~/img/portfolio/@item.IMG" alt="">
                        </a>
                        <div class="portfolio-caption">
                            <h4>@item.Nombre</h4>
                            <p class="text-muted">Web</p>
                        </div>
                    </div>
                }
            }
        </div>
    </div>
</section>
```

**Lógica de la vista**:
1. Itera sobre los módulos del usuario (`SP_MODULOS_USUARIOS_Result`)
2. Valida si el módulo está habilitado
3. Deshabilita visualmente módulos inactivos
4. Muestra imagen y enlace a cada módulo
5. Grid responsivo de 4 columnas (col-md-3)

### Login/Login_.cshtml

**Formulario de inicio de sesión**.

```cshtml
@model WebTIGA.Models.UserLogin
@{
    Layout = "~/Views/Shared/_Layout_Login.cshtml";
}

<div class="container">
    <div class="row justify-content-center">
        <div class="col-xl-5 col-lg-6 col-md-9">
            <div class="card o-hidden border-0 shadow-lg my-5">
                <div class="card-body p-0">
                    <div class="row">
                        <div class="col-lg-12">
                            <div class="p-5">
                                <div class="text-center">
                                    <img src="~/img/logo.png" class="img-fluid" alt="Logo">
                                    <h1 class="h4 text-gray-900 mb-4">TIGA - Sistema de Auditoría</h1>
                                </div>
                                
                                @using (Html.BeginForm("Login_", "Login", FormMethod.Post, 
                                        new { @class = "user" }))
                                {
                                    @Html.AntiForgeryToken()
                                    
                                    <div class="form-group">
                                        @Html.TextBoxFor(m => m.Usuario, 
                                            new { @class = "form-control form-control-user", 
                                                  @placeholder = "Usuario" })
                                        @Html.ValidationMessageFor(m => m.Usuario, "", 
                                            new { @class = "text-danger" })
                                    </div>
                                    
                                    <div class="form-group">
                                        @Html.PasswordFor(m => m.Password, 
                                            new { @class = "form-control form-control-user", 
                                                  @placeholder = "Contraseña" })
                                        @Html.ValidationMessageFor(m => m.Password, "", 
                                            new { @class = "text-danger" })
                                    </div>
                                    
                                    <button type="submit" 
                                            class="btn btn-primary btn-user btn-block">
                                        Iniciar Sesión
                                    </button>
                                }
                                
                                <hr>
                                
                                <div class="text-center">
                                    <a class="small" href="@Url.Action("Forgot_Password", "Login")">
                                        ¿Olvidaste tu contraseña?
                                    </a>
                                </div>
                                
                                @if (ViewBag.Message1 != null)
                                {
                                    <div class="alert alert-danger mt-3">
                                        @ViewBag.Message1
                                    </div>
                                }
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

@section scripts {
    @Scripts.Render("~/bundles/jqueryval")
}
```

**Elementos clave**:
- `Html.BeginForm`: Genera formulario con CSRF token
- `Html.TextBoxFor` / `Html.PasswordFor`: Inputs fuertemente tipados
- `Html.ValidationMessageFor`: Mensajes de validación
- `@Html.AntiForgeryToken()`: Protección CSRF
- `@section scripts`: Sección para scripts específicos

### WebPlanAnual/ElaboracionPlanAnual.cshtml

**Vista principal de elaboración del plan anual**.

```cshtml
@model WebTIGA.Models.ContenedorModelos
@{
    Layout = "~/Views/Shared/_Layout_Plan_Anual.cshtml";
}

<div class="container-fluid">
    
    <!-- Page Heading -->
    <div class="d-sm-flex align-items-center justify-content-between mb-4">
        <h1 class="h3 mb-0 text-gray-800">Elaboración Plan Anual</h1>
        @if (ViewBag.idRol == 1 || ViewBag.idRol == 2)
        {
            <button class="btn btn-success" data-toggle="modal" data-target="#modalCrearPlan">
                <i class="fas fa-plus"></i> Crear Plan
            </button>
        }
    </div>
    
    <!-- Filtros -->
    <div class="card shadow mb-4">
        <div class="card-header py-3">
            <h6 class="m-0 font-weight-bold text-primary">Filtros</h6>
        </div>
        <div class="card-body">
            <div class="row">
                <div class="col-md-3">
                    <label>Año:</label>
                    <select id="filtroAnio" class="form-control">
                        <option value="2024">2024</option>
                        <option value="2025" selected>2025</option>
                        <option value="2026">2026</option>
                    </select>
                </div>
                <div class="col-md-3">
                    <label>Estado:</label>
                    <select id="filtroEstado" class="form-control">
                        <option value="">Todos</option>
                        <option value="En Elaboración">En Elaboración</option>
                        <option value="Aprobado">Aprobado</option>
                        <option value="En Ejecución">En Ejecución</option>
                    </select>
                </div>
                <div class="col-md-3">
                    <button class="btn btn-primary mt-4" onclick="filtrarPlanes()">
                        <i class="fas fa-search"></i> Buscar
                    </button>
                </div>
            </div>
        </div>
    </div>
    
    <!-- Tabla de Planes -->
    <div class="card shadow mb-4">
        <div class="card-header py-3">
            <h6 class="m-0 font-weight-bold text-primary">Planes Anuales</h6>
        </div>
        <div class="card-body">
            <div class="table-responsive">
                <table class="table table-bordered" id="dataTable" width="100%">
                    <thead>
                        <tr>
                            <th>ID</th>
                            <th>Año</th>
                            <th>Estado</th>
                            <th>Fecha Creación</th>
                            <th>Usuario Creación</th>
                            <th>Acciones</th>
                        </tr>
                    </thead>
                    <tbody>
                        @foreach (var plan in Model.DPA_Plan_Anual)
                        {
                            <tr>
                                <td>@plan.ID_Plan_Anual</td>
                                <td>@plan.Anio</td>
                                <td>
                                    <span class="badge badge-@(plan.Estado == "Aprobado" ? "success" : "warning")">
                                        @plan.Estado
                                    </span>
                                </td>
                                <td>@plan.Fecha_Creacion?.ToString("dd/MM/yyyy")</td>
                                <td>@plan.Usuario_Creacion</td>
                                <td>
                                    <a href="@Url.Action("MantenimientoPlan", new { idPlan = plan.ID_Plan_Anual })" 
                                       class="btn btn-sm btn-info">
                                        <i class="fas fa-edit"></i> Editar
                                    </a>
                                    <button class="btn btn-sm btn-primary" 
                                            onclick="verDetalle(@plan.ID_Plan_Anual)">
                                        <i class="fas fa-eye"></i> Ver
                                    </button>
                                </td>
                            </tr>
                        }
                    </tbody>
                </table>
            </div>
        </div>
    </div>
</div>

<!-- Modal Crear Plan -->
<div class="modal fade" id="modalCrearPlan" tabindex="-1">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
            @Html.Partial("PV_CrearPlan")
        </div>
    </div>
</div>

@section scripts {
    <script>
        function filtrarPlanes() {
            var anio = $('#filtroAnio').val();
            var estado = $('#filtroEstado').val();
            
            window.location.href = '@Url.Action("ElaboracionPlanAnual")' + 
                '?anio=' + anio + '&estado=' + estado;
        }
        
        function verDetalle(idPlan) {
            window.location.href = '@Url.Action("MantenimientoPlan")' + 
                '?idPlan=' + idPlan;
        }
        
        // DataTables initialization
        $(document).ready(function() {
            $('#dataTable').DataTable({
                "language": {
                    "url": "//cdn.datatables.net/plug-ins/1.10.24/i18n/Spanish.json"
                },
                "order": [[0, "desc"]]
            });
        });
    </script>
}
```

**Elementos destacados**:
- **Validación de roles**: `@if (ViewBag.idRol == 1)`
- **Razor foreach**: Iteración sobre colecciones
- **Html.Partial**: Inclusión de vistas parciales
- **Helpers de URL**: `@Url.Action()`
- **Formateo de fechas**: `ToString("dd/MM/yyyy")`
- **Badges condicionales**: Cambio de color según estado
- **DataTables**: Plugin para tablas interactivas
- **Modals de Bootstrap**: Para formularios emergentes

### WebSeguimientoObservaciones/Inicio.cshtml

**Vista principal de seguimiento con filtros complejos**.

```cshtml
@model WebTIGA.Models.ContenedorModelos
@{
    Layout = "~/Views/Shared/_Layout_Seguimiento_Observaciones.cshtml";
}

<div class="container-fluid">
    
    <!-- Filtros Avanzados -->
    <div class="card shadow mb-4">
        <div class="card-header py-3 d-flex justify-content-between">
            <h6 class="m-0 font-weight-bold text-primary">Filtros de Búsqueda</h6>
            <button class="btn btn-sm btn-secondary" onclick="limpiarFiltros()">
                <i class="fas fa-eraser"></i> Limpiar
            </button>
        </div>
        <div class="card-body">
            <form id="formFiltros" method="get" action="@Url.Action("Inicio")">
                <div class="row">
                    <div class="col-md-3">
                        <label>Fecha de Corte:</label>
                        <input type="text" id="fechaCorte" name="fechaCorte" 
                               class="form-control datepicker" 
                               value="@ViewBag.FechaCorte" />
                    </div>
                    
                    <div class="col-md-3">
                        <label>Año:</label>
                        <select name="filtroAnio" class="form-control">
                            @foreach (var anio in ViewBag.AñosObservaciones)
                            {
                                <option value="@anio">@anio</option>
                            }
                        </select>
                    </div>
                    
                    <div class="col-md-6">
                        <label>Estado de Observaciones:</label>
                        <div class="form-check form-check-inline">
                            <input type="checkbox" name="o_check" value="1" checked>
                            <label>Abiertas</label>
                        </div>
                        <div class="form-check form-check-inline">
                            <input type="checkbox" name="v_check" value="1" checked>
                            <label>Vencidas</label>
                        </div>
                        <div class="form-check form-check-inline">
                            <input type="checkbox" name="p_check" value="1" checked>
                            <label>Próximas a Vencer</label>
                        </div>
                        <div class="form-check form-check-inline">
                            <input type="checkbox" name="c_check" value="1">
                            <label>Cerradas</label>
                        </div>
                    </div>
                </div>
                
                <div class="row mt-3">
                    <div class="col-md-12 text-right">
                        <button type="submit" class="btn btn-primary">
                            <i class="fas fa-search"></i> Buscar
                        </button>
                        <button type="button" class="btn btn-success" onclick="descargarExcel()">
                            <i class="fas fa-file-excel"></i> Exportar a Excel
                        </button>
                    </div>
                </div>
            </form>
        </div>
    </div>
    
    <!-- Grid de Observaciones -->
    <div class="card shadow mb-4">
        <div class="card-header py-3">
            <h6 class="m-0 font-weight-bold text-primary">
                Observaciones (@Model.fn_Stock_Observaciones_Integrado_v7.Count())
            </h6>
        </div>
        <div class="card-body">
            <div class="table-responsive">
                <table class="table table-bordered table-hover" id="tableObservaciones">
                    <thead class="thead-light">
                        <tr>
                            <th>ID</th>
                            <th>Proyecto</th>
                            <th>Título</th>
                            <th>Unidad Responsable</th>
                            <th>Propietario</th>
                            <th>Riesgo</th>
                            <th>Estado</th>
                            <th>Fecha Vencimiento</th>
                            <th>Acciones</th>
                        </tr>
                    </thead>
                    <tbody>
                        @foreach (var obs in Model.fn_Stock_Observaciones_Integrado_v7)
                        {
                            string rowClass = "";
                            if (obs.Estado == "Vencida") rowClass = "table-danger";
                            else if (obs.Estado == "Próxima a Vencer") rowClass = "table-warning";
                            
                            <tr class="@rowClass">
                                <td>@obs.ID</td>
                                <td>@obs.Proyecto</td>
                                <td>@obs.Titulo_Observacion</td>
                                <td>@obs.Unidad_Responsable</td>
                                <td>@obs.Propietario</td>
                                <td>
                                    <span class="badge badge-@(obs.Riesgo == "Alto" ? "danger" : obs.Riesgo == "Medio" ? "warning" : "success")">
                                        @obs.Riesgo
                                    </span>
                                </td>
                                <td>@obs.Estado</td>
                                <td>@obs.Fecha_Vencimiento?.ToString("dd/MM/yyyy")</td>
                                <td>
                                    <a href="@Url.Action("DetalleIndividual", new { id = obs.ID })" 
                                       class="btn btn-sm btn-info">
                                        <i class="fas fa-eye"></i>
                                    </a>
                                </td>
                            </tr>
                        }
                    </tbody>
                </table>
            </div>
        </div>
    </div>
</div>

@section scripts {
    <script>
        $(document).ready(function() {
            // Datepicker
            $('.datepicker').datepicker({
                format: 'dd/mm/yyyy',
                language: 'es',
                autoclose: true
            });
            
            // DataTables
            $('#tableObservaciones').DataTable({
                "language": {
                    "url": "//cdn.datatables.net/plug-ins/1.10.24/i18n/Spanish.json"
                },
                "pageLength": 50,
                "order": [[7, "asc"]]  // Ordenar por fecha vencimiento
            });
        });
        
        function descargarExcel() {
            var params = $('#formFiltros').serialize();
            window.location.href = '@Url.Action("DescargarReporteObservaciones")' + '?' + params;
        }
        
        function limpiarFiltros() {
            $('#formFiltros')[0].reset();
            $('#formFiltros').submit();
        }
    </script>
}
```

**Características avanzadas**:
- **Checkboxes múltiples**: Para filtros complejos
- **Datepicker**: Selector de fechas
- **Clases condicionales**: `table-danger`, `table-warning`
- **Contador de registros**: `.Count()`
- **Badges de riesgo**: Colores según nivel
- **Export a Excel**: Llamada a acción del controlador
- **DataTables avanzado**: Paginación, búsqueda, ordenamiento

---

## Vistas Parciales

Las vistas parciales (Partial Views) son componentes reutilizables que se incluyen en otras vistas. En TIGA se usan principalmente para **modals**.

### Convención de Nomenclatura

**Prefijo `PV_`** (Partial View):
- `PV_CrearEvaluacion.cshtml`
- `PV_EditarProyecto.cshtml`
- `PV_DetalleActividad.cshtml`

### Ejemplo: PV_CrearEvaluacion.cshtml

```cshtml
@model WebTIGA.Models.DPA_Evaluacion

<div class="modal-header">
    <h5 class="modal-title">Crear Nueva Evaluación</h5>
    <button type="button" class="close" data-dismiss="modal">
        <span>&times;</span>
    </button>
</div>

@using (Ajax.BeginForm("PV_CrearEvaluacion", "WebPlanAnual", 
        new AjaxOptions { 
            HttpMethod = "POST",
            OnSuccess = "onSuccessCrearEvaluacion",
            OnFailure = "onErrorCrearEvaluacion"
        }))
{
    @Html.AntiForgeryToken()
    
    <div class="modal-body">
        <div class="form-group">
            <label>Nombre de la Evaluación:</label>
            @Html.TextBoxFor(m => m.Nombre, new { @class = "form-control", @required = "required" })
            @Html.ValidationMessageFor(m => m.Nombre, "", new { @class = "text-danger" })
        </div>
        
        <div class="form-group">
            <label>Tipo de Evaluación:</label>
            @Html.DropDownListFor(m => m.Tipo_Evaluacion, 
                new SelectList(ViewBag.TiposEvaluacion), 
                "Seleccione...", 
                new { @class = "form-control" })
        </div>
        
        <div class="form-group">
            <label>Negocio:</label>
            @Html.DropDownListFor(m => m.ID_Negocio, 
                (SelectList)ViewBag.Negocios, 
                "Seleccione...", 
                new { @class = "form-control" })
        </div>
        
        <div class="form-group">
            <label>Fecha Inicio Estimada:</label>
            @Html.TextBoxFor(m => m.Fecha_Inicio_Estimada, 
                new { @class = "form-control datepicker", @type = "text" })
        </div>
        
        <div class="form-group">
            <label>Días Estimados:</label>
            @Html.TextBoxFor(m => m.Dias_Estimados, 
                new { @class = "form-control", @type = "number", @min = "1" })
        </div>
    </div>
    
    <div class="modal-footer">
        <button type="button" class="btn btn-secondary" data-dismiss="modal">
            Cancelar
        </button>
        <button type="submit" class="btn btn-primary">
            <i class="fas fa-save"></i> Guardar
        </button>
    </div>
}

<script>
    function onSuccessCrearEvaluacion(response) {
        if (response.success) {
            $('#modalCrearEvaluacion').modal('hide');
            toastr.success('Evaluación creada exitosamente');
            location.reload();
        } else {
            toastr.error(response.message);
        }
    }
    
    function onErrorCrearEvaluacion() {
        toastr.error('Error al crear la evaluación');
    }
</script>
```

**Uso en la vista principal**:
```cshtml
<!-- Botón que abre el modal -->
<button class="btn btn-success" data-toggle="modal" data-target="#modalCrearEvaluacion">
    <i class="fas fa-plus"></i> Nueva Evaluación
</button>

<!-- Modal Container -->
<div class="modal fade" id="modalCrearEvaluacion" tabindex="-1">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
            @Html.Partial("PV_CrearEvaluacion")
        </div>
    </div>
</div>
```

### Ajax.BeginForm vs Html.BeginForm

**Ajax.BeginForm**: Envío asíncrono (no recarga página)
```cshtml
@using (Ajax.BeginForm("Action", "Controller", 
        new AjaxOptions { 
            HttpMethod = "POST",
            OnSuccess = "functionSuccess",
            OnFailure = "functionError"
        }))
{
    // Formulario
}
```

**Html.BeginForm**: Envío tradicional (recarga página)
```cshtml
@using (Html.BeginForm("Action", "Controller", FormMethod.Post))
{
    @Html.AntiForgeryToken()
    // Formulario
}
```

---

## Helpers y Componentes

### 1. Html Helpers

#### TextBox y PasswordBox
```cshtml
@Html.TextBoxFor(m => m.Nombre, new { @class = "form-control", @placeholder = "Nombre" })
@Html.PasswordFor(m => m.Password, new { @class = "form-control" })
@Html.TextAreaFor(m => m.Descripcion, new { @class = "form-control", @rows = 5 })
```

#### DropDownList
```cshtml
@Html.DropDownListFor(m => m.CategoriaId, 
    (SelectList)ViewBag.Categorias, 
    "Seleccione una categoría", 
    new { @class = "form-control" })
```

#### CheckBox y RadioButton
```cshtml
@Html.CheckBoxFor(m => m.Activo, new { @class = "form-check-input" })
@Html.RadioButtonFor(m => m.Genero, "M", new { @class = "form-check-input" }) Masculino
@Html.RadioButtonFor(m => m.Genero, "F", new { @class = "form-check-input" }) Femenino
```

#### Hidden Fields
```cshtml
@Html.HiddenFor(m => m.Id)
```

#### Display y Label
```cshtml
@Html.LabelFor(m => m.Nombre, "Nombre Completo:", new { @class = "control-label" })
@Html.DisplayFor(m => m.FechaCreacion)
```

#### Validation
```cshtml
@Html.ValidationSummary(true, "", new { @class = "text-danger" })
@Html.ValidationMessageFor(m => m.Email, "", new { @class = "text-danger" })
```

### 2. Url Helpers

```cshtml
<!-- Action Link -->
<a href="@Url.Action("Edit", "Usuario", new { id = 5 })">Editar</a>

<!-- Content URL -->
<img src="@Url.Content("~/img/logo.png")" alt="Logo" />

<!-- JavaScript -->
var url = '@Url.Action("GetData", "Api")';
```

### 3. Ajax Helpers

Requiere `jquery.unobtrusive-ajax.js`.

```cshtml
@Ajax.ActionLink("Ver Detalle", "Detalle", new { id = Model.Id }, 
    new AjaxOptions { 
        UpdateTargetId = "divDetalle",
        InsertionMode = InsertionMode.Replace,
        HttpMethod = "GET"
    })
```

### 4. Secciones Personalizadas

**Definir sección en layout**:
```cshtml
@RenderSection("scripts", required: false)
@RenderSection("styles", required: false)
```

**Usar en vista**:
```cshtml
@section scripts {
    <script src="~/Scripts/custom.js"></script>
    <script>
        $(document).ready(function() {
            // Código específico de esta vista
        });
    </script>
}

@section styles {
    <link href="~/Content/custom.css" rel="stylesheet" />
}
```

---

## JavaScript y Estilos

### Organización de Scripts

**Bundle Config** (`App_Start/BundleConfig.cs`):
```csharp
bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
    "~/Scripts/jquery-{version}.js"));

bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
    "~/Scripts/bootstrap.js"));

bundles.Add(new ScriptBundle("~/bundles/custom").Include(
    "~/Scripts/custom.js",
    "~/Scripts/datatables.js"));
```

**Uso en vistas**:
```cshtml
@Scripts.Render("~/bundles/jquery")
@Scripts.Render("~/bundles/bootstrap")
@Scripts.Render("~/bundles/custom")
```

### Librerías JavaScript Comunes

1. **jQuery 3.3.1**: Manipulación DOM
2. **Bootstrap 4.0**: Framework CSS y componentes JS
3. **DataTables**: Tablas interactivas
4. **Toastr**: Notificaciones toast
5. **Select2**: Dropdowns avanzados
6. **Datepicker**: Selectores de fecha
7. **Chart.js**: Gráficos
8. **SweetAlert2**: Alertas personalizadas

### Ejemplo de Uso de DataTables

```html
<table id="miTabla" class="table table-bordered">
    <thead>
        <tr>
            <th>Columna 1</th>
            <th>Columna 2</th>
        </tr>
    </thead>
    <tbody>
        <!-- Datos -->
    </tbody>
</table>

<script>
    $(document).ready(function() {
        $('#miTabla').DataTable({
            "language": {
                "url": "//cdn.datatables.net/plug-ins/1.10.24/i18n/Spanish.json"
            },
            "pageLength": 25,
            "order": [[0, "asc"]],
            "responsive": true
        });
    });
</script>
```

### Ejemplo de Toastr

```javascript
// Success
toastr.success('Operación exitosa', 'Éxito');

// Error
toastr.error('Ocurrió un error', 'Error');

// Warning
toastr.warning('Advertencia', 'Atención');

// Info
toastr.info('Información importante', 'Info');
```

### Ejemplo de SweetAlert2

```javascript
Swal.fire({
    title: '¿Está seguro?',
    text: "Esta acción no se puede deshacer",
    icon: 'warning',
    showCancelButton: true,
    confirmButtonColor: '#3085d6',
    cancelButtonColor: '#d33',
    confirmButtonText: 'Sí, eliminar',
    cancelButtonText: 'Cancelar'
}).then((result) => {
    if (result.isConfirmed) {
        // Ejecutar eliminación
        $.post('@Url.Action("Delete")', { id: id }, function(response) {
            if (response.success) {
                Swal.fire('Eliminado', 'El registro ha sido eliminado', 'success');
            }
        });
    }
});
```

---

## Patrones y Convenciones

### 1. Estructura de Card

**Patrón Bootstrap Card**:
```cshtml
<div class="card shadow mb-4">
    <div class="card-header py-3">
        <h6 class="m-0 font-weight-bold text-primary">Título</h6>
    </div>
    <div class="card-body">
        <!-- Contenido -->
    </div>
</div>
```

### 2. Formularios Estándar

```cshtml
@using (Html.BeginForm("Action", "Controller", FormMethod.Post, 
        new { @class = "form-horizontal" }))
{
    @Html.AntiForgeryToken()
    
    <div class="form-group">
        @Html.LabelFor(m => m.Nombre, new { @class = "control-label col-md-2" })
        <div class="col-md-10">
            @Html.TextBoxFor(m => m.Nombre, new { @class = "form-control" })
            @Html.ValidationMessageFor(m => m.Nombre, "", new { @class = "text-danger" })
        </div>
    </div>
    
    <div class="form-group">
        <div class="col-md-offset-2 col-md-10">
            <button type="submit" class="btn btn-primary">Guardar</button>
            <a href="@Url.Action("Index")" class="btn btn-default">Cancelar</a>
        </div>
    </div>
}
```

### 3. Tablas Responsivas

```cshtml
<div class="table-responsive">
    <table class="table table-bordered table-hover">
        <thead class="thead-light">
            <tr>
                <th>Columna 1</th>
                <th>Columna 2</th>
                <th>Acciones</th>
            </tr>
        </thead>
        <tbody>
            @foreach (var item in Model)
            {
                <tr>
                    <td>@item.Propiedad1</td>
                    <td>@item.Propiedad2</td>
                    <td>
                        <a href="@Url.Action("Edit", new { id = item.Id })" 
                           class="btn btn-sm btn-info">
                            <i class="fas fa-edit"></i>
                        </a>
                        <a href="@Url.Action("Delete", new { id = item.Id })" 
                           class="btn btn-sm btn-danger">
                            <i class="fas fa-trash"></i>
                        </a>
                    </td>
                </tr>
            }
        </tbody>
    </table>
</div>
```

### 4. Badges y Estados

```cshtml
<span class="badge badge-success">Activo</span>
<span class="badge badge-danger">Inactivo</span>
<span class="badge badge-warning">Pendiente</span>
<span class="badge badge-info">En Proceso</span>
```

### 5. Botones con Iconos

```cshtml
<button class="btn btn-primary">
    <i class="fas fa-save"></i> Guardar
</button>

<button class="btn btn-success">
    <i class="fas fa-plus"></i> Crear Nuevo
</button>

<button class="btn btn-danger">
    <i class="fas fa-trash"></i> Eliminar
</button>

<a href="@Url.Action("Index")" class="btn btn-secondary">
    <i class="fas fa-arrow-left"></i> Regresar
</a>
```

### 6. Validación Condicional de Roles

```cshtml
@if (ViewBag.idRol == 1 || ViewBag.idRol == 2)
{
    <button class="btn btn-success">Solo para Administradores</button>
}
else
{
    <p class="text-muted">No tiene permisos para esta acción</p>
}
```

### 7. Formateo de Fechas

```cshtml
@Model.FechaCreacion?.ToString("dd/MM/yyyy")
@Model.FechaHora?.ToString("dd/MM/yyyy HH:mm:ss")
```

---

## Mejores Prácticas

### ✅ DO

1. **Usar modelos fuertemente tipados**: `@model MiModelo`
2. **Helpers de HTML**: En lugar de HTML plano
3. **Validación del lado del cliente**: jQuery Validate
4. **Secciones para scripts**: Evitar scripts inline en el body
5. **ViewBag solo para datos simples**: Modelos para datos complejos
6. **Parciales para código reutilizable**: Evitar duplicación
7. **Layouts por módulo**: Mantener consistencia
8. **AJAX para operaciones CRUD**: Mejor UX sin recargas

### ❌ DON'T

1. **No lógica de negocio en vistas**: Solo presentación
2. **No código C# extenso**: Mover a controlador o helper
3. **No inline styles**: Usar clases CSS
4. **No exponer datos sensibles**: En ViewSource o JavaScript
5. **No olvidar AntiForgeryToken**: En formularios POST
6. **No hardcodear URLs**: Usar `Url.Action()`
7. **No mezclar idiomas**: Español o inglés, no ambos

---

## Referencia Rápida

### Modelo
```cshtml
@model MiNamespace.MiModelo
```

### Iteración
```cshtml
@foreach (var item in Model.Coleccion)
{
    <p>@item.Propiedad</p>
}
```

### Condicional
```cshtml
@if (condicion)
{
    <p>Verdadero</p>
}
else
{
    <p>Falso</p>
}
```

### Razor Syntax
```cshtml
@* Comentario Razor *@
@{ var variable = "valor"; }
@Html.Helper()
@Url.Action("Action", "Controller")
```

---

**Nota**: Las vistas en TIGA priorizan la usabilidad, responsividad y consistencia visual. Bootstrap 4 es el framework base para todos los componentes de UI.
