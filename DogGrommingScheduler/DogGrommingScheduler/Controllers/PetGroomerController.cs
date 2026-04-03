using AplicationLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.PetGroomerDtos;

[ApiController]
[Route("api/[controller]")]
public class PetGroomerController : ControllerBase
{
    private readonly IPetGroomerService _service;
    public PetGroomerController(IPetGroomerService service) => _service = service;

    /// <summary>
    /// Get all the pet groomers in the system.
    /// </summary>
    /// <returns> Return all the pet groomers in the system.</returns>
    /// <response code="200">Returns all the pet groomers in the system.</response>
    /// <response code="400">If there is an error while fetching the pet groomers.</response>
    /// <response code="401">If the user is not authorized to access this endpoint.</response>
    /// <response code="500">If there is an internal server error while fetching the pet groomers.</response>   
    [HttpGet]
    [ProducesResponseType(typeof(PetGroomerDto),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllGroomersAsync();
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
    }
}
