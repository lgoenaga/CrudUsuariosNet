# 02 - Frontend con Blazor WebAssembly

## 🎨 ¿Qué es Blazor?

**Blazor** es un framework de Microsoft que permite crear aplicaciones web interactivas usando **C# en lugar de JavaScript**. Existen dos versiones:

### Blazor Server
- El código C# se ejecuta en el servidor
- Solo se envía HTML al navegador
- Requiere conexión constante con el servidor

### Blazor WebAssembly (lo que usa este proyecto)
- El código C# se descarga y **ejecuta directamente en el navegador**
- Funciona como una SPA (Single Page Application)
- No requiere conexión constante después de cargarse

## 🌐 ¿Qué es una SPA (Single Page Application)?

Una **SPA** es una aplicación web que:
- Se carga **una sola vez** en el navegador
- **No recarga la página** al navegar entre secciones
- Es más **rápida y fluida** que las aplicaciones web tradicionales
- Funciona como una aplicación de escritorio o móvil

**Ejemplo**: Gmail, Facebook, Twitter son SPAs.

## 📂 Estructura del FrontEnd

```
FrontEnd/BlazorCrudUsuarios/
│
├── Program.cs                          # ⭐ Punto de entrada de la aplicación
├── App.razor                           # Componente raíz (Router principal)
├── _Imports.razor                      # Importaciones globales para todos los componentes
├── BlazorCrudUsuarios.csproj          # Archivo de proyecto
│
├── wwwroot/                            # 📁 Archivos estáticos (HTML, CSS, JS)
│   ├── index.html                      # HTML principal que carga Blazor
│   ├── css/app.css                     # Estilos personalizados
│   └── favicon.png                     # Icono de la aplicación
│
└── src/
    ├── UI/                             # 🎨 Interfaz de usuario
    │   ├── Pages/                      # Páginas principales
    │   │   ├── Login.razor             # Página de inicio de sesión
    │   │   ├── Users.razor             # Lista de usuarios (solo Admin)
    │   │   ├── CreateUser.razor        # Formulario crear usuario
    │   │   ├── EditUser.razor          # Formulario editar usuario
    │   │   └── UserProfile.razor       # Perfil del usuario actual
    │   │
    │   ├── Components/                 # Componentes reutilizables
    │   │   └── ConfirmDialog.razor     # Diálogo de confirmación
    │   │
    │   └── Layouts/                    # Plantillas de diseño
    │       └── MainLayout.razor        # Layout principal (navbar, footer)
    │
    ├── Application/                    # 🔧 Lógica de aplicación
    │   ├── Services/                   # Servicios del frontend
    │   │   ├── AuthService.cs          # Servicio de autenticación
    │   │   ├── UserService.cs          # Servicio de gestión de usuarios
    │   │   ├── TokenService.cs         # Gestión de tokens JWT
    │   │   └── CustomAuthenticationStateProvider.cs  # Estado de autenticación
    │   │
    │   ├── Interfaces/                 # Contratos de servicios
    │   │   ├── IAuthService.cs
    │   │   └── IUserService.cs
    │   │
    │   └── Models/                     # Modelos de datos (DTOs)
    │       ├── LoginRequest.cs
    │       ├── LoginResponse.cs
    │       ├── UserResponse.cs
    │       └── CreateUserRequest.cs
    │
    └── Shared/
        └── Constants/
            └── ApiRoutes.cs            # Rutas de la API centralizadas
```

## 🧩 ¿Qué es un Componente Razor?

Un **componente Razor** es un archivo `.razor` que combina:
- **HTML**: Para la estructura visual
- **C#**: Para la lógica y datos
- **CSS** (opcional): Para estilos

### Estructura de un Componente:

```razor
@page "/ruta"                           <!-- Ruta de navegación -->
@using Namespace                        <!-- Importaciones -->

<!-- ✅ PARTE HTML (Markup) -->
<div>
    <h1>Hola @nombre</h1>
    <button @onclick="Saludar">Click</button>
</div>

@code {
    // ✅ PARTE C# (Lógica)
    string nombre = "Estudiante";
    
    void Saludar() 
    {
        nombre = "Mundo";
    }
}
```

## 📄 Archivos Clave del Frontend

### 1️⃣ Program.cs - Punto de Entrada

**Ruta**: `FrontEnd/BlazorCrudUsuarios/Program.cs`

**Propósito**: Configurar e iniciar la aplicación Blazor.

