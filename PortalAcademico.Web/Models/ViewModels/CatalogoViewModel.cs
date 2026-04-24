using System.Collections.Generic;

namespace PortalAcademico.Web.Models.ViewModels;

public class CatalogoViewModel
{
    public FiltroCursoViewModel Filtros { get; set; } = new();
    public List<Curso> Cursos { get; set; } = new();
}
