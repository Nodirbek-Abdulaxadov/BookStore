namespace BookStore.InventoryService.Models.Dtos;

public record BookDto(
    string Id,
    string Title,
    string Author,
    string Description,
    decimal Price,
    string Publisher,
    int Year,
    int Pages,
    string Language,
    long Barcode,
    string ImageUrl,
    List<BookCategoryDto> Categories
);