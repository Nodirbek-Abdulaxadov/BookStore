using BookStore.StorageService.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.StorageService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ImagesController(IS3Interface s3Interface)
    : ControllerBase
{
    private readonly IS3Interface s3Interface = s3Interface;

    [HttpPost]
    public async Task<IActionResult> UploadImageAsync(IFormFile file)
    {
        try
        {
            var fileKey = await s3Interface.UploadFileAsync(file);
            return Ok(fileKey);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{fileName}")]
    public async Task<IActionResult> GetImageAsync(string fileName)
    {
        try
        {
            var file = await s3Interface.GetFileUrlAsync(fileName);
            return File(file, "image/jpeg");
        }
        catch(ArgumentNullException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{fileName}")]
    public async Task<IActionResult> DeleteImageAsync(string fileName)
    {
        try
        {
            await s3Interface.DeleteFileAsync(fileName);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}