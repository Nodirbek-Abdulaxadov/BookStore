namespace BookStore.InventoryService.Models;

public record AddBook(
    [Required, MinLength(3)]
    string Title,
    [Required, MinLength(3)]
    string Author,
    string Description,
    [Required]
    decimal Price,
    [Required]
    string Publisher,
    [Required]
    int Year,
    [Required]
    int Pages,
    [Required]
    string Language,
    [Required]
    long Barcode,
    [Required]
    string ImageUrl,
    [Required]
    List<string> CategoryIds    
);

public record UpdateBook(
    [Required]
    string Id,
    [Required, MinLength(3)]
    string Title,
    [Required, MinLength(3)]
    string Author,
    string Description,
    [Required]
    decimal Price,
    [Required]
    string Publisher,
    [Required]
    int Year,
    [Required]
    int Pages,
    [Required]
    string Language,
    [Required]
    long Barcode,
    [Required]
    string ImageUrl,
    [Required]
    List<string> CategoryIds
);