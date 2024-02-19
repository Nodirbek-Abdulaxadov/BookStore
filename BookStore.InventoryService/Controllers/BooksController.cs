using BookStore.Core.Helpers;
using Microsoft.Extensions.Caching.Distributed;

namespace BookStore.InventoryService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BooksController(IBookInterface books,
                             IDistributedCache cache)
    : ControllerBase
{
    private readonly IBookInterface _books = books;
    private readonly IDistributedCache _cache = cache;

    private const string _cacheKey = "books";

    [HttpGet]
    public async Task<IActionResult> GetBooks()
    {
        var cachedData = await _cache.GetStringAsync(_cacheKey);
        if (cachedData != null)
        {
            return Ok(cachedData);
        }

        var categories = await _books.GetBooks();
        if (categories.Any())
        {
            await _cache.SetStringAsync(_cacheKey, Json.Serialize(categories));
        }

        var json = Json.Serialize(categories);

        return Ok(json);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBook(string id)
    {
        var book = await _books.GetBook(id);

        if (book == null)
        {
            return NotFound();
        }

        return Ok(book);
    }

    [HttpPost]
    public async Task<IActionResult> AddBook([FromBody] AddBook book)
    {
        if (ModelState.IsValid)
        {
            var categories = await _books.GetBooks();
            if (categories.Any(c => c.Title == book.Title))
            {
                return BadRequest("Book already exists");
            }

            await _books.AddBook(book);

            await _cache.RemoveAsync(_cacheKey);
            return Ok();
        }

        return BadRequest();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateBook([FromBody] UpdateBook book)
    {
        if (ModelState.IsValid)
        {
            var books = await _books.GetBooks();
            if (books.Any(c => c.Title == book.Title && c.Id != book.Id))
            {
                return BadRequest("Book already exists");
            }

            var excitingBook = await _books.GetBook(book.Id);
            if (excitingBook.ImageUrl != book.ImageUrl)
            {
                await DRY(excitingBook.ImageUrl);
            }

            await _books.UpdateBook(book);
            await _cache.RemoveAsync(_cacheKey);
            return Ok();
        }

        return BadRequest();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook(string id)
    {
        var book = await _books.GetBook(id);

        if (book == null)
        {
            return NotFound();
        }

        await DRY(book.ImageUrl);
        await _books.DeleteBook(id);
        await _cache.RemoveAsync(_cacheKey);
        return Ok();
    }

    private async Task DRY(string imageUrl)
    {
        HttpClient client = new();
        var fileName = imageUrl.Split('/').Last();
        var response = await client.DeleteAsync($"https://localhost:7163/api/Images/{fileName}");
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Image deletion failed");
        }
    }
}