using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using PortalAcademico.Web.Data;
using PortalAcademico.Web.Models;
using PortalAcademico.Web.Models.ViewModels;

namespace PortalAcademico.Web.Controllers;

[Authorize(Roles = "Coordinador")]
public class CoordinadorController(ApplicationDbContext context, IDistributedCache cache) : Controller
{
    public async Task<IActionResult> Index()
    {
        var cursos = await context.Cursos.OrderByDescending(c => c.Activo).ThenBy(c => c.Nombre).ToListAsync();
        return View(cursos);
    }

    public IActionResult Create()
    {
        return View(new CursoFormViewModel() { Codigo = "", Nombre = "" });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CursoFormViewModel model)
    {
        if (model.HorarioInicio >= model.HorarioFin)
        {
            ModelState.AddModelError("HorarioFin", "El horario fin debe ser mayor al horario inicio.");
        }

        if (ModelState.IsValid)
        {
            bool codeExists = await context.Cursos.AnyAsync(c => c.Codigo == model.Codigo);
            if (codeExists)
            {
                ModelState.AddModelError("Codigo", "El código del curso ya existe.");
                return View(model);
            }

            var curso = new Curso
            {
                Codigo = model.Codigo,
                Nombre = model.Nombre,
                Creditos = model.Creditos,
                CupoMaximo = model.CupoMaximo,
                HorarioInicio = model.HorarioInicio,
                HorarioFin = model.HorarioFin,
                Activo = model.Activo
            };

            context.Cursos.Add(curso);
            await context.SaveChangesAsync();
            await cache.RemoveAsync("cache_cursos_activos");

            TempData["SuccessMessage"] = "Curso creado con éxito.";
            return RedirectToAction(nameof(Index));
        }
        return View(model);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var curso = await context.Cursos.FindAsync(id);
        if (curso == null) return NotFound();

        var model = new CursoFormViewModel
        {
            Id = curso.Id,
            Codigo = curso.Codigo,
            Nombre = curso.Nombre,
            Creditos = curso.Creditos,
            CupoMaximo = curso.CupoMaximo,
            HorarioInicio = curso.HorarioInicio,
            HorarioFin = curso.HorarioFin,
            Activo = curso.Activo
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CursoFormViewModel model)
    {
        if (id != model.Id) return NotFound();

        if (model.HorarioInicio >= model.HorarioFin)
        {
            ModelState.AddModelError("HorarioFin", "El horario fin debe ser mayor al horario inicio.");
        }

        if (ModelState.IsValid)
        {
            bool codeExists = await context.Cursos.AnyAsync(c => c.Codigo == model.Codigo && c.Id != model.Id);
            if (codeExists)
            {
                ModelState.AddModelError("Codigo", "El código del curso ya pertenece a otro curso.");
                return View(model);
            }

            var curso = await context.Cursos.FindAsync(id);
            if (curso == null) return NotFound();

            curso.Codigo = model.Codigo;
            curso.Nombre = model.Nombre;
            curso.Creditos = model.Creditos;
            curso.CupoMaximo = model.CupoMaximo;
            curso.HorarioInicio = model.HorarioInicio;
            curso.HorarioFin = model.HorarioFin;
            curso.Activo = model.Activo;

            await context.SaveChangesAsync();
            await cache.RemoveAsync("cache_cursos_activos");

            TempData["SuccessMessage"] = "Curso actualizado con éxito.";
            return RedirectToAction(nameof(Index));
        }
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleActivo(int id)
    {
        var curso = await context.Cursos.FindAsync(id);
        if (curso == null) return NotFound();

        curso.Activo = !curso.Activo;
        await context.SaveChangesAsync();
        await cache.RemoveAsync("cache_cursos_activos");

        TempData["SuccessMessage"] = $"Curso {(curso.Activo ? "activado" : "desactivado")} con éxito.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Matriculas(int cursoId)
    {
        var curso = await context.Cursos.FindAsync(cursoId);
        if (curso == null) return NotFound();

        ViewBag.CursoNombre = curso.Nombre;
        ViewBag.CursoId = cursoId;

        var matriculas = await context.Matriculas
            .Include(m => m.Usuario)
            .Where(m => m.CursoId == cursoId)
            .OrderByDescending(m => m.FechaRegistro)
            .ToListAsync();

        return View(matriculas);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CambiarEstadoMatricula(int matriculaId, EstadoMatricula estado)
    {
        var matricula = await context.Matriculas.FindAsync(matriculaId);
        if (matricula == null) return NotFound();

        matricula.Estado = estado;
        await context.SaveChangesAsync();

        TempData["SuccessMessage"] = $"Estado de matrícula cambiado a {estado}.";
        return RedirectToAction(nameof(Matriculas), new { cursoId = matricula.CursoId });
    }
}
