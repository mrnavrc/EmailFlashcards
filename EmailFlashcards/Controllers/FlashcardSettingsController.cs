using EmailFlashcards.Data;
using EmailFlashcards.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmailFlashcards.Controllers
{
    public class FlashcardSettingsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        public FlashcardSettingsController(ApplicationDbContext context,
                                           UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        // GET - index page Flashcards settings
        public async Task<IActionResult> Index()
        {
            string userId = _userManager.GetUserId(User);
            var settings = new List<FlashcardSetting>();
            settings = await _context.FlashcardsSettings.Where(settings => settings.UserId == userId)
                                                            .ToListAsync();


            ViewData["FlashcardsPerDay"] = "8";
            ViewData["Time"] = "12:00";
            ViewData["FlashcardEmailAdress"] = "mr@seznam.cz";
            return View();
        }


        // POST - save flashcards settings
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> SettingsEdit(int id, [Bind("FlashcardSettingsId, UserId, FlashcardsPerDay, FlashcardEmailAdress, Time")] FlashcardSetting flashcardsettings)
        {

            ModelState.Remove("UserId");
            if (ModelState.IsValid)
            { 
                flashcardsettings.UserId = _userManager.GetUserId(User);
                flashcardsettings.FlashcardSettingsId = 1;
                _context.Update(flashcardsettings);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
