using AplicationLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class PetGroomerController : ControllerBase
{
    private readonly IPetGroomerService _service;
    public PetGroomerController(IPetGroomerService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllGroomersAsync();
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
    }
}
