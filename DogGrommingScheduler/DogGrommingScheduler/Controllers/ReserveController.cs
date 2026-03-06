using AplicationLogic.DTOs.Reserve;
using AplicationLogic.Interfaces;
using BusinessLogic.Results;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReserveController : ControllerBase
    {
        private readonly IReserveService _reserveService;

        public ReserveController(IReserveService reserveService)
        {
            _reserveService = reserveService;
        }

        /// <summary>
        /// Create Reserve
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateReserveDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _reserveService.ProcessNewReserveAsync(dto);

            if (result.IsFailure)
            {
                return HandleErrorResult(result);
            }

            return Ok(new { Message = "Reserve Created successfully." });
        }

        /// <summary>
        /// Cancel Reserve
        /// </summary>
        [HttpPut("cancel/{id}")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            var result = await _reserveService.CancelReserveAsync(id);

            if (result.IsFailure)
            {
                return HandleErrorResult(result);
            }

            return Ok(new { Message = "Reserve Cancel successfully." });
        }

        private IActionResult HandleErrorResult(Result result)
        {
            if (result.Errors.Any(e => e.Code == "Error.NotFound"))
                return NotFound(result.Errors);

            if (result.Errors.Any(e => e.Code == "Error.Conflict"))
                return Conflict(result.Errors);

            return BadRequest(result.Errors);
        }
    }
}
