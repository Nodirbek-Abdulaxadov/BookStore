using BookStore.Core.Helpers;
using Microsoft.Extensions.Caching.Distributed;

namespace BookStore.InventoryService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController(ICategoryInterface categoryInterface,
                                  IDistributedCache cache)
    : ControllerBase
{
    private readonly ICategoryInterface _categories = categoryInterface;
    private readonly IDistributedCache _cache = cache;
    private const string _cacheKey = "categories";

    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        var cachedData = await _cache.GetStringAsync(_cacheKey);
        if (cachedData != null)
        {
            return Ok(cachedData);
        }

        var categories = await _categories.GetCategories();
        if (categories.Any())
        { 
            await _cache.SetStringAsync(_cacheKey, Json.Serialize(categories));
        }

        var json = Json.Serialize(categories);

        return Ok(json);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategory(string id)
    {
        var category = await _categories.GetCategory(id);

        if (category == null)
        {
            return NotFound();
        }

        return Ok(category);
    }

    [HttpPost]
    public async Task<IActionResult> AddCategory([FromBody] AddCategory category)
    {
        if (ModelState.IsValid)
        {
            var categories = await _categories.GetCategories();
            if (categories.Any(c => c.Name == category.Name))
            {
                return BadRequest("Category already exists");
            }

            await _categories.AddCategory(category);

            await _cache.RemoveAsync(_cacheKey);
            return Ok();
        }

        return BadRequest();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateCategory([FromBody] Category category)
    {
        if (ModelState.IsValid)
        {
            var categories = await _categories.GetCategories();
            if (categories.Any(c => c.Name == category.Name && c.Id != category.Id))
            {
                return BadRequest("Category already exists");
            }

            await _categories.UpdateCategory(category);
            await _cache.RemoveAsync(_cacheKey);
            return Ok();
        }

        return BadRequest();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(string id)
    {
        var category = await _categories.GetCategory(id);

        if (category == null)
        {
            return NotFound();
        }

        await _categories.DeleteCategory(id);
        await _cache.RemoveAsync(_cacheKey);
        return Ok();
    }
}