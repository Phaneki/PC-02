using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalAcademico.Web.Data;
using PortalAcademico.Web.Models;

namespace PortalAcademico.Web.Controllers;

[Authorize]
public class MatriculasController(ApplicationDbContext context) : Controller
{
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(int CursoId)
    {
        var curso = await context.Cursos.FirstOrDefaultAsync(c => c.Id == CursoId && c.Activo);
        if (curso == null)
        {
            TempData["ErrorMessage"] = "El curso no existe o ya no está disponible.";
            return RedirectToAction("Index", "Cursos");
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Challenge();
        }

        // 1. Validar doble matrícula
        bool yaInscrito = await context.Matriculas
            .AnyAsync(m => m.CursoId == CursoId && m.UsuarioId == userId && m.Estado != EstadoMatricula.Cancelada);
        
        if (yaInscrito)
        {
            TempData["ErrorMessage"] = "Ya te encuentras matriculado o tienes una matrícula pendiente para este curso.";
            return RedirectToAction("Details", "Cursos", new { id = CursoId });
        }

        // 2. Validar Cupo
        int inscritos = await context.Matriculas
            .CountAsync(m => m.CursoId == CursoId && m.Estado != EstadoMatricula.Cancelada);
            
        if (inscritos >= curso.CupoMaximo)
        {
            TempData["ErrorMessage"] = "Lo sentimos, el curso ha alcanzado su cupo máximo.";
            return RedirectToAction("Details", "Cursos", new { id = CursoId });
        }

        // 3. Validar Cruce de Horarios
        var matriculasActuales = await context.Matriculas
            .Include(m => m.Curso)
            .Where(m => m.UsuarioId == userId && m.Estado != EstadoMatricula.Cancelada)
            .ToListAsync();

        bool hayCruce = matriculasActuales.Any(m => 
            m.Curso!.HorarioInicio < curso.HorarioFin && 
            m.Curso.HorarioFin > curso.HorarioInicio);

        if (hayCruce)
        {
            TempData["ErrorMessage"] = "El horario de este curso se cruza con otro curso en el que ya estás matriculado.";
            return RedirectToAction("Details", "Cursos", new { id = CursoId });
        }

        // Todo correcto, crear matrícula
        var nuevaMatricula = new Matricula
        {
            CursoId = CursoId,
            UsuarioId = userId,
            Estado = EstadoMatricula.Pendiente
        };

        context.Matriculas.Add(nuevaMatricula);
        await context.SaveChangesAsync();

        TempData["SuccessMessage"] = $"¡Te has inscrito correctamente al curso '{curso.Nombre}'! Tu matrícula está en estado Pendiente.";
        return RedirectToAction("Details", "Cursos", new { id = CursoId });
    }
}
