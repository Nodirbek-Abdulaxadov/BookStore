using BookStore.InventoryService.Models.Dtos;

namespace BookStore.InventoryService.Common;

public class Mapper
{
    private readonly IBookInterface _bookInterface;
    private readonly ICategoryInterface _categoryInterface;
    private readonly IEnumerable<Category> _categories;
    private readonly IEnumerable<Book> _books;

    public Mapper(IBookInterface bookInterface,
                  ICategoryInterface categoryInterface)
    {
        _bookInterface = bookInterface;
        _categoryInterface = categoryInterface;
        _categories = _categoryInterface.GetCategories().Result;
        _books = _bookInterface.GetBooks().Result;
    }

    public BookDto MapToBookDto(Book book)
        => new BookDto(book.Id, book.Title,
                        book.Author, book.Description,
                        book.Price, book.Publisher,
                        book.Year, book.Pages,
                        book.Language, book.Barcode,
                        book.ImageUrl,
                        _categories.Where(c => book.CategoryIds
                                                    .Contains(c.Id))
                                                    .Select(c => (BookCategoryDto)c)
                                                    .ToList());

    public CategoryDto MapToCategoryDto(Category category)
        => new CategoryDto(category.Id, category.Name,
                           _books.Where(b => b.CategoryIds
                                              .Any(categoryId => categoryId == category.Id))
                                              .Select(book => MapToBookDto(book)).ToList()
                          );
}