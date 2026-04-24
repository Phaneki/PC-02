using System;
using System.ComponentModel.DataAnnotations;

namespace PortalAcademico.Web.Models.ViewModels;

public class CursoFormViewModel
{
    public int Id { get; set; }

    [Required]
    [StringLength(20)]
    [Display(Name = "Código")]
    public required string Codigo { get; set; }

    [Required]
    [StringLength(100)]
    public required string Nombre { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Los créditos deben ser mayores a 0.")]
    public int Creditos { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "El cupo máximo debe ser mayor a 0.")]
    [Display(Name = "Cupo Máximo")]
    public int CupoMaximo { get; set; }

    [Display(Name = "Horario Inicio")]
    public TimeSpan HorarioInicio { get; set; }
    
    [Display(Name = "Horario Fin")]
    public TimeSpan HorarioFin { get; set; }

    public bool Activo { get; set; } = true;
}
