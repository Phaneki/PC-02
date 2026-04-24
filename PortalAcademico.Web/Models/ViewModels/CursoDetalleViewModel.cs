namespace PortalAcademico.Web.Models.ViewModels;

public class CursoDetalleViewModel
{
    public required Curso Curso { get; set; }
    public bool EstaLleno { get; set; }
    public bool YaInscrito { get; set; }
    public int InscritosActuales { get; set; }
}
