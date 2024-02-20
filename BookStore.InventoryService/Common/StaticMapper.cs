namespace BookStore.InventoryService.Common;

public static class StaticMapper
{
    public static Category MapToCategory(this AddCategory category)
        => new()
            {
                Name = category.Name,
            };

    public static Book MapToBook(this AddBook book)
        => new()
        {
            Title = book.Title,
            Author = book.Author,
            Description = book.Description,
            Price = book.Price,
            Publisher = book.Publisher,
            Year = book.Year,
            Pages = book.Pages,
            Language = book.Language,
            Barcode = book.Barcode,
            ImageUrl = book.ImageUrl,
            CategoryIds = book.CategoryIds,
        };

    public static Book MapToBook(this UpdateBook book)
        => new()
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            Description = book.Description,
            Price = book.Price,
            Publisher = book.Publisher,
            Year = book.Year,
            Pages = book.Pages,
            Language = book.Language,
            Barcode = book.Barcode,
            ImageUrl = book.ImageUrl,
            CategoryIds = book.CategoryIds,
        };
}