**Código principal**:
```csharp
var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");           // Carga App.razor en div #app
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices();                 // Componentes MudBlazor

// HttpClient para backend
builder.Services.AddScoped(_ => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7068/")  // URL de la API
});

// Servicios
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddAuthorizationCore();          // Autorización

builder.Services.AddScoped<AuthenticationStateProvider, 
    CustomAuthenticationStateProvider>();         // Estado de autenticación

await builder.Build().RunAsync();
```

**¿Qué hace?**
- Registra servicios para usar en toda la aplicación (inyección de dependencias)
- Configura `HttpClient` para comunicarse con la API
- Habilita autenticación y autorización

---

### 2️⃣ App.razor - Componente Raíz

**Ruta**: `FrontEnd/BlazorCrudUsuarios/App.razor`

**Propósito**: Configurar el enrutamiento de la aplicación.

**Código**:
```razor
<CascadingAuthenticationState>
    <Router AppAssembly="@typeof(App).Assembly">
        <Found Context="routeData">
            <RouteView RouteData="@routeData" DefaultLayout="@typeof(MainLayout)"/>
        </Found>
        <NotFound>
            <p>Página no encontrada.</p>
        </NotFound>
    </Router>
</CascadingAuthenticationState>
```

**¿Qué hace?**
- `Router`: Busca componentes con `@page` y los muestra según la URL
- `CascadingAuthenticationState`: Proporciona información de autenticación a todos los componentes
- `DefaultLayout`: Usa `MainLayout.razor` como plantilla por defecto

---

### 3️⃣ _Imports.razor - Importaciones Globales

**Ruta**: `FrontEnd/BlazorCrudUsuarios/_Imports.razor`

**Propósito**: Declarar using que todos los componentes necesitan.

```razor
@using System.Net.Http
@using System.Net.Http.Json
@using Microsoft.AspNetCore.Components.Forms
@using Microsoft.AspNetCore.Components.Routing
@using Microsoft.AspNetCore.Components.Authorization
@using BlazorCrudUsuarios
@using MudBlazor
```

**¿Qué hace?**
Evita tener que escribir `@using` en cada archivo `.razor`.

---

### 4️⃣ wwwroot/index.html - HTML Principal

**Ruta**: `FrontEnd/BlazorCrudUsuarios/wwwroot/index.html`

**Propósito**: Página HTML que carga Blazor.

**Elementos importantes**:
```html
<div id="app">...</div>                 <!-- Aquí se carga Blazor -->
<script src="_framework/blazor.webassembly.js"></script>  <!-- Framework Blazor -->

<script>
    // Helper para localStorage (guardar token)
    window.localStorageHelper = {
        set: function (key, value) {
            localStorage.setItem(key, value);
        },
        get: function (key) {
            return localStorage.getItem(key);
        },
        remove: function (key) {
            localStorage.removeItem(key);
        }
    };
</script>
```

---

## 🎨 Layouts - Plantillas de Diseño

### MainLayout.razor

**Ruta**: `FrontEnd/BlazorCrudUsuarios/src/UI/Layouts/MainLayout.razor`

**Propósito**: Define la estructura común de todas las páginas (navbar, footer, contenido).

**Componentes principales**:
- **MudAppBar**: Barra de navegación superior
- **MudDrawer**: Menú lateral para móviles
- **AuthorizeView**: Muestra contenido según autenticación
- **@Body**: Aquí se carga el contenido de cada página

**Interacción con otros componentes**:
```
MainLayout.razor
    ├── Contiene navegación
    ├── Muestra email del usuario autenticado
    ├── Botón Logout → llama a Logout()
    └── @Body → Carga Login.razor, Users.razor, etc.
```

**Método importante**:
```csharp
private async Task Logout()
{
    await Js.InvokeVoidAsync("localStorageHelper.remove", "token");
    Navigation.NavigateTo("/login", forceLoad: true);
}
```

---

## 📄 Pages - Páginas Razor

### 1. Login.razor

**Ruta**: `FrontEnd/BlazorCrudUsuarios/src/UI/Pages/Login.razor`

**Propósito**: Página de inicio de sesión.

**Rutas**: 
- `@page "/"`
- `@page "/Login"`

**Componentes clave**:
- `MudTextField`: Campos de entrada para email y password
- `MudButton`: Botón de login

