namespace BookStore.InventoryService.Data.Interfaces;

public interface ICategoryInterface
{
    Task<IEnumerable<Category>> GetCategories();
    Task<Category> GetCategory(string id);
    Task AddCategory(AddCategory category);
    Task UpdateCategory(Category category);
    Task DeleteCategory(string id);
}