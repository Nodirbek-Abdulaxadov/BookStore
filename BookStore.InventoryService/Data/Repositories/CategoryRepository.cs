namespace BookStore.InventoryService.Data.Repositories;

public class CategoryRepository (InventoryDatabase database)
    : ICategoryInterface
{
    private readonly InventoryDatabase _database = database;

    public async Task AddCategory(AddCategory category)
    {
        await _database.Categories.InsertOneAsync(category.MapToCategory());
    }

    public async Task DeleteCategory(string id)
        => await _database.Categories.DeleteOneAsync(category => category.Id == id);

    public async Task<IEnumerable<Category>> GetCategories()
        => await _database.Categories.Find(_ => true).ToListAsync();

    public async Task<Category> GetCategory(string id)
        => await _database.Categories.Find(category => category.Id == id).FirstOrDefaultAsync();

    public Task UpdateCategory(Category category)
        => _database.Categories.ReplaceOneAsync(c => c.Id == category.Id, category);
}