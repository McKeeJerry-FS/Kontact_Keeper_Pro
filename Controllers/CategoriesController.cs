using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Kontact_Keeper_Pro.Data;
using Kontact_Keeper_Pro.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Kontact_Keeper_Pro.Controllers
{
    [Authorize]
    public class CategoriesController : Controller
    {
        #region Properties
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailSender _emailService;

        public CategoriesController(ApplicationDbContext context,
                                    UserManager<AppUser> userManager,
                                    IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _emailService = emailSender;
        }

        #endregion


        #region GET: Categories
        // GET: Categories
        public async Task<IActionResult> Index()
        {
            string? userId = _userManager.GetUserId(User);

            IEnumerable<Category> categories = await _context.Categories.Where(c => c.AppUserId == userId)
                                                                        .ToListAsync();
            return View(categories);
        }

        #endregion


        #region GET: Categories/Details
        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.AppUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        #endregion


        #region GET: Categories/Create
        // GET: Categories/Create
        public IActionResult Create()
        {
            ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        #endregion


        #region POST: Categories/Create
        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Category category)
        {
            ModelState.Remove("AppUserId");

            if (ModelState.IsValid)
            {
                category.AppUserId = _userManager.GetUserId(User);


                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", category.AppUserId);
            return View(category);
        }

        #endregion


        #region GET: Categories/Edit
        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", category.AppUserId);
            return View(category);
        }

        #endregion


        #region POST: Categories/Edit
        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AppUserId,Name")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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
            string? userId = _userManager.GetUserId(User);


            ViewData["AppUserId"] = new SelectList(_context.Categories.Where(c => c.AppUserId == userId), "Id", "Name", category.AppUserId);
            return View(category);
        }

        #endregion

        #region GET: EmailCategory
        [HttpGet]
        public async Task<IActionResult> EmailCategory(int? id, string? swalMessage)
        {
            if (id == null)
            {
                return NotFound();
            }

            //ViewData["SwalMessage"] = swalMessage;

            string? userId = _userManager.GetUserId(User);
            Category? category = await _context.Categories.Include(c => c.Contacts)
                                                          .FirstOrDefaultAsync(c => c.Id == id && c.AppUserId == userId);

            if (category == null)
            {
                return NotFound();
            }

            IEnumerable<string?> emails = category.Contacts.Select(c => c.Email);
            EmailData emailData = new()
            {
                GroupName = category.Name,
                EmailAddress = string.Join(";", emails),
                EmailSubject = $"Group Message: For {category.Name}! **** Eyes Only ****"
            };
            ViewData["EmailContacts"] = category.Contacts.ToList();
            return View(emailData);
        }

        #endregion

        #region POST: EmailCategory
        [HttpPost]
        public async Task<IActionResult> EmailCategory(EmailData emailData, int? id)
        {
            string? swalMessage = string.Empty;
            if (ModelState.IsValid)
            {
                // SweetAlert
                try
                {
                    string? email = emailData.EmailAddress;
                    string? subject = emailData.EmailSubject;
                    string? htmlMessage = emailData.EmailBody;

                    // call email service
                    await _emailService.SendEmailAsync(email!, subject!, htmlMessage!);
                    swalMessage = "Success!! Email Sent!";




                }
                catch (Exception)
                {
                    swalMessage = "Error!!! Email Failed to send!";
                    throw;
                }
            }


            string? userId = _userManager.GetUserId(User);
            Category? category = await _context.Categories.Include(c => c.Contacts)
                                                          .FirstOrDefaultAsync(c => c.Id == id && c.AppUserId == userId);

            ViewData["EmailContacts"] = category.Contacts.ToList();

            // testing
            ViewData["SwalMessage"] = swalMessage;

            return View(emailData);
        }

        #endregion
        #region GET: Categories/Delete
        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.AppUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        #endregion


        #region POST: Categories/Delete
        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Categories == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Categories'  is null.");
            }
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        #endregion


        #region CategoriesExists
        private bool CategoryExists(int id)
        {
            return (_context.Categories?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        #endregion    
    }
}
