namespace BookStore.InventoryService.Data.Repositories;

public class BookRepository(InventoryDatabase database)
    : IBookInterface
{
    private readonly InventoryDatabase _database = database;

    public async Task AddBook(AddBook book)
        => await _database.Books.InsertOneAsync(book.MapToBook());

    public async Task DeleteBook(string id)
        => await _database.Books.DeleteOneAsync(book => book.Id == id);

    public async Task<Book> GetBook(string id)
        => await _database.Books.Find(book => book.Id == id).FirstOrDefaultAsync();

    public async Task<IEnumerable<Book>> GetBooks()
        => await _database.Books.Find(book => true).ToListAsync();

    public async Task UpdateBook(UpdateBook book)
        => await _database.Books.ReplaceOneAsync( b => b.Id == book.Id, book.MapToBook());
}
