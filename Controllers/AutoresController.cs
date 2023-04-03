using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App_Library.Models;

namespace App_Library.Controllers
{
    /// <summary>
    /// Constructor que inicializa el contexto de base de datos a través de una inyección de dependencia.
    /// </summary>
    /// <param name="context">Contexto de la base de datos.</param>
    public class AutoresController : Controller
    {
        private readonly DbTravelContext _context;

        public AutoresController(DbTravelContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Acción que devuelve una vista que muestra una lista de autores en la base de datos.
        /// </summary>
        /// <returns>Vista con la lista de autores.</returns>
        // GET: Autores
        public async Task<IActionResult> Index()
        {
              return _context.Autores != null ? 
                          View(await _context.Autores.ToListAsync()) :
                          Problem("Entity set 'DbTravelContext.Autores'  is null.");
        }

        /// <summary>
        /// Acción que muestra los detalles de un autor en particular.
        /// </summary>
        /// <param name="id">Identificador del autor.</param>
        /// <returns>Vista con los detalles del autor o NotFound si el autor no existe.</returns>

        // GET: Autores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Autores == null)
            {
                return NotFound();
            }

            var autore = await _context.Autores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (autore == null)
            {
                return NotFound();
            }

            return View(autore);
        }

        /// <summary>
        /// Acción que devuelve una vista para crear un nuevo autor.
        /// </summary>
        /// <returns>Vista para crear un nuevo autor.</returns>
        // GET: Autores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Autores/Create
        /// <summary>
        /// Acción que se ejecuta al enviar el formulario para crear un nuevo autor.
        /// </summary>
        /// <param name="autore">Autor que se va a crear.</param>
        /// <returns>Redirecciona a la acción Index si el autor se crea correctamente o muestra la vista Create con los errores de validación si hay algún problema.</returns>


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Apellidos")] Autore autore)
        {
            if (ModelState.IsValid)
            {
                _context.Add(autore);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(autore);
        }


        /// <summary>
        /// Acción que devuelve una vista para editar un autor existente.
        /// </summary>
        /// <param name="id">Identificador del autor.</param>
        /// <returns>Vista para editar el autor o NotFound si el autor no existe.</returns>

        // GET: Autores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Autores == null)
            {
                return NotFound();
            }

            var autore = await _context.Autores.FindAsync(id);
            if (autore == null)
            {
                return NotFound();
            }
            return View(autore);
        }

        /// <summary>
        /// Edita un autor en la base de datos.
        /// </summary>
        /// <param name="id">Identificador del autor a editar.</param>
        /// <param name="autore">Modelo de autor a actualizar.</param>
        /// <returns>Una tarea asincrónica que representa la operación y devuelve una acción del controlador.</returns>

        // POST: Autores/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Apellidos")] Autore autore)
        {
            if (id != autore.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(autore);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AutoreExists(autore.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(autore);
        }

        /// <summary>
        /// Muestra la vista para eliminar un autor.
        /// </summary>
        /// <param name="id">Identificador del autor a eliminar.</param>
        /// <returns>Una tarea asincrónica que representa la operación y devuelve una acción del controlador.</returns>

        // GET: Autores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Autores == null)
            {
                return NotFound();
            }

            var autore = await _context.Autores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (autore == null)
            {
                return NotFound();
            }

            return View(autore);
        }

        /// <summary>
        /// Elimina un autor de la base de datos.
        /// </summary>
        /// <param name="id">Identificador del autor a eliminar.</param>
        /// <returns>Una tarea asincrónica que representa la operación y devuelve una acción del controlador.</returns>

        // POST: Autores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Autores == null)
            {
                return Problem("Entity set 'DbTravelContext.Autores'  is null.");
            }
            var autore = await _context.Autores.FindAsync(id);
            if (autore != null)
            {
                _context.Autores.Remove(autore);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Verifica si un autor con el identificador especificado existe en la base de datos.
        /// </summary>
        /// <param name="id">Identificador del autor a buscar.</param>
        /// <returns>Verdadero si el autor existe, falso en caso contrario.</returns>

        private bool AutoreExists(int id)
        {
          return (_context.Autores?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
