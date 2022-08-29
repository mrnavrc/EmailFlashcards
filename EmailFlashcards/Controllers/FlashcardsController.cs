using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using EmailFlashcards.Data;
using EmailFlashcards.Models;

namespace EmailFlashcards.Controllers
{
    public class FlashcardsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FlashcardsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Flashcards
        [Authorize]
        public async Task<IActionResult> Index()
        {
              return _context.Flashcards != null ? 
                          View(await _context.Flashcards.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Flashcards'  is null.");
        }

        // GET: Flashcards/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Flashcards == null)
            {
                return NotFound();
            }

            var flashcard = await _context.Flashcards
                .FirstOrDefaultAsync(m => m.FlashcardId == id);
            if (flashcard == null)
            {
                return NotFound();
            }

            return View(flashcard);
        }

        // GET: Flashcards/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Flashcards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Flashcard_Id,Title,Text,CardCreatedDate,FlashcardsCategory")] Flashcard flashcard)
        {
            if (ModelState.IsValid)
            {
                _context.Add(flashcard);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(flashcard);
        }

        // GET: Flashcards/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Flashcards == null)
            {
                return NotFound();
            }

            var flashcard = await _context.Flashcards.FindAsync(id);
            if (flashcard == null)
            {
                return NotFound();
            }
            return View(flashcard);
        }

        // POST: Flashcards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Flashcard_Id,Title,Text,CardCreatedDate,FlashcardsCategory")] Flashcard flashcard)
        {
            if (id != flashcard.FlashcardId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(flashcard);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FlashcardExists(flashcard.FlashcardId))
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
            return View(flashcard);
        }

        // GET: Flashcards/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Flashcards == null)
            {
                return NotFound();
            }

            var flashcard = await _context.Flashcards
                .FirstOrDefaultAsync(m => m.FlashcardId == id);
            if (flashcard == null)
            {
                return NotFound();
            }

            return View(flashcard);
        }

        // POST: Flashcards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Flashcards == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Flashcards'  is null.");
            }
            var flashcard = await _context.Flashcards.FindAsync(id);
            if (flashcard != null)
            {
                _context.Flashcards.Remove(flashcard);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FlashcardExists(int id)
        {
          return (_context.Flashcards?.Any(e => e.FlashcardId == id)).GetValueOrDefault();
        }
    }
}