**Métodos importantes**:
```csharp
private async Task HandleLogin()
{
    var token = await AuthService.Login(new LoginRequest
    {
        Email = _email,
        Password = _password
    });
    
    // Guardar token en localStorage
    await Js.InvokeVoidAsync("localStorageHelper.set", "token", token);
    
    // Notificar cambio de autenticación
    var customAuth = (CustomAuthenticationStateProvider)AuthStateProvider;
    customAuth.NotifyUserAuthentication(token);
    
    // Redirigir según rol
    var role = await TokenService.GetRole();
    Navigation.NavigateTo(role == "Admin" ? "/users" : "/users/me");
}
```

**Inyecciones de dependencias**:
```csharp
@inject IAuthService AuthService
@inject IJSRuntime Js
@inject NavigationManager Navigation
@inject AuthenticationStateProvider AuthStateProvider
@inject TokenService TokenService
```

---

### 2. Users.razor

**Ruta**: `FrontEnd/BlazorCrudUsuarios/src/UI/Pages/Users.razor`

**Propósito**: Lista todos los usuarios (solo Admin).

**Ruta**: `@page "/Users"`

**Autorización**: `@attribute [Authorize(Roles = "Admin")]`

**Componentes clave**:
- `MudTable`: Tabla de usuarios
- `MudButton`: Botón "Nuevo Usuario"
- `MudChip`: Muestra el rol del usuario

**Métodos importantes**:
```csharp
protected override async Task OnInitializedAsync()
{
    // Verificar autorización
    var state = await AuthStateProvider.GetAuthenticationStateAsync();
    var user = state.User;

    if (!user.IsInRole("Admin"))
    {
        Navigation.NavigateTo("/users/me");
        return;
    }
    
    // Cargar usuarios
    _users = await UserService.GetUsers();
}

private async Task ConfirmDelete(int id)
{
    // Mostrar diálogo de confirmación
    var dialog = await DialogService.ShowAsync<ConfirmDialog>("Confirmación", parameters);
    var result = await dialog.Result;
    
    if (result is { Canceled: false, Data: true })
    {
        await Delete(id);
    }
}
```

**Interacción con otros componentes**:
```
Users.razor
    ├── Llama a UserService.GetUsers()
    ├── Muestra ConfirmDialog al eliminar
    ├── Navega a CreateUser.razor (botón Nuevo)
    └── Navega a EditUser.razor (botón Editar)
```

---

### 3. CreateUser.razor

**Ruta**: `FrontEnd/BlazorCrudUsuarios/src/UI/Pages/CreateUser.razor`

**Propósito**: Formulario para crear un nuevo usuario.

**Ruta**: `@page "/users/create"`

**Autorización**: `@attribute [Authorize]`

**Método principal**:
```csharp
private async Task HandleCreate()
{
    await UserService.CreateUser(new CreateUserRequest
    {
        Email = _email,
        Password = _password
    });
    
    // Volver a la lista
    Navigation.NavigateTo("/users");
}
```

---

### 4. EditUser.razor

**Ruta**: `FrontEnd/BlazorCrudUsuarios/src/UI/Pages/EditUser.razor`

**Propósito**: Formulario para editar un usuario existente.

**Ruta**: `@page "/users/edit/{Id:int}"`

**Parámetro**: `[Parameter] public int Id { get; set; }`

**Método de inicialización**:
```csharp
protected override async Task OnInitializedAsync()
{
    // Obtener datos del usuario
    var users = await UserService.GetUsers();
    var user = users.FirstOrDefault(u => u.Id == Id);
    
    if (user != null)
    {
        _email = user.Email;
    }
}
```

---

### 5. UserProfile.razor

**Ruta**: `FrontEnd/BlazorCrudUsuarios/src/UI/Pages/UserProfile.razor`

**Propósito**: Mostrar perfil del usuario autenticado.

**Rutas**: 
- `@page "/UserProfile"`
- `@page "/users/me"`

**Método**:
```csharp
protected override async Task OnInitializedAsync()
{
    var token = await TokenService.GetToken();
    
    var handler = new JwtSecurityTokenHandler();
    var jwt = handler.ReadJwtToken(token);
    
    _email = jwt.Claims.FirstOrDefault(c => c.Type.Contains("email"))?.Value;
    _role = jwt.Claims.FirstOrDefault(c => c.Type.Contains("role"))?.Value;
}
```

---

## 🧩 Components - Componentes Reutilizables

### ConfirmDialog.razor

**Ruta**: `FrontEnd/BlazorCrudUsuarios/src/UI/Components/ConfirmDialog.razor`

