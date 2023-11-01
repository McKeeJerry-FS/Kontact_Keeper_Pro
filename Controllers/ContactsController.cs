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

namespace Kontact_Keeper_Pro.Controllers
{
    [Authorize]
    public class ContactsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ContactsController(ApplicationDbContext context, 
                                  UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Contacts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Contacts.Include(c => c.AppUser);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Contacts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Contacts == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts
                .Include(c => c.AppUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // GET: Contacts/Create
        public IActionResult Create()
        {
            //ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,DateOfBirth,Address1,Address2,City,State,ZipCode,Email,PhoneNumber,ImageFile")] Contact contact)
        {
            ModelState.Remove("AppUserId");



            if (ModelState.IsValid)
            {
                contact.AppUserId = _userManager.GetUserId(User);
                contact.Created = DateTime.Now;



                _context.Add(contact);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", contact.AppUserId);
            return View(contact);
        }

        // GET: Contacts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Contacts == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", contact.AppUserId);
            return View(contact);
        }

        // POST: Contacts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AppUserId,FirstName,LastName,Created,Updated,DateOfBirth,Address1,Address2,City,State,ZipCode,Email,PhoneNumber,ImageData,ImageType")] Contact contact)
        {
            if (id != contact.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contact);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactExists(contact.Id))
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
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", contact.AppUserId);
            return View(contact);
        }

        // GET: Contacts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Contacts == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts
                .Include(c => c.AppUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Contacts == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Contacts'  is null.");
            }
            var contact = await _context.Contacts.FindAsync(id);
            if (contact != null)
            {
                _context.Contacts.Remove(contact);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactExists(int id)
        {
            return (_context.Contacts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
