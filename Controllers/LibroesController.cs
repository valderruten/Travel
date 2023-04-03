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
    public class LibroesController : Controller
    {
        private readonly DbTravelContext _context;

        public LibroesController(DbTravelContext context)
        {
            _context = context;
        }

        // GET: Libroes
        public async Task<IActionResult> Index()
        {
            var dbTravelContext = _context.Libros.Include(l => l.Editoriales);
            return View(await dbTravelContext.ToListAsync());
        }

        // GET: Libroes/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.Libros == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros
                .Include(l => l.Editoriales)
                .FirstOrDefaultAsync(m => m.Isbn == id);
            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        // GET: Libroes/Create
        public IActionResult Create()
        {
            ViewData["EditorialesId"] = new SelectList(_context.Editoriales, "Id", "Nombre");
            return View();
        }

        // POST: Libroes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Isbn,EditorialesId,Titulo,Sinopsis,NPaginas")] Libro libro)
        {
            if (ModelState.IsValid)
            {
                _context.Add(libro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EditorialesId"] = new SelectList(_context.Editoriales, "Id", "Id", libro.EditorialesId);
            return View(libro);
        }

        // GET: Libroes/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.Libros == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros.FindAsync(id);
            if (libro == null)
            {
                return NotFound();
            }
            ViewData["EditorialesId"] = new SelectList(_context.Editoriales, "Id", "Nombre", libro.EditorialesId);
            return View(libro);
        }

        // POST: Libroes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Isbn,EditorialesId,Titulo,Sinopsis,NPaginas")] Libro libro)
        {
            if (id != libro.Isbn)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(libro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LibroExists(libro.Isbn))
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
            ViewData["EditorialesId"] = new SelectList(_context.Editoriales, "Id", "Nombre", libro.EditorialesId);
            return View(libro);
        }

        // GET: Libroes/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.Libros == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros
                .Include(l => l.Editoriales)
                .FirstOrDefaultAsync(m => m.Isbn == id);
            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        // POST: Libroes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.Libros == null)
            {
                return Problem("Entity set 'DbTravelContext.Libros'  is null.");
            }
            var libro = await _context.Libros.FindAsync(id);
            if (libro != null)
            {
                _context.Libros.Remove(libro);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LibroExists(long id)
        {
          return (_context.Libros?.Any(e => e.Isbn == id)).GetValueOrDefault();
        }
    }
}
