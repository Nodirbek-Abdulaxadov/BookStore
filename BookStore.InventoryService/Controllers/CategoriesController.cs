namespace BookStore.InventoryService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController(ICategoryInterface categoryInterface)
    : ControllerBase
{
    private readonly ICategoryInterface _categories = categoryInterface;

    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _categories.GetCategories();
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategory(string id)
    {
        var category = await _categories.GetCategory(id);
        return Ok(category);
    }

    [HttpPost]
    public async Task<IActionResult> AddCategory([FromBody] AddCategory category)
    {
        await _categories.AddCategory(category);
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateCategory([FromBody] Category category)
    {
        await _categories.UpdateCategory(category);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(string id)
    {
        await _categories.DeleteCategory(id);
        return Ok();
    }
}