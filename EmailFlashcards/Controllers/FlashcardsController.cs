using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmailFlashcards.Data;
using EmailFlashcards.Models;
using Microsoft.AspNetCore.Identity;
using EmailFlashcards.Services.Interfaces;

namespace EmailFlashcards.Controllers
{
    public class FlashcardsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IFlashcardService _flashcardService;

        public FlashcardsController(ApplicationDbContext context,
                                    UserManager<User> userManager,
                                    IFlashcardService flashcardService)
        {
            _context = context;
            _userManager = userManager;
            _flashcardService = flashcardService;
        }

        // GET: Flashcards
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Flashcards.Include(c => c.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Flashcards/Details/5
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
        public async Task<IActionResult> Create()
        {
            string UserId = _userManager.GetUserId(User);
            ViewData["CategoryList"] = new MultiSelectList(await _flashcardService.GetUserCategoriesAsync(UserId), "CategoryId", "FlashcardCategoryName");
            return View();
        }

        // POST: Flashcards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FlashcardId,FlashcardTitle,FlashcardText,FlashcardCreatedDate")] Flashcard flashcard, List<int> CategoryList)
        {
            ModelState.Remove("UserId");

            if (ModelState.IsValid)
            {
              
                flashcard.UserId = _userManager.GetUserId(User);
                flashcard.FlashcardCreatedDate = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
                
                   
                _context.Add(flashcard);
                await _context.SaveChangesAsync();

                // loop over all selected categories

                foreach (int categoryId in CategoryList)
                {
                    await _flashcardService.AddFlashcardToCategoryAsync(categoryId, flashcard.FlashcardId);
                }
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Flashcards/Edit/5
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
        public async Task<IActionResult> Edit(int id, [Bind("FlashcardId,FlashcardTitle,FlashcardText,FlashcardCreatedDate")] Flashcard flashcard)
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
