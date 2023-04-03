using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App_Library.Models;

///<summary>
/// Controlador para la biblioteca de aplicaciones.
///</summary>

namespace App_Library.Controllers
{
 ///<summary>
///Controlador que maneja las operaciones relacionadas con las editoriales en la aplicación.
///</summary>
public class EditorialesController : Controller
    {
        private readonly DbTravelContext _context;

        public EditorialesController(DbTravelContext context)
        {
            _context = context;
        }

        ///<summary>
        ///Este método devuelve una vista de todas las editoriales presentes en la base de datos.
        ///Si no hay editoriales en la base de datos, se genera un error de problema.
        ///</summary>
        ///<returns>Una vista de todas las editoriales presentes en la base de datos o un error de problema si no hay editoriales.</returns>
        // GET: Editoriales
        public async Task<IActionResult> Index()
        {
              return _context.Editoriales != null ? 
                          View(await _context.Editoriales.ToListAsync()) :
                          Problem("Entity set 'DbTravelContext.Editoriales'  is null.");
        }

        ///<summary>
        ///Método que muestra los detalles de una editorial en particular según su ID.
        ///</summary>
        ///<param name="id">El ID de la editorial</param>
        ///<returns>Retorna una vista con los detalles de la editorial o un mensaje de error si no se encuentra la editorial o si el ID es nulo.</returns>
        
        // GET: Editoriales/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Editoriales == null)
            {
                return NotFound();
            }

            var editoriale = await _context.Editoriales
                .FirstOrDefaultAsync(m => m.Id == id);
            if (editoriale == null)
            {
                return NotFound();
            }

            return View(editoriale);
        }

        /// <summary>
        /// Este método devuelve una vista de creación para la acción "Create" en el controlador.
        /// </summary>
        /// <returns>Vista de creación</returns>
        // GET: Editoriales/Create
        public IActionResult Create()
        {
            return View();
        }

        ///<summary>
        ///Este método HTTP POST crea una nueva instancia de la clase Editoriale en la base de datos.
        ///Se valida si el modelo es válido y, en caso afirmativo, se agrega la instancia a la base de datos y se redirige a la acción "Index".
        ///Si el modelo no es válido, se muestra la vista "Create" con la instancia no válida.
        ///</summary>
        ///<param name="editoriale">La instancia de la clase Editoriale que se creará en la base de datos.</param>
        ///<returns>La vista "Index" si el modelo es válido y se guarda correctamente en la base de datos. De lo contrario, la vista "Create" con la instancia no válida.</returns>
        // POST: Editoriales/Create 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Sede")] Editoriale editoriale)
        {
            if (ModelState.IsValid)
            {
                _context.Add(editoriale);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(editoriale);
        }

        ///<summary>
        ///Método que muestra la vista para editar una editorial
        ///</summary>
        ///<param name="id">Identificador de la editorial a editar</param>
        ///<returns>Retorna una vista con los datos de la editorial a editar</returns>
        // GET: Editoriales/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Editoriales == null)
            {
                return NotFound();
            }

            var editoriale = await _context.Editoriales.FindAsync(id);
            if (editoriale == null)
            {
                return NotFound();
            }
            return View(editoriale);
        }

        /// <summary>
        /// Acción para editar una editorial mediante una solicitud HTTP POST.
        /// </summary>
        /// <param name="id">Identificador de la editorial a editar.</param>
        /// <param name="editoriale">Objeto de la clase Editoriale con los datos de la editorial a editar.</param>
        /// <returns>Una tarea asincrónica que devuelve un objeto IActionResult.</returns>
        // POST: Editoriales/Edit/5 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Sede")] Editoriale editoriale)
        {
            if (id != editoriale.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(editoriale);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EditorialeExists(editoriale.Id))
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
            return View(editoriale);
        }

        ///<summary>
        ///Método asincrónico que elimina un registro de la tabla "Editoriales" a partir de su ID.
        ///</summary>
        ///<param name="id">Identificador del registro a eliminar.</param>
        ///<returns>Devuelve una vista con los detalles del registro eliminado o un error de "no encontrado" si no se encontró el registro.</returns>
        // GET: Editoriales/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Editoriales == null)
            {
                return NotFound();
            }

            var editoriale = await _context.Editoriales
                .FirstOrDefaultAsync(m => m.Id == id);
            if (editoriale == null)
            {
                return NotFound();
            }

            return View(editoriale);
        }

        ///<summary>
        /// Acción HttpPost que elimina un registro de la tabla Editoriales, dado su identificador único.
        ///</summary>
        /// <param name="id">Identificador único del registro a eliminar.</param>
        /// <returns>Una tarea asincrónica que representa la operación de eliminación y redirige al usuario a la acción Index.</returns>
        // POST: Editoriales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Editoriales == null)
            {
                return Problem("Entity set 'DbTravelContext.Editoriales'  is null.");
            }
            var editoriale = await _context.Editoriales.FindAsync(id);
            if (editoriale != null)
            {
                _context.Editoriales.Remove(editoriale);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Comprueba si existe una editorial en la base de datos con el id especificado.
        /// </summary>
        /// <param name="id">El id de la editorial a comprobar.</param>
        /// <returns>Devuelve un valor booleano que indica si existe o no una editorial con el id especificado.</returns>
        private bool EditorialeExists(int id)
        {
          return (_context.Editoriales?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
