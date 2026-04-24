using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace PortalAcademico.Web.Models;

public class Matricula
{
    public int Id { get; set; }

    public int CursoId { get; set; }

    [Required]
    public required string UsuarioId { get; set; }

    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

    public EstadoMatricula Estado { get; set; } = EstadoMatricula.Pendiente;

    // Navigation properties
    public Curso? Curso { get; set; }
    public IdentityUser? Usuario { get; set; }
}
