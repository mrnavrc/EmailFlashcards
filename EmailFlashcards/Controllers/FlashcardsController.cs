using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmailFlashcards.Data;
using EmailFlashcards.Models;
using Microsoft.AspNetCore.Identity;
using EmailFlashcards.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using X.PagedList;
using Org.BouncyCastle.Bcpg;

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
        [Authorize]
        public IActionResult Index(int categoryId, int? page, string SuccessMessage = null, string DeleteAction = null)
        {
            ViewData["DeleteAction"] = DeleteAction;
            ViewData["SuccessMessage"] = SuccessMessage;

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            string UserId = _userManager.GetUserId(User);

            User user = _context.Users
                                .Include(c => c.Flashcards)
                                .ThenInclude(c => c.Categories)
                                .FirstOrDefault(u => u.Id == UserId);
            var categories = user.Categories;
            ViewData["CategoryList"] = new SelectList(categories, "CategoryId", "FlashcardCategoryName", categoryId);

            if (categoryId == 0)
            {
                var flashcards = _context.Flashcards.Where(f => f.UserId == UserId)
                                                .OrderByDescending(f => f.FlashcardCreatedDate)
                                                .ToPagedList(pageNumber, pageSize);
                return View(flashcards);
            }
            else
            {
                var flashcards = _context.Categories.FirstOrDefault(c => c.CategoryId == categoryId)
                                  .Flashcards.OrderByDescending(f => f.FlashcardCreatedDate)
                                  .ToPagedList(pageNumber, pageSize);
                return View(flashcards);
            }

           
           
        }


        // POST: SearchFlashcards$

        [Authorize]
        public IActionResult SearchFlashcard(string searchString)
        {
            string userId = _userManager.GetUserId(User);
            User user = _context.Users
                                .Include(_context => _context.Flashcards)
                                .ThenInclude(_context => _context.Categories)
                                .FirstOrDefault(_context => _context.Id == userId);
            var flashcards = new List<Flashcard>();
            if(String.IsNullOrEmpty(searchString))
            {
                return View(nameof(Index), flashcards);
            }
            else
            {
                flashcards = user.Flashcards.Where(flashcard => flashcard.FlashcardText!.ToLower().Contains(searchString.ToLower()))
                                            .ToList();
            }

            return View(nameof(Index), flashcards);
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
        public async Task<IActionResult> Create()
        {
            string UserId = _userManager.GetUserId(User);
            ViewData["CategoryList"] = new MultiSelectList(await _flashcardService.GetUserCategoriesAsync(UserId), "CategoryId", "FlashcardCategoryName");
            return View();
        }

        // POST: Flashcards/Create
        [Authorize]
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
                return RedirectToAction(nameof(Index), new { SuccessMessage = "succes" });
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Flashcards/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            string UserId = _userManager.GetUserId(User);
            var flashcard = await _context.Flashcards.Where(f => f.FlashcardId == id && f.UserId == UserId)
                                               .FirstOrDefaultAsync();

 
            ViewData["CategoryList"] = new MultiSelectList(await _flashcardService.GetUserCategoriesAsync(UserId), "CategoryId", "FlashcardCategoryName", await _flashcardService.GetFlashcardCategoryIdAsync(flashcard.FlashcardId));
        
            return View(flashcard);
        }

        // POST: Flashcards/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("FlashcardId, FlashcardTitle, FlashcardText")] Flashcard flashcard, List<int> CategoryList)
        {
            string UserId = _userManager.GetUserId(User);
            if (ModelState.IsValid)
            {
                try
                {
                    flashcard.UserId = _userManager.GetUserId(User);
                    flashcard.FlashcardCreatedDate = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
                    _context.Update(flashcard);
                    await _context.SaveChangesAsync();

                    //remove the current categories
                    List<Category> oldCategories = (await _flashcardService.GetFlashcardCategoriesAsync(flashcard.FlashcardId)).ToList();
                    foreach (var category in oldCategories)
                    {
                        await _flashcardService.RemoveFlashcardFromCategoryAsync(category.CategoryId, flashcard.FlashcardId);
                    }
                    //add the selected categories
                    foreach (int categoryid in CategoryList)
                    {
                        await _flashcardService.AddFlashcardToCategoryAsync(categoryid, flashcard.FlashcardId);
                    }

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
                return RedirectToAction(nameof(Index), new { SuccessMessage = "succes" });
            }
            ViewData["CategoryList"] = new MultiSelectList(await _flashcardService.GetUserCategoriesAsync(UserId), "CategoryId", "FlashcardCategoryName", await _flashcardService.GetFlashcardCategoryIdAsync(flashcard.FlashcardId));
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
        [Authorize]
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
            return RedirectToAction(nameof(Index), new { DeleteAction = "Delete" });
        }

        public FileResult ExportCsv()
        {
            string userId = _userManager.GetUserId(User);
            
            var flashcardList = _context.Flashcards.Where(f => f.UserId == userId)
                                                   .ToList();
                                            
            return File(flashcardList, "flashcards.csv");
        }
        public virtual FlashcardCsvResult File(IEnumerable<Flashcard> flashcardList, string fileDownloadName)
        {
            return new FlashcardCsvResult(flashcardList, fileDownloadName);
        }

        private bool FlashcardExists(int id)
        {
          return (_context.Flashcards?.Any(e => e.FlashcardId == id)).GetValueOrDefault();
        }


       
    }
}
