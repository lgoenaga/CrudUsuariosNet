# 04 - Comandos de .NET CLI

## 🎯 ¿Qué es .NET CLI?

**.NET CLI** (Command Line Interface) es una herramienta de línea de comandos que permite:
- Crear proyectos
- Compilar código
- Ejecutar aplicaciones
- Gestionar paquetes NuGet
- Publicar aplicaciones

**Ventaja**: Es multiplataforma (Windows, Linux, macOS) y no requiere Visual Studio.

---

## 📋 Comandos Básicos

### 1. `dotnet --version`

#### Descripción:
Muestra la versión del SDK de .NET instalado en tu sistema.

#### Cuándo utilizarlo:
- Para verificar que .NET está instalado correctamente
- Para saber qué versión tienes instalada
- Para verificar compatibilidad con un proyecto

#### Ejemplo:
```bash
dotnet --version
```

#### Resultado esperado:
```
8.0.128
```

**Interpretación:**
- Versión **8.0**: .NET 8 (versión mayor)
- **128**: Número de compilación específico

---

### 2. `dotnet restore`

#### Descripción:
Restaura todas las dependencias y herramientas de un proyecto especificadas en el archivo `.csproj`.

#### Cuándo utilizarlo:
- Después de clonar un repositorio
- Después de agregar un nuevo paquete NuGet al `.csproj`
- Cuando hay problemas con referencias de paquetes
- Antes de `dotnet build` (aunque `build` lo hace automáticamente)

#### Ejemplo:
```bash
cd BackEnd/ApiCrudUsuarios
dotnet restore
```

#### Resultado esperado:
```
  Determining projects to restore...
  Restored /home/usuario/ApiCrudUsuarios/ApiCrudUsuarios.csproj (in 2.5 sec).
```

**¿Qué hace internamente?**
- Lee `ApiCrudUsuarios.csproj`
- Descarga paquetes de NuGet.org
- Guarda los paquetes en caché local (`~/.nuget/packages/`)
- Crea el archivo `obj/project.assets.json`

#### Archivo relacionado:
```xml
<!-- ApiCrudUsuarios.csproj -->
<ItemGroup>
    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.26" />
    <PackageReference Include="Swashbuckle.AspNetCore" Version="6.6.2" />
</ItemGroup>
```

---

### 3. `dotnet build`

#### Descripción:
Compila el proyecto y todas sus dependencias.

#### Cuándo utilizarlo:
- Para verificar que el código compila sin errores
- Antes de ejecutar pruebas
- Para generar los archivos binarios (.dll)
- En procesos de CI/CD

#### Ejemplo:
```bash
cd BackEnd/ApiCrudUsuarios
dotnet build
```

#### Resultado esperado:
```
Build succeeded.
    0 Warning(s)
    0 Error(s)

Time Elapsed 00:00:05.32
```

**¿Qué hace?**
- Ejecuta `dotnet restore` automáticamente
- Compila archivos `.cs` a archivos `.dll`
- Genera archivos en `bin/Debug/net8.0/`

#### Archivos generados:
```
bin/Debug/net8.0/
├── ApiCrudUsuarios.dll          # Binario compilado
├── ApiCrudUsuarios.pdb          # Símbolos de depuración
├── ApiCrudUsuarios.deps.json    # Dependencias
└── ApiCrudUsuarios.runtimeconfig.json  # Configuración de runtime
```

#### Opciones útiles:
```bash
# Compilar en modo Release (optimizado)
dotnet build --configuration Release

# Compilar sin restaurar dependencias
dotnet build --no-restore

# Compilar con más información de diagnóstico
dotnet build --verbosity detailed
```

---

### 4. `dotnet run`

#### Descripción:
Compila (si es necesario) y ejecuta el proyecto.

#### Cuándo utilizarlo:
- Para ejecutar la aplicación en modo desarrollo
- Para probar cambios rápidamente
- Durante el desarrollo diario

#### Ejemplo - BackEnd:
```bash
cd BackEnd/ApiCrudUsuarios
dotnet run
```

#### Resultado esperado:
```
Building...
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:7068
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5033
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
```

**¿Qué hace?**
1. Ejecuta `dotnet build` si el código cambió
2. Ejecuta el archivo `.dll` generado
3. Inicia la aplicación
4. Queda en ejecución hasta que presiones `Ctrl+C`

#### Ejemplo - FrontEnd:
```bash
cd FrontEnd/BlazorCrudUsuarios
dotnet run
```

#### Resultado esperado:
```
Building...
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:7025
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5191
```

#### Opciones útiles:
```bash
# Ejecutar sin compilar (asume que ya compilaste)
dotnet run --no-build

# Ejecutar en modo Release
dotnet run --configuration Release

# Pasar argumentos al programa
dotnet run -- --argumento valor

# Especificar proyecto si hay varios en la carpeta
dotnet run --project ApiCrudUsuarios.csproj
```

