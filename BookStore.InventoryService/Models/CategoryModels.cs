namespace BookStore.InventoryService.Models;

public record AddCategory(
    [Required, MinLength(3)]
    string Name
);