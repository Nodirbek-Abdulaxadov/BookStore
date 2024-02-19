namespace BookStore.InventoryService.Data.Interfaces;

public interface IBookInterface
{
    Task<IEnumerable<Book>> GetBooks();
    Task<Book> GetBook(string id);
    Task AddBook(AddBook book);
    Task UpdateBook(UpdateBook book);
    Task DeleteBook(string id);
}