---

### 5. `dotnet watch`

#### Descripción:
Ejecuta el proyecto y lo **reinicia automáticamente** cuando detecta cambios en los archivos.

#### Cuándo utilizarlo:
- Durante el desarrollo
- Para evitar reiniciar manualmente la aplicación cada vez que cambias el código
- Para desarrollo con recarga en caliente (hot reload)

#### Ejemplo:
```bash
cd BackEnd/ApiCrudUsuarios
dotnet watch
```

#### Resultado esperado:
```
dotnet watch 🔥 Hot reload enabled. For a list of supported edits, see https://aka.ms/dotnet/hot-reload.
  💡 Press "Ctrl + R" to restart.
dotnet watch 🔧 Building...
  Determining projects to restore...
  All projects are up-to-date for restore.
  ApiCrudUsuarios -> /path/to/bin/Debug/net8.0/ApiCrudUsuarios.dll
dotnet watch 🚀 Started
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:7068
```

**¿Qué hace?**
- Compila y ejecuta el proyecto
- Monitorea cambios en archivos `.cs`, `.razor`, `.cshtml`, etc.
- Al detectar cambios:
  - **Hot Reload**: Aplica cambios sin reiniciar (cuando es posible)
  - **Restart**: Reinicia la aplicación si hot reload no es posible

#### Ejemplo de uso:
1. Ejecutas `dotnet watch`
2. La aplicación inicia en `https://localhost:7068`
3. Modificas `AuthController.cs`
4. Guardas el archivo
5. La aplicación se recarga automáticamente
6. Los cambios están disponibles inmediatamente

#### Comandos interactivos:
Mientras `dotnet watch` está ejecutándose:
- `Ctrl + R`: Forzar reinicio manual
- `Ctrl + C`: Detener la aplicación

---

### 6. `dotnet clean`

#### Descripción:
Elimina los archivos generados por compilaciones anteriores.

#### Cuándo utilizarlo:
- Cuando hay problemas de compilación extraños
- Para empezar con un estado limpio
- Para liberar espacio en disco
- Antes de hacer un commit o push a Git (opcional)

#### Ejemplo:
```bash
cd BackEnd/ApiCrudUsuarios
dotnet clean
```

#### Resultado esperado:
```
Build succeeded.
    0 Warning(s)
    0 Error(s)

Time Elapsed 00:00:01.23
```

**¿Qué elimina?**
- Carpeta `bin/`
- Carpeta `obj/`
- Archivos `.dll`, `.pdb`, etc.

**¿Qué NO elimina?**
- Código fuente
- Archivos de configuración
- Paquetes NuGet descargados (están en caché global)

#### Uso típico:
```bash
# Limpiar y luego compilar desde cero
dotnet clean
dotnet build
```

---

### 7. `dotnet publish`

#### Descripción:
Prepara la aplicación para ser desplegada en producción.

#### Cuándo utilizarlo:
- Para desplegar en un servidor
- Para crear un paquete distribuible
- Para generar ejecutables independientes

#### Ejemplo básico:
```bash
cd BackEnd/ApiCrudUsuarios
dotnet publish -c Release -o ./publish
```

#### Resultado esperado:
```
Microsoft (R) Build Engine version 17.8.0+...
Build succeeded.

ApiCrudUsuarios -> /path/to/bin/Release/net8.0/ApiCrudUsuarios.dll
ApiCrudUsuarios -> /path/to/publish/
```

**¿Qué hace?**
- Compila el proyecto en modo Release (optimizado)
- Copia todos los archivos necesarios a la carpeta de salida
- Incluye dependencias necesarias
- Optimiza el tamaño y rendimiento

#### Opciones importantes:

##### Publicar en carpeta específica:
```bash
dotnet publish -o /var/www/miapp
```

##### Publicar para Linux:
```bash
dotnet publish -r linux-x64 --self-contained false
```

##### Publicar como ejecutable independiente:
```bash
dotnet publish -r win-x64 --self-contained true -p:PublishSingleFile=true
```

**Explicación de parámetros:**
- `-c Release`: Modo Release (optimizado)
- `-o`: Output directory (carpeta de salida)
- `-r`: Runtime identifier (sistema operativo y arquitectura)
- `--self-contained true`: Incluye el runtime de .NET (no requiere .NET instalado)
- `--self-contained false`: Requiere .NET instalado en el servidor
- `-p:PublishSingleFile=true`: Genera un único ejecutable

#### Contenido de la carpeta publish:
```
publish/
├── ApiCrudUsuarios.dll
├── ApiCrudUsuarios.pdb
├── appsettings.json
├── appsettings.Production.json
├── web.config (para IIS)
└── dependencias...
```

