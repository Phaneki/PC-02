using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PortalAcademico.Web.Models;

namespace PortalAcademico.Web.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
{
    public DbSet<Curso> Cursos { get; set; }
    public DbSet<Matricula> Matriculas { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Índice único en Código de Curso
        builder.Entity<Curso>()
            .HasIndex(c => c.Codigo)
            .IsUnique();

        // Reglas de negocio para Curso a nivel de BD (Check Constraints)
        builder.Entity<Curso>()
            .ToTable(t => 
            {
                t.HasCheckConstraint("CK_Curso_Creditos", "Creditos > 0");
                t.HasCheckConstraint("CK_Curso_Horario", "HorarioInicio < HorarioFin");
            });

        // Índice único para evitar doble matrícula de un usuario en un mismo curso
        builder.Entity<Matricula>()
            .HasIndex(m => new { m.CursoId, m.UsuarioId })
            .IsUnique();
    }
}
