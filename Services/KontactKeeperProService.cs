using Kontact_Keeper_Pro.Data;
using Kontact_Keeper_Pro.Models;
using Kontact_Keeper_Pro.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Kontact_Keeper_Pro.Services
{
    public class KontactKeeperProService : IKontactKeeperProService
    {
        private readonly ApplicationDbContext _context;

        public KontactKeeperProService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task AddCategoriesToContactAsync(IEnumerable<int> categoryIds, int contactId)
        {
            try
            {
                Contact? contact = await _context.Contacts
                                                 .Include(c => c.Categories)
                                                 .FirstOrDefaultAsync(c => c.Id == contactId);

                foreach (int categoryId in categoryIds)
                {
                    Category? category = await _context.Categories.FindAsync(categoryId);

                    if (contact != null && category != null)
                    {
                        contact.Categories.Add(category);
                    }
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task RemoveCategoriesFromContactAsync(int contactId)
        {
            try
            {
                Contact? contact = await _context.Contacts
                                                 .Include(c => c.Categories)
                                                 .FirstOrDefaultAsync(c => c.Id == contactId);

                if (contact != null)
                {
                    contact.Categories.Clear();
                    _context.Update(contact);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
