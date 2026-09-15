# Profit TM

Aplicacion web empresarial para la gestion de operaciones administrativas y
reportes sobre bases de datos Profit. El proyecto esta construido con
ASP.NET MVC 5 sobre .NET Framework y combina vistas Razor, Web API, Entity
Framework 6 y componentes DevExpress.

## Funcionalidades principales

- Autenticacion basada en Forms Authentication.
- Seleccion de empresa y contexto de conexion para trabajar con distintas
	bases de datos administrativas.
- Modulos organizados por areas:
	- Caja y bancos.
	- Compras.
	- Inventario.
	- Ventas.
	- Fiscal.
	- General.
- Reportes DevExpress con vista previa y exportacion.
- API HTTP bajo la ruta `api/{controller}/{id}`.
- Procesamiento asincrono de documentos pendientes mediante Quartz.NET.
- Registro de incidentes y trazas de procesamiento.

## Tecnologia

| Componente | Version o detalle |
| --- | --- |
| .NET Framework | 4.7.2 |
| ASP.NET MVC | 5.2.x |
| ASP.NET Web API | 5.2.x |
| Entity Framework | 6.2.0 |
| DevExpress MVC y XtraReports | 18.2.6 |
| Quartz.NET | 3.8.1 |
| Newtonsoft.Json | 13.0.3 |
| Base de datos | Microsoft SQL Server |
| IDE recomendado | Visual Studio 2019 o posterior |

## Requisitos previos

1. Windows con Visual Studio y la carga de trabajo **ASP.NET y desarrollo
	 web**.
2. Developer Pack o targeting pack de **.NET Framework 4.7.2**.
3. Una instancia de SQL Server con las bases de datos requeridas por el
	 ambiente.
4. Acceso a los ensamblados de DevExpress 18.2.6. El proyecto referencia
	 componentes con licencia de DevExpress; tener solamente el proyecto no
	 garantiza que esos ensamblados puedan redistribuirse.
5. Permisos para leer y escribir los archivos de logs y recursos que utilice
	 el ambiente de ejecucion.

## Puesta en marcha local

### 1. Abrir la solucion

Abra `ProfitTM.sln` en Visual Studio y seleccione `ProfitTM` como proyecto de
inicio. El proyecto esta configurado para ejecutarse con IIS Express.

### 2. Configurar las conexiones

Las conexiones se declaran en `ProfitTM/Web.config`, dentro de
`<connectionStrings>`:

- `ProfitTMEntities`: base de datos propia de la aplicacion.
- `ProfitAdmEntities`: base de datos administrativa de Profit.
- `DemoAdmin`, `DemoCont` y `DemoNomi`: conexiones auxiliares de demostracion.

Reemplace servidor, base de datos y credenciales por valores del entorno
local. No agregue contrasenas reales al repositorio. Para desarrollo, use un
archivo de configuracion local no versionado o el mecanismo de secretos de su
entorno. Antes de iniciar la aplicacion, compruebe que la cuenta configurada
tenga acceso a todas las bases de datos necesarias.

### 3. Restaurar y compilar

Desde Visual Studio:

1. Restaure los paquetes NuGet si Visual Studio lo solicita.
2. Ejecute **Build > Rebuild Solution**.
3. Confirme que los ensamblados de DevExpress requeridos esten disponibles.

Tambien puede compilar la solucion desde una consola de desarrollador de
Visual Studio:

```powershell
msbuild .\ProfitTM.sln /t:Restore,Build /p:Configuration=Debug
```

Si la restauracion no esta habilitada para la instalacion de MSBuild, restaure
los paquetes desde Visual Studio y vuelva a ejecutar la compilacion.

### 4. Ejecutar

Presione `F5` para depurar con IIS Express o `Ctrl+F5` para ejecutar sin el
depurador. La ruta inicial es:

```text
https://localhost:<puerto>/Home/Index
```

El puerto se determina desde la configuracion de IIS Express de la solucion.

## Flujo de inicio de la aplicacion

Durante `Application_Start`, la aplicacion:

