# SETUP.md - Guía de Instalación y Configuración

## 📋 Índice
1. [Requisitos del Sistema](#requisitos-del-sistema)
2. [Instalación de Dependencias](#instalación-de-dependencias)
3. [Configuración de Base de Datos](#configuración-de-base-de-datos)
4. [Configuración de la Aplicación](#configuración-de-la-aplicación)
5. [Compilación y Ejecución](#compilación-y-ejecución)
6. [Despliegue en IIS](#despliegue-en-iis)
7. [Resolución de Problemas](#resolución-de-problemas)
8. [Mantenimiento](#mantenimiento)

---

## Requisitos del Sistema

### Software Requerido

#### Desarrollo
- **Visual Studio 2017 o superior**
  - Workload: ASP.NET and web development
  - Workload: .NET desktop development
  
- **.NET Framework 4.6.1 SDK**
  - Incluido en Visual Studio 2017+
  
- **SQL Server 2014 o superior**
  - Express, Developer o Enterprise Edition
  - SQL Server Management Studio (SSMS) recomendado
  
- **IIS 7.0 o superior** (para despliegue local)
  - Incluido en Windows 7/8/10/Server

#### Opcionales (Recomendados)
- **Git** para control de versiones
- **NuGet Package Manager** (incluido en Visual Studio)
- **Postman** para pruebas de API

### Requisitos de Hardware

#### Desarrollo
- **CPU**: Intel Core i5 o superior
- **RAM**: Mínimo 8 GB (16 GB recomendado)
- **Disco**: 10 GB libres mínimo
- **Resolución**: 1366x768 mínimo

#### Servidor de Producción
- **CPU**: 4 cores mínimo
- **RAM**: 16 GB mínimo
- **Disco**: 50 GB libres mínimo (SSD recomendado)
- **Red**: Conexión estable a base de datos

### Sistemas Operativos Soportados

#### Desarrollo
- Windows 10 Pro/Enterprise (64-bit)
- Windows 11 Pro/Enterprise (64-bit)

#### Servidor
- Windows Server 2016 o superior
- Windows Server 2019 (recomendado)

---

## Instalación de Dependencias

### 1. Clonar el Repositorio

```bash
# HTTPS
git clone https://github.com/AAAIIAAA/TIGA.git

# SSH
git clone git@github.com:AAAIIAAA/TIGA.git

# Navegar al directorio
cd TIGA
```

### 2. Abrir el Proyecto en Visual Studio

1. Abrir Visual Studio
2. File → Open → Project/Solution
3. Navegar a la carpeta del proyecto
4. Seleccionar `WebTIGA.csproj`
5. Click en "Open"

### 3. Restaurar Paquetes NuGet

**Método Automático**:
```
Click derecho en el proyecto → Restore NuGet Packages
```

**Método Manual**:
```
Tools → NuGet Package Manager → Package Manager Console
PM> Update-Package -reinstall
```

### 4. Verificar Dependencias Instaladas

Las siguientes dependencias se instalarán automáticamente desde `packages.config`:

#### Principales
- **Entity Framework 6.1.3**: ORM
- **Microsoft.AspNet.Mvc 5.2.4**: Framework MVC
- **EPPlus 5.8.2**: Procesamiento Excel
- **LinqToExcel 1.11.0**: Lectura de Excel
- **DocumentFormat.OpenXml 2.15.0**: Documentos Office
- **Newtonsoft.Json 11.0**: Serialización JSON
- **jQuery 3.3.1**: JavaScript framework
- **Bootstrap 3.3.7**: Framework CSS

#### Adicionales
- Microsoft.AspNetCore.* (paquetes varios)
- jQuery.Validation 1.17.0
- Microsoft.CodeDom.Providers.DotNetCompilerPlatform 2.0.0

### 5. Verificar Instalación

En Package Manager Console:
```powershell
PM> Get-Package

# Debe mostrar lista completa de paquetes instalados
```

---

## Configuración de Base de Datos

### 1. Crear Base de Datos

**Opción A: Desde Script SQL**

```sql
-- En SQL Server Management Studio
-- Abrir y ejecutar el script de creación de BD
-- (ubicado en /Database/Scripts/CreateDatabase.sql)

CREATE DATABASE PROYECTOSIAV2
GO

USE PROYECTOSIAV2
GO

-- Ejecutar scripts de tablas, SPs, funciones, etc.
```

**Opción B: Restaurar Backup**

```sql
-- Si existe un backup (.bak)
RESTORE DATABASE PROYECTOSIAV2
FROM DISK = 'C:\Backups\PROYECTOSIAV2.bak'
WITH MOVE 'PROYECTOSIAV2' TO 'C:\SQLData\PROYECTOSIAV2.mdf',
     MOVE 'PROYECTOSIAV2_log' TO 'C:\SQLData\PROYECTOSIAV2_log.ldf',
     REPLACE
GO
```

### 2. Crear Usuario de Aplicación

```sql
-- Crear login SQL
CREATE LOGIN TIGAUser WITH PASSWORD = 'Password123!';
GO

-- Crear usuario en la base de datos
USE PROYECTOSIAV2;
GO

CREATE USER TIGAUser FOR LOGIN TIGAUser;
GO

-- Asignar permisos
ALTER ROLE db_datareader ADD MEMBER TIGAUser;
ALTER ROLE db_datawriter ADD MEMBER TIGAUser;
ALTER ROLE db_executor ADD MEMBER TIGAUser;
GO
```

### 3. Verificar Stored Procedures

Ejecutar script de validación:

```sql
-- Verificar que existen los SPs principales
SELECT name, type_desc 
FROM sys.objects 
WHERE type IN ('P', 'FN', 'TF')
ORDER BY type_desc, name;

-- Debe incluir:
-- SP_DPA_Mantenimiento_Plan
-- SP_MODULOS_USUARIOS
-- fn_TG_Obtener_Proyectos_v2
-- etc.
```

### 4. Poblar Datos Iniciales

```sql
-- Ejecutar scripts de datos maestros
-- (usuarios, roles, módulos, etc.)

-- Ejemplo: Usuario administrador inicial
INSERT INTO Persona (Nombres, Apellidos, Email, Activo)
VALUES ('Admin', 'Sistema', 'admin@pacifico.com.pe', 1);

INSERT INTO Usuario (IdPersona, Usuario1, Password, Estado)
VALUES (1, 'admin', 'HASH_PASSWORD', 1);

-- Insertar módulos
INSERT INTO Modulo (IdModulo, Nombre, Descripcion, Icono, Enlace, Activo)
VALUES 
(1, 'Home', 'Dashboard Principal', 'fa-home', '/Home/Index', 1),
(12, 'Plan Anual', 'Gestión Plan Anual', 'fa-tasks', '/WebPlanAnual/Inicio', 1),
(14, 'Seguimiento', 'Seguimiento Observaciones', 'fa-clipboard-check', '/WebSeguimientoObservaciones/Inicio', 1);

-- Más inserts...
```

---

## Configuración de la Aplicación

### 1. Configurar Connection String

Editar `Web.config`:

```xml
<connectionStrings>
    <!-- Connection String Principal -->
    <add name="PROYECTOSIAV2Entities1" 
         connectionString="metadata=res://*/Models.ContenedorModelos.csdl|res://*/Models.ContenedorModelos.ssdl|res://*/Models.ContenedorModelos.msl;
         provider=System.Data.SqlClient;
         provider connection string=&quot;
         data source=NOMBRE_SERVIDOR;
         initial catalog=PROYECTOSIAV2;
         user id=TIGAUser;
         password=Password123!;
         MultipleActiveResultSets=True;
         App=EntityFramework&quot;" 
         providerName="System.Data.EntityClient" />
    
    <!-- Connection String para TeamMate (si aplica) -->
    <add name="TeamMateR12Entities" 
         connectionString="..." 
         providerName="System.Data.EntityClient" />
</connectionStrings>
```

**Parámetros importantes**:
- `data source`: Nombre del servidor SQL (ej: `localhost\SQLEXPRESS`)
- `initial catalog`: Nombre de la base de datos
- `user id`: Usuario de SQL Server
- `password`: Contraseña del usuario

**Para Windows Authentication**:
```xml
data source=SERVIDOR;
initial catalog=PROYECTOSIAV2;
integrated security=True;
MultipleActiveResultSets=True;
```

### 2. Configurar AppSettings

En `Web.config`:

```xml
<appSettings>
    <add key="webpages:Version" value="3.0.0.0" />
    <add key="webpages:Enabled" value="false" />
    <add key="ClientValidationEnabled" value="true" />
    <add key="UnobtrusiveJavaScriptEnabled" value="true" />
    
    <!-- Configuraciones personalizadas -->
    <add key="UrlDomain" value="http://localhost:PORT/" />
    <add key="EmailFrom" value="notificaciones@pacifico.com.pe" />
    <add key="EmailHost" value="smtp.office365.com" />
    <add key="EmailPort" value="587" />
    <add key="EmailUser" value="usuario@pacifico.com.pe" />
    <add key="EmailPassword" value="PASSWORD_CIFRADO" />
    
    <!-- Configuración de sesión -->
    <add key="SessionTimeout" value="60" />
</appSettings>
```

### 3. Configurar Compilación

En `Web.config`:

```xml
<system.web>
    <compilation debug="true" targetFramework="4.6.1" />
    <httpRuntime targetFramework="4.6.1" maxRequestLength="52428800" />
    <customErrors mode="Off" />
    
    <!-- Configuración de sesión -->
    <sessionState mode="InProc" timeout="60" />
    
    <!-- Autenticación -->
    <authentication mode="Forms">
        <forms loginUrl="~/Login/Login_" timeout="60" />
    </authentication>
</system.web>
```

**Parámetros clave**:
- `debug="true"`: Solo para desarrollo (cambiar a `false` en producción)
- `maxRequestLength`: Tamaño máximo de archivos (50 MB en este caso)
- `customErrors mode="Off"`: Mostrar errores detallados (cambiar en producción)
- `timeout="60"`: Timeout de sesión en minutos

### 4. Configurar IIS Express (Desarrollo)

Editar `.vs/config/applicationhost.config` o usar GUI de Visual Studio:

**Project Properties**:
1. Click derecho en proyecto → Properties
2. Web tab
3. Servers: IIS Express
4. Project Url: `http://localhost:PORT`
5. Override application root URL: (opcional)

**Puerto personalizado**:
```xml
<!-- En applicationhost.config -->
<site name="WebTIGA" id="1">
    <application path="/" applicationPool="Clr4IntegratedAppPool">
        <virtualDirectory path="/" physicalPath="C:\Path\To\WebTIGA" />
    </application>
    <bindings>
        <binding protocol="http" bindingInformation="*:5555:localhost" />
    </bindings>
</site>
```

### 5. Configurar SMTP (Opcional)

Para recuperación de contraseñas:

```xml
<system.net>
    <mailSettings>
        <smtp from="notificaciones@pacifico.com.pe">
            <network host="smtp.office365.com"
                     port="587"
                     userName="usuario@pacifico.com.pe"
                     password="PASSWORD"
                     enableSsl="true" />
        </smtp>
    </mailSettings>
</system.net>
```

---

## Compilación y Ejecución

### 1. Compilar el Proyecto

**Método 1: Visual Studio**
```
Build → Build Solution (Ctrl + Shift + B)
```

**Método 2: MSBuild (Command Line)**
```cmd
cd C:\Path\To\WebTIGA
"C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe" WebTIGA.csproj /p:Configuration=Release
```

### 2. Verificar Compilación

Verificar carpeta `bin/`:
```
bin/
├── WebTIGA.dll
├── WebTIGA.pdb
├── EntityFramework.dll
├── Newtonsoft.Json.dll
└── (otras dependencias)
```

### 3. Ejecutar en Desarrollo

**Método 1: Visual Studio**
```
Debug → Start Debugging (F5)
# o
Debug → Start Without Debugging (Ctrl + F5)
```

**Método 2: IIS Express**
```cmd
cd "C:\Program Files (x86)\IIS Express"
iisexpress.exe /path:"C:\Path\To\WebTIGA" /port:5555
```

### 4. Verificar Ejecución

Abrir navegador y acceder a:
```
http://localhost:PORT/Login/Login_
```

Debe mostrar la página de login.

### 5. Login de Prueba

Usuario inicial (si se configuró):
```
Usuario: admin
Contraseña: (definida en la BD)
```

---

## Despliegue en IIS

### 1. Preparar Publicación

**En Visual Studio**:
```
1. Click derecho en proyecto → Publish
2. Target: Folder
3. Folder location: C:\Publish\WebTIGA
4. Configuration: Release
5. Click "Publish"
```

### 2. Configurar IIS

**Habilitar IIS (si no está instalado)**:
```
Control Panel → Programs → Turn Windows features on or off
Marcar:
☑ Internet Information Services
  ☑ Web Management Tools
    ☑ IIS Management Console
  ☑ World Wide Web Services
    ☑ Application Development Features
      ☑ ASP.NET 4.6
      ☑ .NET Extensibility 4.6
```

### 3. Crear Application Pool

**IIS Manager**:
```
1. Abrir IIS Manager (inetmgr)
2. Application Pools → Add Application Pool
   - Name: WebTIGA_AppPool
   - .NET CLR version: v4.0
   - Managed pipeline mode: Integrated
3. Configurar Advanced Settings:
   - Identity: ApplicationPoolIdentity (o cuenta específica)
   - Start Mode: AlwaysRunning (opcional)
```

### 4. Crear Sitio Web

**IIS Manager**:
```
1. Sites → Add Website
   - Site name: WebTIGA
   - Application pool: WebTIGA_AppPool
   - Physical path: C:\Publish\WebTIGA
   - Binding:
     * Type: http
     * IP address: All Unassigned
     * Port: 80 (o el deseado)
     * Host name: tiga.pacifico.local (opcional)
2. Click OK
```

### 5. Configurar Permisos

**Dar permisos a IIS**:
```cmd
icacls "C:\Publish\WebTIGA" /grant "IIS_IUSRS:(OI)(CI)F" /T
icacls "C:\Publish\WebTIGA" /grant "IIS AppPool\WebTIGA_AppPool:(OI)(CI)F" /T
```

### 6. Configurar Web.config para Producción

**Transformaciones en Web.Release.config**:
```xml
<configuration xmlns:xdt="http://schemas.microsoft.com/XML-Document-Transform">
    
    <!-- Deshabilitar debug -->
    <system.web>
        <compilation xdt:Transform="RemoveAttributes(debug)" />
        <customErrors mode="On" xdt:Transform="Replace">
            <error statusCode="404" redirect="~/Error/NotFound" />
            <error statusCode="500" redirect="~/Error/ServerError" />
        </customErrors>
    </system.web>
    
    <!-- Connection String de producción -->
    <connectionStrings>
        <add name="PROYECTOSIAV2Entities1"
             connectionString="..."
             xdt:Transform="SetAttributes"
             xdt:Locator="Match(name)" />
    </connectionStrings>
    
    <!-- Configurar errores detallados OFF -->
    <system.webServer>
        <httpErrors errorMode="Custom" xdt:Transform="Replace" />
    </system.webServer>
</configuration>
```

### 7. SSL/HTTPS (Opcional pero Recomendado)

**Instalar certificado**:
```
1. IIS Manager → Server Certificates
2. Create Self-Signed Certificate (desarrollo) o importar certificado
3. En el sitio → Bindings → Add
   - Type: https
   - Port: 443
   - SSL certificate: Seleccionar certificado
```

**Forzar HTTPS en Web.config**:
```xml
<system.webServer>
    <rewrite>
        <rules>
            <rule name="Redirect to HTTPS" stopProcessing="true">
                <match url="(.*)" />
                <conditions>
                    <add input="{HTTPS}" pattern="^OFF$" />
                </conditions>
                <action type="Redirect" url="https://{HTTP_HOST}/{R:1}" redirectType="Permanent" />
            </rule>
        </rules>
    </rewrite>
</system.webServer>
```

### 8. Verificar Despliegue

```
1. Abrir navegador
2. Navegar a: http://servidor/Login/Login_
3. Verificar que carga correctamente
4. Probar login
5. Verificar acceso a módulos
```

---

## Resolución de Problemas

### Error: "Could not load file or assembly"

**Solución**:
```
1. Limpiar solución: Build → Clean Solution
2. Restaurar paquetes: Update-Package -reinstall
3. Rebuild: Build → Rebuild Solution
```

### Error: "A network-related or instance-specific error"

**Causa**: No se puede conectar a SQL Server

**Solución**:
```
1. Verificar que SQL Server está ejecutándose
2. Verificar firewall (puerto 1433)
3. Verificar SQL Server Configuration Manager:
   - TCP/IP habilitado
   - Named Pipes habilitado
4. Verificar connection string en Web.config
5. Probar conexión con SSMS
```

### Error: "Login failed for user"

**Solución**:
```sql
-- Verificar usuario existe
USE master;
SELECT name FROM sys.server_principals WHERE name = 'TIGAUser';

-- Resetear permisos
USE PROYECTOSIAV2;
ALTER ROLE db_owner ADD MEMBER TIGAUser;
```

### Error 403: Forbidden (IIS)

**Solución**:
```cmd
# Verificar permisos
icacls "C:\Publish\WebTIGA"

# Reestablecer permisos
icacls "C:\Publish\WebTIGA" /reset /T
icacls "C:\Publish\WebTIGA" /grant "IIS_IUSRS:(OI)(CI)F" /T
```

### Error 500: Internal Server Error

**Solución**:
```xml
<!-- Habilitar errores detallados en Web.config -->
<system.web>
    <customErrors mode="Off" />
</system.web>

<system.webServer>
    <httpErrors errorMode="Detailed" />
</system.webServer>
```

### Session State Error

**Solución**:
```xml
<!-- En Web.config -->
<system.web>
    <sessionState mode="InProc" timeout="60" />
</system.web>
```

### EPPlus License Error

**Solución**:
```csharp
// Agregar al inicio de métodos que usan EPPlus
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
```

---

## Mantenimiento

### 1. Backup de Base de Datos

**SQL Server**:
```sql
-- Backup completo
BACKUP DATABASE PROYECTOSIAV2
TO DISK = 'C:\Backups\PROYECTOSIAV2_Full.bak'
WITH FORMAT, INIT, NAME = 'Full Backup';

-- Backup programado (crear Job en SQL Agent)
```

### 2. Actualización de Dependencias

**Package Manager Console**:
```powershell
# Ver paquetes desactualizados
PM> Get-Package -Updates

# Actualizar todos
PM> Update-Package

# Actualizar uno específico
PM> Update-Package EntityFramework
```

### 3. Logs de Aplicación

**Configurar NLog o Log4Net** (recomendado):

```xml
<!-- Web.config -->
<configSections>
    <section name="nlog" type="NLog.Config.ConfigSectionHandler, NLog"/>
</configSections>

<nlog>
    <targets>
        <target name="file" 
                xsi:type="File" 
                fileName="${basedir}/logs/app-${shortdate}.log"
                layout="${longdate} ${level} ${message} ${exception}" />
    </targets>
    <rules>
        <logger name="*" minlevel="Info" writeTo="file" />
    </rules>
</nlog>
```

### 4. Monitoreo de Performance

**IIS Logs**:
```
Ubicación: C:\inetpub\logs\LogFiles\W3SVC1\
```

**SQL Server Profiler**:
```
Monitorear queries lentas y deadlocks
```

### 5. Actualización de Código

```bash
# Obtener cambios del repositorio
git pull origin main

# Restaurar paquetes
Update-Package -reinstall

# Rebuild
Build → Rebuild Solution

# Publicar
Publish → Deploy
```

---

## Checklist de Instalación

### Desarrollo
- [ ] Visual Studio instalado
- [ ] .NET Framework 4.6.1 SDK
- [ ] SQL Server instalado
- [ ] Base de datos creada
- [ ] Connection string configurado
- [ ] Paquetes NuGet restaurados
- [ ] Proyecto compila sin errores
- [ ] Aplicación ejecuta localmente
- [ ] Login funciona correctamente

### Producción
- [ ] IIS instalado y configurado
- [ ] Application Pool creado
- [ ] Sitio web creado en IIS
- [ ] Permisos de carpeta configurados
- [ ] Web.config de producción configurado
- [ ] Connection string de producción
- [ ] Certificado SSL instalado (opcional)
- [ ] Backup de BD programado
- [ ] Logs habilitados
- [ ] Monitoreo configurado

---

## Recursos Adicionales

### Documentación
- [ASP.NET MVC 5](https://docs.microsoft.com/en-us/aspnet/mvc/mvc5)
- [Entity Framework 6](https://docs.microsoft.com/en-us/ef/ef6/)
- [IIS Configuration](https://docs.microsoft.com/en-us/iis/)

### Soporte
- Email: soporteauditoria@pacifico.com.pe
- Repositorio: https://github.com/AAAIIAAA/TIGA

---

**Última actualización**: Diciembre 2025  
**Versión de documentación**: 1.0
