# Portal Académico — Gestión de Cursos y Matrículas

Este proyecto es la solución al Examen Parcial. Consiste en un portal web interno para que una universidad gestione cursos, estudiantes y matrículas.

## Tecnologías Utilizadas
- **Backend:** ASP.NET Core MVC (.NET 8)
- **Autenticación:** ASP.NET Core Identity
- **Base de Datos:** SQLite con Entity Framework Core
- **Vistas:** Razor Views
- **Caché / Sesión:** Redis
- **Infraestructura:** Render.com (Web Service) + RedisLabs

## Requisitos Previos
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Una instancia de Redis en ejecución (local o en la nube) para el manejo de sesiones.

## Configuración Local y Ejecución

1. Clonar el repositorio.
2. Navegar a la carpeta del proyecto web:
   ```bash
   cd PortalAcademico.Web
   ```
3. Aplicar las migraciones para crear la base de datos local SQLite:
   ```bash
   dotnet ef database update
   ```
4. Ejecutar la aplicación:
   ```bash
   dotnet run
   ```

## Variables de Entorno (Producción)
Para desplegar en Render.com, se deben configurar las siguientes variables de entorno:
- `ConnectionStrings__DefaultConnection`: Cadena de conexión a la base de datos SQLite (o PostgreSQL si se migra).
- `Redis__Configuration`: Cadena de conexión de Redis (ej. `redis-xxxxx.cloud.redislabs.com:port,password=YOUR_PASSWORD`).

## URL de Despliegue (Render)
*(Pendiente por configurar)*
