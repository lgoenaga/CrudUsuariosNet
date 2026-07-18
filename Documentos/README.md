# 📚 Documentación del Proyecto CRUD de Usuarios

## 🎓 Bienvenido

Esta documentación ha sido creada especialmente para estudiantes que están aprendiendo desarrollo web con .NET, Blazor y Web API. No se asume conocimiento previo de estas tecnologías.

## 📖 ¿Cómo usar esta documentación?

### Orden de lectura recomendado:

1. **[01-Descripcion-General.md](01-Descripcion-General.md)** (⏱️ 20 min)
   - Comienza aquí para entender el panorama completo
   - Conoce la arquitectura general del proyecto
   - Aprende cómo interactúan FrontEnd y BackEnd

2. **[02-Frontend-Blazor.md](02-Frontend-Blazor.md)** (⏱️ 40 min)
   - Aprende qué es Blazor y cómo funciona
   - Entiende la estructura del FrontEnd
   - Conoce componentes, páginas y servicios
   - Descubre cómo se consume la API

3. **[03-Backend-WebApi.md](03-Backend-WebApi.md)** (⏱️ 45 min)
   - Comprende qué es una Web API
   - Aprende sobre arquitectura en capas
   - Conoce los endpoints disponibles
   - Entiende JWT, Entity Framework y servicios

4. **[04-Comandos-DotNet.md](04-Comandos-DotNet.md)** (⏱️ 30 min)
   - Domina los comandos esenciales de .NET CLI
   - Aprende a compilar, ejecutar y publicar
   - Conoce las mejores prácticas

5. **[05-Archivos-Clave-DotNet.md](05-Archivos-Clave-DotNet.md)** (⏱️ 40 min)
   - Entiende cada archivo importante del proyecto
   - Aprende cuándo se ejecuta cada componente
   - Conoce la inyección de dependencias

## 🎯 Objetivos de aprendizaje

Al completar esta documentación, serás capaz de:

✅ Entender la arquitectura de una aplicación full-stack con .NET  
✅ Crear componentes Blazor y páginas interactivas  
✅ Desarrollar una Web API con autenticación JWT  
✅ Usar Entity Framework Core para acceso a datos  
✅ Implementar autenticación y autorización  
✅ Consumir APIs desde el frontend  
✅ Usar comandos de .NET CLI con confianza  
✅ Comprender la inyección de dependencias  

## 🛠️ Requisitos previos

Antes de comenzar, asegúrate de tener:

- **.NET 8 SDK** instalado ([Descargar](https://dotnet.microsoft.com/download))
- **Docker Desktop** para SQL Server ([Descargar](https://www.docker.com/products/docker-desktop))
- Un **editor de código** (VS Code, Visual Studio, Rider)
- Conocimientos básicos de **C#** y **programación orientada a objetos**
- Conocimientos básicos de **HTML**

## 🚀 Primeros pasos

### 1. Verificar instalación de .NET

```bash
dotnet --version
```

Deberías ver: `8.0.xxx`

### 2. Clonar el repositorio

```bash
git clone <url-del-repositorio>
cd CrudUsuarios
```

### 3. Iniciar la base de datos

```bash
docker-compose up -d
```

### 4. Ejecutar el BackEnd

```bash
cd BackEnd/ApiCrudUsuarios
dotnet restore
dotnet run
```

La API estará disponible en: `https://localhost:7068`  
Swagger UI en: `https://localhost:7068/swagger`

### 5. Ejecutar el FrontEnd

En otra terminal:

```bash
cd FrontEnd/BlazorCrudUsuarios
dotnet restore
dotnet run
```

La aplicación estará disponible en: `https://localhost:7025`

## 📋 Estructura de la documentación

```
Documentos/
├── README.md                      # Este archivo
├── 01-Descripcion-General.md      # Visión general del proyecto
├── 02-Frontend-Blazor.md          # Documentación del FrontEnd
├── 03-Backend-WebApi.md           # Documentación del BackEnd
├── 04-Comandos-DotNet.md          # Guía de comandos CLI
└── 05-Archivos-Clave-DotNet.md    # Explicación de archivos importantes
```

## 🎨 Convenciones de la documentación

- 📌 **Nota importante**: Información relevante
- ✅ **Buena práctica**: Recomendación
- ❌ **Evitar**: Práctica no recomendada
- 🔥 **Ejemplo de código**: Código para copiar y probar
- 💡 **Consejo**: Sugerencia útil
- ⚠️ **Advertencia**: Posible problema

## 🧪 Ejercicios prácticos

Para reforzar tu aprendizaje, intenta:

### Nivel Principiante:
1. Cambiar el puerto del BackEnd o FrontEnd
2. Agregar un nuevo campo a la tabla de usuarios (ej: Nombre, Apellido)
3. Crear una nueva página Razor de prueba
4. Modificar el diseño del MainLayout

### Nivel Intermedio:
5. Agregar validación de formularios con DataAnnotations
6. Implementar paginación en la lista de usuarios
7. Crear un nuevo endpoint en el BackEnd
8. Agregar un botón de "Cambiar contraseña"

### Nivel Avanzado:
9. Implementar refresh tokens
10. Agregar búsqueda y filtros en la lista de usuarios
11. Implementar una nueva entidad (ej: Productos)
12. Agregar pruebas unitarias

## 🔗 Recursos adicionales

### Documentación oficial:
- [Documentación .NET](https://docs.microsoft.com/dotnet/)
- [Documentación Blazor](https://docs.microsoft.com/aspnet/core/blazor/)
- [Documentación Entity Framework Core](https://docs.microsoft.com/ef/core/)

### Tutoriales recomendados:
- [Tutorial Blazor (Microsoft)](https://dotnet.microsoft.com/learn/aspnet/blazor-tutorial/intro)
- [Tutorial Web API (Microsoft)](https://docs.microsoft.com/aspnet/core/tutorials/first-web-api)

### Comunidad:
- [Stack Overflow - Etiqueta blazor](https://stackoverflow.com/questions/tagged/blazor)
- [Reddit - r/dotnet](https://reddit.com/r/dotnet)
- [Discord - .NET Community](https://discord.gg/dotnet)

## ❓ Preguntas frecuentes

### ¿Por qué no puedo conectarme a SQL Server?
Asegúrate de que Docker está ejecutándose y que el contenedor de SQL Server está activo:
```bash
docker ps
```

### ¿Por qué el FrontEnd no puede comunicarse con el BackEnd?
Verifica que:
1. El BackEnd está ejecutándose
2. La URL en `Program.cs` del FrontEnd coincide con la del BackEnd
3. CORS está configurado correctamente

### ¿Cómo creo un usuario Admin inicial?
Puedes registrar un usuario normal y luego actualizar su rol directamente en la base de datos.

### ¿Dónde se guarda el token JWT?
En el `localStorage` del navegador. Puedes verlo en las DevTools (F12) → Application → Local Storage.

## 🤝 Contribuciones

Si encuentras errores o quieres mejorar la documentación:
1. Crea un issue explicando el problema
2. O mejor aún, crea un pull request con la corrección

## 📝 Notas finales

Esta documentación es un recurso vivo que puede actualizarse. Si algo no está claro o falta información, no dudes en preguntar.

**¡Feliz aprendizaje! 🚀**

---

**Fecha de creación**: Julio 2026  
**Autor**: Documentación generada para estudiantes  
**Versión del proyecto**: .NET 8.0  

