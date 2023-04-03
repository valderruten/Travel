using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App_Library.Models;

/// <summary>
/// Controlador para manejar la vista de los autores que escribieron los libros y los libros que escribieron los autores.
/// </summary>
namespace App_Library.Controllers
{
    public class AutoresHasLibroesController : Controller
    {
        private readonly DbTravelContext _context;

        /// <summary>
        /// Constructor que inicializa el contexto de la base de datos para AutoresHasLibros.
        /// </summary>
        /// <param name="context">Contexto de la base de datos para AutoresHasLibros.</param>
  
        public AutoresHasLibroesController(DbTravelContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Vista principal de AutoresHasLibros que muestra una lista de autores y los libros que escribieron.
        /// </summary>
        /// <returns>Vista con la lista de autores y los libros que escribieron.</returns>
        
        // GET: AutoresHasLibroes
        public async Task<IActionResult> Index()
        {
            var dbTravelContext = _context.AutoresHasLibros.Include(a => a.Autores).Include(a => a.LibrosIsbnNavigation);
            return View(await dbTravelContext.ToListAsync());
        }

        /// <summary>
        /// Vista de detalles de un autor y un libro específico.
        /// </summary>
        /// <param name="id">ID del libro.</param>
        /// <returns>Vista con los detalles de un autor y un libro específico.</returns>
        
        // GET: AutoresHasLibroes/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.AutoresHasLibros == null)
            {
                return NotFound();
            }

            var autoresHasLibro = await _context.AutoresHasLibros
                .Include(a => a.Autores)
                .Include(a => a.LibrosIsbnNavigation)
                .FirstOrDefaultAsync(m => m.LibrosIsbn == id);
            if (autoresHasLibro == null)
            {
                return NotFound();
            }

            return View(autoresHasLibro);
        }

        /// <summary>
        /// Vista para crear una relación entre un autor y un libro.
        /// </summary>
        /// <returns>Vista para crear una relación entre un autor y un libro.</returns>
    
        // GET: AutoresHasLibroes/Create
        public IActionResult Create()
        {
            ViewData["AutoresId"] = new SelectList(_context.Autores, "Id", "Id");
            ViewData["LibrosIsbn"] = new SelectList(_context.Libros, "Isbn", "Isbn");
            return View();
        }

        /// <summary>
        /// Función para crear una nueva relación entre un autor y un libro en la base de datos.
        /// </summary>
        /// <param name="autoresHasLibro">Relación entre un autor y un libro a crear.</param>
        /// <returns>Vista principal de AutoresHasLibros.</returns>
    
        // POST: AutoresHasLibroes/Create


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AutoresId,LibrosIsbn")] AutoresHasLibro autoresHasLibro)
        {
        
            var autor = await _context.Autores.FindAsync(autoresHasLibro.AutoresId);
            var libro = await _context.Libros.FindAsync(autoresHasLibro.LibrosIsbn);
            if (autor == null || libro == null)
            {
                ModelState.AddModelError("", "El autor o el libro no existe.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(autoresHasLibro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(autoresHasLibro);
        }


        ///<summary>
        /// Método de acción asincrónico que maneja la edición de una relación entre autores y libros.
        /// Recibe como parámetro el ID de la relación a editar.
        /// Si el ID es nulo o no existe en la base de datos, se devuelve un resultado NotFound.
        /// Si se encuentra la relación, se cargan los datos necesarios para la vista de edición y se devuelve la vista con la relación encontrada.
        ///</summary>
        ///<param name="id">El ID de la relación a editar</param>
        ///<returns>Una vista de edición de la relación entre autores y libros</returns>
        ///
        /// // GET: AutoresHasLibroes/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.AutoresHasLibros == null)
            {
                return NotFound();
            }

            var autoresHasLibro = await _context.AutoresHasLibros.FindAsync(id);
            if (autoresHasLibro == null)
            {
                return NotFound();
            }
            ViewData["AutoresId"] = new SelectList(_context.Autores, "Id", "Id", autoresHasLibro.AutoresId);
            ViewData["LibrosIsbn"] = new SelectList(_context.Libros, "Isbn", "Isbn", autoresHasLibro.LibrosIsbn);
            return View(autoresHasLibro);
        }

        ///<summary>
        ///Este método maneja la solicitud HTTP POST para editar una entrada en la tabla AutoresHasLibro en la base de datos.
        ///</summary>
        ///<param name="id">El ID de la entrada que se va a editar</param>
        ///<param name="autoresHasLibro">El objeto AutoresHasLibro que se va a editar</param>
        ///<returns>Una tarea asincrónica que representa el resultado de la operación de edición</returns>
        
        // POST: AutoresHasLibroes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("AutoresId,LibrosIsbn")] AutoresHasLibro autoresHasLibro)
        {
            if (id != autoresHasLibro.LibrosIsbn)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(autoresHasLibro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AutoresHasLibroExists(autoresHasLibro.LibrosIsbn))
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
            ViewData["AutoresId"] = new SelectList(_context.Autores, "Id", "Id", autoresHasLibro.AutoresId);
            ViewData["LibrosIsbn"] = new SelectList(_context.Libros, "Isbn", "Isbn", autoresHasLibro.LibrosIsbn);
            return View(autoresHasLibro);
        }

        ///<summary>
        ///Este método controla la eliminación de un libro de la base de datos.
        ///Se busca un registro en la tabla AutoresHasLibros con el id del libro proporcionado.
        ///Si no se encuentra, se devuelve un error 404.
        ///Si se encuentra, se carga el registro en la vista y se devuelve para su eliminación.
        ///</summary>
        ///<param name="id">El id del libro a eliminar.</param>
        ///<returns>Un objeto IActionResult que representa el resultado de la operación.</returns>
        
        // GET: AutoresHasLibroes/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.AutoresHasLibros == null)
            {
                return NotFound();
            }

            var autoresHasLibro = await _context.AutoresHasLibros
                .Include(a => a.Autores)
                .Include(a => a.LibrosIsbnNavigation)
                .FirstOrDefaultAsync(m => m.LibrosIsbn == id);
            if (autoresHasLibro == null)
            {
                return NotFound();
            }

            return View(autoresHasLibro);
        }

        /// <summary>
        /// Realiza la eliminación confirmada de un registro específico según el ID proporcionado.
        /// </summary>
        /// <param name="id">El ID del registro a eliminar.</param>
        /// <returns>Una acción de tipo Task&lt;IActionResult&gt; que redirige a la vista Index.</returns>

        // POST: AutoresHasLibroes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.AutoresHasLibros == null)
            {
                return Problem("Entity set 'DbTravelContext.AutoresHasLibros'  is null.");
            }
            var autoresHasLibro = await _context.AutoresHasLibros.FindAsync(id);
            if (autoresHasLibro != null)
            {
                _context.AutoresHasLibros.Remove(autoresHasLibro);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Comprueba si existe una relación entre autores y libros para un libro dado en función de su identificador.
        /// </summary>
        /// <param name="id">Identificador del libro a comprobar.</param>
        /// <returns>Valor booleano que indica si existe una relación entre autores y libros para el libro dado.</returns>
       private bool AutoresHasLibroExists(long id)
        {
          return (_context.AutoresHasLibros?.Any(e => e.LibrosIsbn == id)).GetValueOrDefault();
        }
    }
}
