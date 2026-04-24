using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalAcademico.Web.Data;
using PortalAcademico.Web.Models;
using PortalAcademico.Web.Models.ViewModels;

namespace PortalAcademico.Web.Controllers;

public class CursosController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index(FiltroCursoViewModel filtros)
    {
        var viewModel = new CatalogoViewModel
        {
            Filtros = filtros
        };

        if (!ModelState.IsValid)
        {
            // Retorna vista vacía de cursos pero con errores del filtro
            return View(viewModel);
        }

        var query = context.Cursos.Where(c => c.Activo).AsQueryable();

        if (!string.IsNullOrWhiteSpace(filtros.Nombre))
        {
            query = query.Where(c => c.Nombre.Contains(filtros.Nombre));
        }

        if (filtros.MinCreditos.HasValue)
        {
            query = query.Where(c => c.Creditos >= filtros.MinCreditos.Value);
        }

        if (filtros.MaxCreditos.HasValue)
        {
            query = query.Where(c => c.Creditos <= filtros.MaxCreditos.Value);
        }

        if (filtros.HorarioInicio.HasValue)
        {
            query = query.Where(c => c.HorarioInicio >= filtros.HorarioInicio.Value);
        }

        if (filtros.HorarioFin.HasValue)
        {
            query = query.Where(c => c.HorarioFin <= filtros.HorarioFin.Value);
        }

        viewModel.Cursos = await query.ToListAsync();

        return View(viewModel);
    }

    public async Task<IActionResult> Details(int id)
    {
        var curso = await context.Cursos.FirstOrDefaultAsync(c => c.Id == id && c.Activo);

        if (curso == null)
        {
            return NotFound();
        }

        int inscritos = await context.Matriculas.CountAsync(m => m.CursoId == id && m.Estado != EstadoMatricula.Cancelada);
        bool estaLleno = inscritos >= curso.CupoMaximo;
        
        bool yaInscrito = false;
        if (User.Identity?.IsAuthenticated == true)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userId))
            {
                yaInscrito = await context.Matriculas.AnyAsync(m => m.CursoId == id && m.UsuarioId == userId && m.Estado != EstadoMatricula.Cancelada);
            }
        }

        var viewModel = new CursoDetalleViewModel
        {
            Curso = curso,
            EstaLleno = estaLleno,
            YaInscrito = yaInscrito,
            InscritosActuales = inscritos
        };

        return View(viewModel);
    }
}
