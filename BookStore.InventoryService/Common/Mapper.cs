namespace BookStore.InventoryService.Common;

public static class Mapper
{
    public static Category MapToCategory(this AddCategory category)
        => new()
            {
                Name = category.Name,
            };
}