using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PortalAcademico.Web.Models;

public class Curso
{
    public int Id { get; set; }

    [Required]
    [StringLength(20)]
    public required string Codigo { get; set; }

    [Required]
    [StringLength(100)]
    public required string Nombre { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Los créditos deben ser mayores a 0.")]
    public int Creditos { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "El cupo máximo debe ser mayor a 0.")]
    public int CupoMaximo { get; set; }

    public TimeSpan HorarioInicio { get; set; }
    
    public TimeSpan HorarioFin { get; set; }

    public bool Activo { get; set; }

    // Navigation properties
    public ICollection<Matricula>? Matriculas { get; set; }
}
