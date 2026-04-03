using Shared.DTOs;
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
        /// Create a new reservation.
        /// </summary>
        /// <remarks>
        /// Processes a reservation request for a pet grooming appointment. Returns a success message on success.
        /// </remarks>
        /// <param name="dto">Reservation details including user, groomer, date, and service info.</param>
        /// <returns>A confirmation message when the reservation is created.</returns>
        /// <response code="200">Reservation created successfully.</response>
        /// <response code="400">Invalid request data or business validation failed.</response>
        /// <response code="404">Referenced resource not found.</response>
        /// <response code="409">Reservation conflict or business rule prevented creation.</response>
        [HttpPost]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(object), StatusCodes.Status409Conflict)]
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
        /// Cancel an existing reservation.
        /// </summary>
        /// <remarks>
        /// Cancels the reservation identified by the given id. Returns a confirmation message on success.
        /// </remarks>
        /// <param name="id">The unique identifier of the reservation to cancel.</param>
        /// <returns>A confirmation message when the reservation is canceled.</returns>
        /// <response code="200">Reservation canceled successfully.</response>
        /// <response code="400">Bad request or validation error.</response>
        /// <response code="404">Reservation not found.</response>
        /// <response code="409">Cancellation conflict due to business rules.</response>
        [HttpPut("cancel/{id}")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(object), StatusCodes.Status409Conflict)]
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
        /// <summary>
        /// Get a groomer's schedule for a specific date.
        /// </summary>
        /// <remarks>
        /// Returns available and booked time slots for the specified groomer on the given date.
        /// </remarks>
        /// <param name="groomerId">The groomer's unique identifier.</param>
        /// <param name="date">The date to retrieve the schedule for (YYYY-MM-DD).</param>
        /// <returns>The schedule data for the groomer on the requested date.</returns>
        /// <response code="200">Schedule retrieved successfully.</response>
        /// <response code="400">Invalid request data.</response>
        /// <response code="404">Groomer or schedule not found.</response>
        /// <response code="409">Conflict while retrieving schedule.</response>
        [HttpGet("schedule/{groomerId}/{date}")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(object), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> GetSchedule(Guid groomerId, DateTime date)
        {
            var result = await _reserveService.GetScheduleForReservationAsync(groomerId, date);

            if (result.IsFailure)
                return HandleErrorResult(result);

            return Ok(result.Value);
        }
        /// <summary>
        /// Get reservations for a user.
        /// </summary>
        /// <remarks>
        /// Returns the list of reservations associated with the given user id.
        /// </remarks>
        /// <param name="userId">The user's unique identifier.</param>
        /// <returns>A list of the user's reservations.</returns>
        /// <response code="200">Reservations retrieved successfully.</response>
        /// <response code="400">Invalid request or retrieval error.</response>
        [HttpGet("my-reserves/{userId}")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetMyReserves(Guid userId)
        {
            var result = await _reserveService.GetUserReservesAsync(userId);

            if (result.IsFailure) return BadRequest(result.Errors);

            return Ok(result.Value);
        }
    }
}
