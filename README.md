# Portal Académico - Examen Parcial

Este repositorio contiene la resolución del examen parcial para el desarrollo de un **Portal Académico** construido con **ASP.NET Core MVC (.NET 10)**.

## Arquitectura y Stack Tecnológico
- **Framework:** ASP.NET Core MVC (.NET 10)
- **Base de Datos:** SQLite (Entity Framework Core)
- **Caché y Sesiones:** RedisLabs (StackExchange.Redis)
- **Seguridad:** ASP.NET Core Identity (Roles, Autenticación, Autorización)
- **Diseño UI:** Bootstrap 5 con Bootstrap Icons

## Credenciales Base
El sistema inicializa automáticamente la base de datos la primera vez que arranca y crea un Coordinador por defecto:
- **Email:** `coordinador@portal.edu`
- **Contraseña:** `Admin123!`

---

## 🚀 Despliegue en Render.com (Web Service)

Para cumplir con la **Pregunta 6**, este repositorio está completamente preparado para desplegarse en [Render.com](https://render.com/) utilizando Docker de forma transparente.

### Pasos de Despliegue:
1. Entra a Render.com y crea un nuevo **Web Service**.
2. Conecta tu repositorio de GitHub y selecciona el branch `main`.
3. Selecciona como entorno **Docker** (Render detectará automáticamente el archivo `Dockerfile` en la raíz).
4. En **Environment Variables**, debes configurar exactamente estas 4 variables:
   - `ASPNETCORE_ENVIRONMENT` = `Production`
   - `ASPNETCORE_URLS` = `http://0.0.0.0:8080`
   - `ConnectionStrings__DefaultConnection` = `DataSource=app.db;Cache=Shared`
   - `ConnectionStrings__RedisConnection` = `AQUÍ_TU_CONEXIÓN_DE_REDIS_LABS`
5. Haz clic en **Create Web Service**.

### Validaciones en Servidor Funcionales:
✅ Login protegido con bloqueo de rutas según Rol (Estudiante vs Coordinador).
✅ Catálogo de Cursos con filtros y botón Inscribir.
✅ Inscripción que respeta cruces de horarios y cupos máximos.
✅ Panel de Coordinador donde el Administrador confirma las matrículas.
✅ Uso de Redis en la nube para mantener las sesiones ("Volver al curso") y el Caché Distribuido (acelerando el catálogo).
