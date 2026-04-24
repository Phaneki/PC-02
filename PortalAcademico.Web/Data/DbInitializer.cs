using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PortalAcademico.Web.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PortalAcademico.Web.Data;

public static class DbInitializer
{
    public static async Task Initialize(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        // Aplicar migraciones pendientes
        await context.Database.MigrateAsync();

        // 1. Crear Roles
        string[] roles = { "Coordinador", "Estudiante" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // 2. Crear Usuario Coordinador
        var adminEmail = "coordinador@portal.edu";
        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var adminUser = new IdentityUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, "Admin123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Coordinador");
            }
        }

        // 3. Crear 3 cursos activos
        if (!context.Cursos.Any())
        {
            var cursos = new Curso[]
            {
                new Curso 
                { 
                    Codigo = "CS101", 
                    Nombre = "Introducción a la Programación", 
                    Creditos = 4, 
                    CupoMaximo = 30, 
                    HorarioInicio = new TimeSpan(8, 0, 0), 
                    HorarioFin = new TimeSpan(10, 0, 0), 
                    Activo = true 
                },
                new Curso 
                { 
                    Codigo = "CS201", 
                    Nombre = "Estructuras de Datos", 
                    Creditos = 5, 
                    CupoMaximo = 25, 
                    HorarioInicio = new TimeSpan(10, 0, 0), 
                    HorarioFin = new TimeSpan(12, 0, 0), 
                    Activo = true 
                },
                new Curso 
                { 
                    Codigo = "MA101", 
                    Nombre = "Cálculo I", 
                    Creditos = 4, 
                    CupoMaximo = 40, 
                    HorarioInicio = new TimeSpan(14, 0, 0), 
                    HorarioFin = new TimeSpan(16, 0, 0), 
                    Activo = true 
                }
            };

            context.Cursos.AddRange(cursos);
            await context.SaveChangesAsync();
        }
    }
}
