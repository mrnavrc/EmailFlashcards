﻿using EmailFlashcards.Data;
using EmailFlashcards.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

            var FlashcardsPerDay = settings.Select(settings => settings.FlashcardsPerDay);
            var Time = settings.Select(settings => settings.Time);
            var FlashcardEmailAdress = settings.Select(settings => settings.FlashcardEmailAdress);

            ViewData["FlashcardsPerDay"] = string.Join(";", FlashcardsPerDay);
            //ViewData["Time"] = new TimeOnly(09, 00);
            ViewData["FlashcardEmailAdress"] = string.Join(";", FlashcardEmailAdress);
            return View();
        }


        // POST - save flashcards settings
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> SettingsEdit(FlashcardSetting flashcardsettings)
        {

            flashcardsettings.UserId = _userManager.GetUserId(User);
            bool RowExist = _context.FlashcardsSettings.Where(f => f.UserId == flashcardsettings.UserId)
                                                       .Any();

            if (RowExist is false)
            {
                _context.Add(flashcardsettings);
                await _context.SaveChangesAsync();
            }
            else
            {
                int FindId = _context.FlashcardsSettings
                                     .AsNoTracking()
                                     .Where(f => f.UserId == flashcardsettings.UserId)
                                     .FirstOrDefault()
                                     .FlashcardSettingsId;
                flashcardsettings.FlashcardSettingsId = FindId;
                _context.Update(flashcardsettings);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

    }
}