#### Ejecutar aplicación publicada:
```bash
cd publish
dotnet ApiCrudUsuarios.dll
```

---

### 8. `dotnet add package`

#### Descripción:
Agrega un paquete NuGet al proyecto.

#### Cuándo utilizarlo:
- Para instalar librerías de terceros
- Para agregar frameworks y herramientas
- Durante el desarrollo cuando necesitas funcionalidad adicional

#### Ejemplo:
```bash
cd BackEnd/ApiCrudUsuarios
dotnet add package Serilog
```

#### Resultado esperado:
```
  Determining projects to restore...
  info : Adding PackageReference for 'Serilog' to project '/path/ApiCrudUsuarios.csproj'.
  info :   GET https://api.nuget.org/v3/registration5-gz-semver2/serilog/index.json
  info : Restoring packages for /path/ApiCrudUsuarios.csproj...
  info : Package 'Serilog' is compatible with all the specified frameworks in project.
  info : PackageReference for 'Serilog' version '3.1.1' added to file '/path/ApiCrudUsuarios.csproj'.
```

**¿Qué hace?**
1. Busca el paquete en NuGet.org
2. Descarga la última versión
3. Agrega una referencia al `.csproj`
4. Ejecuta `dotnet restore`

#### Especificar versión:
```bash
dotnet add package Serilog --version 3.0.0
```

#### Ejemplo con MudBlazor:
```bash
cd FrontEnd/BlazorCrudUsuarios
dotnet add package MudBlazor
```

#### Modificación en el .csproj:
```xml
<ItemGroup>
    <PackageReference Include="Serilog" Version="3.1.1" />
</ItemGroup>
```

#### Paquetes comunes:

**Backend:**
```bash
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Swashbuckle.AspNetCore
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
```

**Frontend:**
```bash
dotnet add package MudBlazor
dotnet add package Microsoft.AspNetCore.Components.Authorization
dotnet add package System.IdentityModel.Tokens.Jwt
```

---

### 9. `dotnet list package`

#### Descripción:
Lista todos los paquetes NuGet instalados en el proyecto.

#### Cuándo utilizarlo:
- Para ver qué paquetes están instalados
- Para verificar versiones de dependencias
- Para auditar dependencias obsoletas o vulnerables

#### Ejemplo:
```bash
cd BackEnd/ApiCrudUsuarios
dotnet list package
```

#### Resultado esperado:
```
Project 'ApiCrudUsuarios' has the following package references
   [net8.0]:
   Top-level Package                                     Requested   Resolved
   > Microsoft.AspNetCore.Authentication.JwtBearer      8.0.26      8.0.26
   > Microsoft.AspNetCore.OpenApi                       8.0.28      8.0.28
   > Microsoft.EntityFrameworkCore                      8.0.26      8.0.26
   > Microsoft.EntityFrameworkCore.SqlServer            8.0.26      8.0.26
   > Microsoft.EntityFrameworkCore.Tools                8.0.26      8.0.26
   > Swashbuckle.AspNetCore                             6.6.2       6.6.2
```

**Columnas:**
- **Top-level Package**: Paquete instalado directamente
- **Requested**: Versión solicitada en `.csproj`
- **Resolved**: Versión realmente instalada

#### Verificar actualizaciones disponibles:
```bash
dotnet list package --outdated
```

#### Resultado:
```
Project 'ApiCrudUsuarios' has the following updates to its packages
   [net8.0]:
   Top-level Package                                 Requested   Resolved   Latest
   > Microsoft.EntityFrameworkCore                   8.0.26      8.0.26     8.0.30
   > Swashbuckle.AspNetCore                          6.6.2       6.6.2      6.8.0
```

#### Verificar vulnerabilidades:
```bash
dotnet list package --vulnerable
```

#### Incluir paquetes transitivos:
```bash
dotnet list package --include-transitive
```

---

## 🔧 Comandos Adicionales Útiles

### `dotnet new`

Crear proyectos desde plantillas:

```bash
# Web API
dotnet new webapi -n MiApi

# Blazor WebAssembly
dotnet new blazorwasm -n MiBlazor

# Librería de clases
dotnet new classlib -n MiLibreria

# Solución
dotnet new sln -n MiSolucion

# Agregar proyecto a solución
dotnet sln add BackEnd/ApiCrudUsuarios/ApiCrudUsuarios.csproj
```

---

### `dotnet test`

Ejecutar pruebas unitarias:

```bash
dotnet test
```

---

### `dotnet ef` (Entity Framework)

Gestionar migraciones de base de datos:

