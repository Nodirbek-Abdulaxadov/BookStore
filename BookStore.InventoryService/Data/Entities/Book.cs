namespace BookStore.InventoryService.Data.Entities;

public class Book : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Publisher { get; set; } = string.Empty;
    public int Year { get; set; }
    public int Pages { get; set; }
    public string Language { get; set; } = string.Empty;
    public long Barcode { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public List<string> CategoryIds { get; set; } = new();
}