**Propósito**: Diálogo de confirmación para acciones críticas (eliminar).

**Uso**:
```csharp
var dialog = await DialogService.ShowAsync<ConfirmDialog>("Confirmación", parameters);
```

**Métodos**:
```csharp
void Confirm() => MudDialog.Close(DialogResult.Ok(true));
void Cancel() => MudDialog.Cancel();
```

---

## 🔧 Services - Servicios del Frontend

### 1. AuthService.cs

**Ruta**: `FrontEnd/BlazorCrudUsuarios/src/Application/Services/AuthService.cs`

**Propósito**: Gestionar autenticación (login).

**Métodos**:

```csharp
public async Task<string> Login(LoginRequest request)
{
    var response = await _httpClient.PostAsJsonAsync(ApiRoutes.Auth.Login, request);
    
    if (!response.IsSuccessStatusCode)
        throw new Exception("Error en login");
    
    var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
    return result!.Token;
}

public async Task SetAuthHeader()
{
    var token = await _tokenService.GetToken();
    
    if (!string.IsNullOrEmpty(token))
    {
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
    }
}
```

**Interacción**:
```
Login.razor → AuthService.Login() → POST /api/auth/login → Backend
```

---

### 2. UserService.cs

**Ruta**: `FrontEnd/BlazorCrudUsuarios/src/Application/Services/UserService.cs`

**Propósito**: Gestionar operaciones CRUD de usuarios.

**Métodos**:

```csharp
public async Task<List<UserResponse>> GetUsers()
{
    await _authService.SetAuthHeader();  // Añade el token JWT
    return await _httpClient.GetFromJsonAsync<List<UserResponse>>(ApiRoutes.Users.Base)
           ?? new List<UserResponse>();
}

public async Task CreateUser(CreateUserRequest request)
{
    await _authService.SetAuthHeader();
    var response = await _httpClient.PostAsJsonAsync(ApiRoutes.Users.Base, request);
    if (!response.IsSuccessStatusCode)
        throw new Exception("Error creando usuario");
}

public async Task DeleteUser(int id)
{
    await _authService.SetAuthHeader();
    var response = await _httpClient.DeleteAsync(ApiRoutes.Users.ById(id));
    if (!response.IsSuccessStatusCode)
        throw new Exception("Error eliminando usuario");
}

public async Task UpdateUser(int id, CreateUserRequest request)
{
    await _authService.SetAuthHeader();
    var response = await _httpClient.PutAsJsonAsync(ApiRoutes.Users.ById(id), request);
    if (!response.IsSuccessStatusCode)
        throw new Exception("Error actualizando usuario");
}
```

---

### 3. TokenService.cs

**Ruta**: `FrontEnd/BlazorCrudUsuarios/src/Application/Services/TokenService.cs`

**Propósito**: Gestionar tokens JWT (obtener token y rol desde localStorage).

**Métodos**:

```csharp
public async Task<string?> GetToken()
{
    return await _js.InvokeAsync<string>("localStorageHelper.get", "token");
}

public async Task<string?> GetRole()
{
    var token = await GetToken();
    
    if (string.IsNullOrEmpty(token))
        return null;
    
    var handler = new JwtSecurityTokenHandler();
    var jwt = handler.ReadJwtToken(token);
    
    return jwt.Claims
        .FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
}
```

**Uso**:
- **JavaScript Interop**: `_js.InvokeAsync()` llama a funciones JavaScript desde C#

---

### 4. CustomAuthenticationStateProvider.cs

**Ruta**: `FrontEnd/BlazorCrudUsuarios/src/Application/Services/CustomAuthenticationStateProvider.cs`

**Propósito**: Proveer el estado de autenticación a toda la aplicación.

**Métodos principales**:

```csharp
public override async Task<AuthenticationState> GetAuthenticationStateAsync()
{
    var token = await _tokenService.GetToken();
    
    var identity = new ClaimsIdentity();
    
    if (!string.IsNullOrEmpty(token))
    {
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);
        
        // Crear identidad con claims del token
        identity = new ClaimsIdentity(
            new[] { new Claim(ClaimTypes.Name, emailClaim.Value) }
                .Concat(jwt.Claims),
            "jwt"
        );
    }
    
    var user = new ClaimsPrincipal(identity);
    return new AuthenticationState(user);
}

public void NotifyUserAuthentication(string token)
{
    // ... crear identidad ...
    
    NotifyAuthenticationStateChanged(
        Task.FromResult(new AuthenticationState(user))
    );
}
```