```bash
# Agregar migración
dotnet ef migrations add NombreMigracion

# Actualizar base de datos
dotnet ef database update

# Listar migraciones
dotnet ef migrations list

# Eliminar última migración
dotnet ef migrations remove

# Generar script SQL
dotnet ef migrations script
```

**Nota**: Requiere instalar `dotnet-ef` globalmente:
```bash
dotnet tool install --global dotnet-ef
```

---

## 📊 Flujo de Trabajo Típico

### Durante el Desarrollo:

```bash
# 1. Clonar repositorio
git clone https://github.com/usuario/CrudUsuarios.git
cd CrudUsuarios

# 2. Restaurar dependencias
cd BackEnd/ApiCrudUsuarios
dotnet restore

cd ../../FrontEnd/BlazorCrudUsuarios
dotnet restore

# 3. Ejecutar BackEnd en modo watch
cd ../../BackEnd/ApiCrudUsuarios
dotnet watch

# 4. En otra terminal, ejecutar FrontEnd
cd ../../FrontEnd/BlazorCrudUsuarios
dotnet watch
```

---

### Antes de Hacer Commit:

```bash
# 1. Limpiar archivos generados
dotnet clean

# 2. Compilar para verificar errores
dotnet build

# 3. (Opcional) Ejecutar tests
dotnet test
```

---

### Para Desplegar en Producción:

```bash
# 1. Publicar BackEnd
cd BackEnd/ApiCrudUsuarios
dotnet publish -c Release -o ./publish

# 2. Publicar FrontEnd
cd ../../FrontEnd/BlazorCrudUsuarios
dotnet publish -c Release -o ./publish

# 3. Copiar archivos publicados al servidor
scp -r publish/* usuario@servidor:/var/www/miapp/

# 4. En el servidor, ejecutar
dotnet ApiCrudUsuarios.dll
```

---

## 🎓 Diferencias entre Comandos

### `dotnet run` vs `dotnet watch`

| Comando | Reinicio automático | Uso |
|---------|---------------------|-----|
| `dotnet run` | ❌ No | Ejecución normal |
| `dotnet watch` | ✅ Sí | Desarrollo con hot reload |

---

### `dotnet build` vs `dotnet publish`

| Aspecto | build | publish |
|---------|-------|---------|
| Propósito | Verificar compilación | Preparar para producción |
| Optimización | Debug | Release |
| Salida | bin/Debug/ | Carpeta especificada |
| Dependencias | Solo referencias | Todas incluidas |
| Uso | Desarrollo | Despliegue |

---

### `dotnet restore` vs `dotnet build`

- `dotnet restore`: Solo descarga paquetes, **no compila**
- `dotnet build`: Descarga paquetes **y compila**

**En la práctica**: No necesitas ejecutar `restore` antes de `build`, porque `build` lo hace automáticamente.

---

## 💡 Consejos y Buenas Prácticas

### 1. Usa `dotnet watch` en desarrollo
```bash
dotnet watch
```
Ahorra tiempo al reiniciar automáticamente.

---

### 2. Especifica la configuración
```bash
# Desarrollo (por defecto)
dotnet run

# Producción
dotnet run -c Release
```

---

### 3. No hagas commit de bin/ y obj/
Estas carpetas se generan automáticamente con `dotnet build`.

**Archivo .gitignore:**
```
bin/
obj/
*.user
*.suo
```

---

### 4. Usa variables de entorno
```bash
# Linux/Mac
export ASPNETCORE_ENVIRONMENT=Production
dotnet run

# Windows
set ASPNETCORE_ENVIRONMENT=Production
dotnet run
```

---

### 5. Verifica actualizaciones regularmente
```bash
dotnet list package --outdated
```

---

### 6. Mantén el SDK actualizado
```bash
# Ver versión actual
dotnet --version

# Descargar última versión desde:
# https://dotnet.microsoft.com/download
```

---

## 📚 Recursos Adicionales

- **Documentación oficial**: https://docs.microsoft.com/dotnet/core/tools/
- **Plantillas disponibles**: `dotnet new --list`
- **Ayuda de comandos**: `dotnet [comando] --help`

---

## 🎯 Resumen Rápido

| Comando | Uso |
|---------|-----|
| `dotnet --version` | Ver versión de .NET |
| `dotnet restore` | Descargar dependencias |
| `dotnet build` | Compilar proyecto |
| `dotnet run` | Ejecutar aplicación |
| `dotnet watch` | Ejecutar con hot reload |
| `dotnet clean` | Limpiar archivos generados |
| `dotnet publish` | Preparar para producción |
| `dotnet add package` | Instalar paquete NuGet |
| `dotnet list package` | Listar paquetes instalados |

---

**Conclusión**: La .NET CLI proporciona todas las herramientas necesarias para desarrollar, compilar y desplegar aplicaciones .NET desde la línea de comandos, sin necesidad de un IDE.

