# CRUD Usuarios .NET 8

Aplicación Full Stack para gestión de usuarios con autenticación y autorización basada en roles.

## 🚀 Tecnologías

### Backend
- **ASP.NET Core Web API (.NET 8)**
- **Entity Framework Core 8.0**
- **SQL Server 2022**
- **JWT Authentication**
- **Swagger/OpenAPI**

### Frontend
- **Blazor WebAssembly (.NET 8)**
- **MudBlazor 9.5.0** (Material Design)
- **HttpClient**
- **AuthenticationStateProvider**

## 📂 Estructura del Proyecto

```
CrudUsuarios/
├── BackEnd/
│   └── ApiCrudUsuarios/          # Web API
│       ├── src/
│       │   ├── WebApi/           # Controllers, Program.cs
│       │   ├── Application/      # Services, DTOs, Interfaces
│       │   ├── Domain/           # Models, Constants
│       │   └── Infrastructure/   # DbContext, Migrations
│       └── appsettings.json
│
├── FrontEnd/
│   └── BlazorCrudUsuarios/       # Blazor WebAssembly
│       ├── src/
│       │   ├── UI/               # Pages, Components, Layouts
│       │   ├── Application/      # Services, Models
│       │   └── Shared/           # Constants
│       └── wwwroot/              # Static files
│
├── Documentos/                   # Documentación completa
└── docker-compose.yml            # SQL Server container
```

## 🎯 Funcionalidades

- ✅ **Registro de usuarios** con contraseñas hasheadas
- ✅ **Login con JWT Token**
- ✅ **Autenticación y autorización** basada en roles (Admin, User)
- ✅ **CRUD completo de usuarios** (Crear, Listar, Editar, Eliminar)
- ✅ **Interfaz moderna** con Material Design (MudBlazor)
- ✅ **API RESTful** documentada con Swagger
- ✅ **Validaciones** en Frontend y Backend
- ✅ **Gestión de errores** personalizada

## 📋 Requisitos Previos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- IDE: [Visual Studio 2022](https://visualstudio.microsoft.com/), [JetBrains Rider](https://www.jetbrains.com/rider/) o [VS Code](https://code.visualstudio.com/)

## 🔧 Instalación y Configuración

### 1. Clonar el repositorio

```bash
git clone git@github.com:lgoenaga/CrudUsuariosNet.git
cd CrudUsuariosNet
```

### 2. Iniciar SQL Server (Docker)

```bash
docker-compose up -d
```

Esto creará un contenedor con SQL Server en `localhost:1433`.

**Credenciales:**
- Usuario: `sa`
- Contraseña: `Lagp2026.`

### 3. Ejecutar Migraciones

```bash
cd BackEnd/ApiCrudUsuarios
dotnet ef database update
```

### 4. Ejecutar el Backend (API)

```bash
cd BackEnd/ApiCrudUsuarios
dotnet run
```

La API estará disponible en:
- **HTTPS**: `https://localhost:7068`
- **Swagger UI**: `https://localhost:7068/swagger`

### 5. Ejecutar el Frontend (Blazor)

En otra terminal:

```bash
cd FrontEnd/BlazorCrudUsuarios
dotnet run
```

La aplicación web estará disponible en:
- **HTTPS**: `https://localhost:7025`

## 🔑 Usuarios de Prueba

Puedes registrar usuarios desde la interfaz o crearlos manualmente en la base de datos.

**Para crear un usuario Admin**, ejecuta en SQL Server:

```sql
INSERT INTO Users (Email, PasswordHash, Role)
VALUES ('admin@test.com', 'AQAAAAIAAYagAAAAEHashed...', 'Admin');
```

O registra un usuario normal y cambia su rol en la base de datos:

```sql
UPDATE Users SET Role = 'Admin' WHERE Email = 'tu@email.com';
```

## 🌐 Endpoints de la API

### Autenticación

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| POST | `/api/auth/register` | Registrar nuevo usuario |
| POST | `/api/auth/login` | Iniciar sesión (obtiene JWT) |

### Usuarios (requieren autenticación)

| Método | Endpoint | Descripción | Rol Requerido |
|--------|----------|-------------|---------------|
| GET | `/api/users` | Listar todos los usuarios | Admin |
| GET | `/api/users/{id}` | Obtener usuario por ID | Admin |
| GET | `/api/users/me` | Obtener perfil del usuario autenticado | User/Admin |
| POST | `/api/users` | Crear nuevo usuario | Admin |
| PUT | `/api/users/{id}` | Actualizar usuario | Admin |
| DELETE | `/api/users/{id}` | Eliminar usuario | Admin |

## 📚 Documentación

El proyecto incluye documentación detallada en la carpeta `Documentos/`:

1. **01-Descripcion-General.md** - Arquitectura y conceptos generales
2. **02-Frontend-Blazor.md** - Estructura del Frontend
3. **03-Backend-WebApi.md** - Estructura del Backend
4. **04-Comandos-DotNet.md** - Comandos útiles de .NET
5. **05-Archivos-Clave-DotNet.md** - Explicación de archivos importantes

## 🛡️ Seguridad

- ✅ Contraseñas hasheadas con **ASP.NET Core Identity PasswordHasher**
- ✅ Autenticación con **JWT Tokens**
- ✅ Autorización basada en **Roles**
- ✅ **CORS** configurado para permitir solo el origen del Frontend
- ✅ **HTTPS** habilitado por defecto

## 🧪 Tecnologías y Patrones Utilizados

- **Arquitectura en capas** (Presentation, Application, Domain, Infrastructure)
- **Repository Pattern** (con Entity Framework Core)
- **Dependency Injection**
- **DTOs** (Data Transfer Objects)
- **Exception Handling** personalizado
- **JWT Bearer Authentication**
- **Blazor Component Lifecycle**
- **Service Pattern**

## 📝 Configuración

### Backend (`appsettings.json`)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=CrudUsuariosDB;User Id=sa;Password=Lagp2026.;TrustServerCertificate=True;"
  },
  "JWT": {
    "KEY": "tu-clave-secreta-aqui-minimo-32-caracteres",
    "Issuer": "https://localhost:7068",
    "Audience": "https://localhost:7025"
  }
}
```

### Frontend (`Program.cs`)

```csharp
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7068/")
});
```

## 🤝 Contribuir

Las contribuciones son bienvenidas. Por favor:

1. Haz un Fork del proyecto
2. Crea una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abre un Pull Request

## 📄 Licencia

Este proyecto es de código abierto y está disponible bajo la licencia MIT.

## 👤 Autor

**Luis Goenaga**

## 📧 Contacto

Para preguntas o sugerencias, abre un issue en el repositorio.

---

⭐ Si este proyecto te fue útil, no olvides darle una estrella en GitHub!