**Uso**: Cuando el usuario hace login, se llama a `NotifyUserAuthentication()` para actualizar el estado en toda la app.

---

## 📦 Models - Modelos de Datos

### LoginRequest.cs
```csharp
public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
```

### LoginResponse.cs
```csharp
public class LoginResponse
{
    public string Token { get; init; } = string.Empty;
}
```

### UserResponse.cs
```csharp
public class UserResponse
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}
```

### CreateUserRequest.cs
```csharp
public class CreateUserRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
```

---

## 🗺️ Navegación y Enrutamiento

### Rutas disponibles:

| Ruta | Componente | Autorización |
|------|-----------|-------------|
| `/` | Login.razor | Pública |
| `/login` | Login.razor | Pública |
| `/users` | Users.razor | Solo Admin |
| `/users/create` | CreateUser.razor | Autenticado |
| `/users/edit/{id}` | EditUser.razor | Autenticado |
| `/users/me` | UserProfile.razor | Autenticado |

### Navegación programática:

```csharp
@inject NavigationManager Navigation

Navigation.NavigateTo("/users");               // Navegar sin recargar
Navigation.NavigateTo("/login", forceLoad: true);  // Forzar recarga
```

---

## 💉 Inyección de Dependencias

En Blazor, puedes inyectar servicios en componentes:

```razor
@inject IAuthService AuthService
@inject NavigationManager Navigation
@inject ISnackbar Snackbar
```

Esto es equivalente a:
```csharp
[Inject] public IAuthService AuthService { get; set; }
```

---

## 📡 Consumo de APIs

### Ejemplo de petición GET:
```csharp
var users = await _httpClient.GetFromJsonAsync<List<UserResponse>>("api/users");
```

### Ejemplo de petición POST:
```csharp
var response = await _httpClient.PostAsJsonAsync("api/auth/login", request);
var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
```

### Ejemplo de petición DELETE:
```csharp
var response = await _httpClient.DeleteAsync($"api/users/{id}");
```

### Ejemplo de petición PUT:
```csharp
var response = await _httpClient.PutAsJsonAsync($"api/users/{id}", request);
```

---

## 📊 Flujo de Navegación del Usuario

```mermaid
flowchart TD
    A[Usuario ingresa a /] --> B[Carga Login.razor]
    B --> C{Credenciales válidas?}
    C -->|No| B
    C -->|Sí| D[Guardar token en localStorage]
    D --> E[Actualizar AuthenticationState]
    E --> F{Rol del usuario?}
    F -->|Admin| G[Redirigir a /users]
    F -->|User| H[Redirigir a /users/me]
    G --> I[Listar todos los usuarios]
    I --> J{Acción?}
    J -->|Crear| K[/users/create]
    J -->|Editar| L[/users/edit/id]
    J -->|Eliminar| M[Confirmar y eliminar]
    K --> G
    L --> G
    M --> G
    H --> N[Ver perfil propio]
    
    style A fill:#e3f2fd
    style G fill:#c8e6c9
    style H fill:#fff9c4
```

---

## 🎨 Componentes MudBlazor Utilizados

- `MudAppBar`: Barra de navegación
- `MudDrawer`: Menú lateral
- `MudTable`: Tablas de datos
- `MudTextField`: Campos de texto
- `MudButton`: Botones
- `MudPaper`: Contenedores con elevación
- `MudDialog`: Diálogos modales
- `MudSnackbar`: Notificaciones
- `MudChip`: Etiquetas (para roles)
- `MudNavLink`: Enlaces de navegación

---

## 🔑 Conceptos Clave

### @page
Define la ruta de una página:
```razor
@page "/users"
```

### @code
Bloque donde se escribe código C#:
```razor
@code {
    string nombre = "Luis";
    void Saludar() { }
}
```

### @inject
Inyecta servicios:
```razor
@inject HttpClient Http
```

### @bind
Enlace de datos bidireccional:
```razor
<input @bind="nombre" />
```

### @onclick
Manejador de eventos:
```razor
<button @onclick="Guardar">Guardar</button>
```

### @attribute [Authorize]
Protege páginas:
```razor
@attribute [Authorize(Roles = "Admin")]
```

---

**Conclusión**: El frontend Blazor proporciona una experiencia de usuario moderna y fluida, comunicándose con el backend a través de HTTP y gestionando el estado de autenticación con JWT tokens almacenados en localStorage.