1. Registra areas, rutas MVC, filtros globales y rutas de Web API.
2. Configura el model binder de DevExpress.
3. Inicializa el visor de documentos y los reportes DevExpress.
4. Inicia un scheduler Quartz con el job `EnvioDocumentos`.

`EnvioDocumentos` se ejecuta cada 3 minutos y procesa lotes de hasta 200
registros pendientes. Reintenta el envio de documentos, actualiza el numero
de control recibido y deja trazas de los estados intermedios. Al detenerse la
aplicacion, el scheduler se apaga desde `Application_End`.

## Estructura del proyecto

```text
ProfitTM.sln
README.md
packages/                  Paquetes NuGet locales
ProfitTM/
	Areas/                   Modulos funcionales y sus vistas
	App_Start/               Registro de rutas, filtros y Web API
	Controllers/             Controladores MVC principales
	Models/                  Entidades y contextos Entity Framework
	Reports/                 Definiciones de reportes DevExpress
	Scripts/                 JavaScript del cliente
	Views/                   Vistas Razor compartidas y principales
	Global.asax.cs           Ciclo de vida y scheduler Quartz
	Web.config               Conexiones y configuracion de la aplicacion
```

Los modelos `ProfitTMModel` y `ProfitAdmModel` son modelos Database First
generados desde Entity Framework. Si cambia el esquema de la base de datos,
actualice el modelo `.edmx` desde Visual Studio y revise los archivos
generados antes de compilar.

## Rutas y API

MVC usa la ruta convencional:

```text
{controller}/{action}/{id}
```

La ruta por defecto apunta a `Home/Index`. Web API se publica bajo el prefijo
`api` y admite rutas por atributos y la ruta convencional:

```text
api/{controller}/{id}
```

Las areas contienen sus propias vistas, controladores y reportes. Para agregar
un reporte, mantenga la pareja de acciones de vista parcial y exportacion que
usan los controladores existentes y configure su fuente de datos con la
conexion de la empresa seleccionada.

## Configuracion por ambiente

Antes de publicar:

- Use conexiones y credenciales administradas fuera del control de versiones.
- Desactive `debug` en el `Web.config` de produccion.
- Revise las transformaciones `Web.Debug.config` y `Web.Release.config`.
- Configure correctamente HTTPS, el certificado del servidor y los permisos
	del Application Pool.
- Verifique la disponibilidad de SQL Server y la conectividad con los
	servicios externos usados para el envio de documentos.
- Defina una estrategia de respaldo para la base de datos de la aplicacion y
	para los registros de incidentes.

## Consideraciones de seguridad

- Nunca publique credenciales de SQL Server ni conexiones reales en Git.
- Cambie inmediatamente cualquier credencial que haya sido incluida por
	accidente en una configuracion compartida.
- Revise `Global.asax.cs` antes de desplegar: el proyecto configura una
	validacion de certificados TLS permisiva para las llamadas salientes. En
	produccion debe reemplazarse por validacion normal de certificados y una
	cadena de confianza correctamente configurada.
- Proteja los endpoints y reportes que requieran autenticacion; no asuma que
	una vista parcial es publica porque no tenga un atributo `[Authorize]`.
- Evite registrar contrasenas, tokens o cadenas de conexion completas en los
	incidentes y trazas.

## Diagnostico rapido

| Sintoma | Revision sugerida |
| --- | --- |
| Error al iniciar por una DLL faltante | Restaurar paquetes y verificar DevExpress 18.2.6 |
| Error de Entity Framework | Revisar `ProfitTMEntities`, `ProfitAdmEntities` y el acceso a SQL Server |
| Login redirige continuamente | Revisar Forms Authentication, cookies y la sesion de IIS Express |
| Reporte sin datos | Validar `Session["CONNECT"]`, la empresa seleccionada y los permisos SQL |
| Documentos pendientes no avanzan | Revisar el job Quartz, conectividad externa e incidentes registrados |

## Estado del proyecto

Este repositorio contiene una aplicacion ASP.NET MVC 5 existente y no un
proyecto ASP.NET Core. Por ello, el ciclo de desarrollo depende de Visual
Studio, IIS Express, .NET Framework y los ensamblados de terceros configurados
en la solucion.
