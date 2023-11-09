namespace Kontact_Keeper_Pro.Services.Interfaces
{
    public interface IKontactKeeperProService
    {
        public Task AddCategoriesToContactAsync(IEnumerable<int> categoryIds, int contactId);
        public Task RemoveCategoriesFromContactAsync(int contactId);
    }
}
