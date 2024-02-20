using BookStore.InventoryService.Models.Dtos;

namespace BookStore.InventoryService.Models;

public record AddCategory(
    [Required, MinLength(3)]
    string Name
);

public record CategoryDto(
       string Id,
       string Name,
       List<BookDto> Books
);