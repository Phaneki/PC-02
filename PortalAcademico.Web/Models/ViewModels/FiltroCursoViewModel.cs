using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PortalAcademico.Web.Models.ViewModels;

public class FiltroCursoViewModel : IValidatableObject
{
    public string? Nombre { get; set; }

    [Display(Name = "Créditos Mínimos")]
    public int? MinCreditos { get; set; }

    [Display(Name = "Créditos Máximos")]
    public int? MaxCreditos { get; set; }

    [Display(Name = "Hora Inicio")]
    public TimeSpan? HorarioInicio { get; set; }

    [Display(Name = "Hora Fin")]
    public TimeSpan? HorarioFin { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (MinCreditos.HasValue && MinCreditos.Value < 0)
        {
            yield return new ValidationResult("Los créditos mínimos no pueden ser negativos.", new[] { nameof(MinCreditos) });
        }

        if (MaxCreditos.HasValue && MaxCreditos.Value < 0)
        {
            yield return new ValidationResult("Los créditos máximos no pueden ser negativos.", new[] { nameof(MaxCreditos) });
        }

        if (MinCreditos.HasValue && MaxCreditos.HasValue && MinCreditos.Value > MaxCreditos.Value)
        {
            yield return new ValidationResult("Los créditos mínimos no pueden ser mayores a los máximos.", new[] { nameof(MinCreditos), nameof(MaxCreditos) });
        }

        if (HorarioInicio.HasValue && HorarioFin.HasValue && HorarioFin.Value <= HorarioInicio.Value)
        {
            yield return new ValidationResult("El Horario Fin no puede ser anterior o igual al Horario Inicio.", new[] { nameof(HorarioInicio), nameof(HorarioFin) });
        }
    }
}
