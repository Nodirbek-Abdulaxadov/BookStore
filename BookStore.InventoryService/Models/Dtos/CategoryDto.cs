namespace BookStore.InventoryService.Models.Dtos;

public class BookCategoryDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

    public static implicit operator BookCategoryDto(Category category)
        => new()
        {
            Id = category.Id,
            Name = category.Name
        };
}