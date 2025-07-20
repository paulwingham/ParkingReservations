using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Paul.ParkingReservations.Core.Services;
using Paul.ParkingReservations.Dto;
using Paul.ParkingReservations.Dto.DomainSettings;

namespace Paul.ParkingReservations.Api.Controllers.V1_0;

public class BookingController : BaseApiController
{
    private readonly ILogger<BookingController> _logger;
    private readonly IBookingService _bookingService;
    private readonly ParkingReservationsAppConfigSettings _parkingReservationsAppConfigSettings;

    public BookingController(ILogger<BookingController> logger, IBookingService bookingService, IOptions<ParkingReservationsAppConfigSettings> parkingReservationsAppConfigSettings)
    {
        _logger = logger;
        _bookingService = bookingService;
        _parkingReservationsAppConfigSettings = parkingReservationsAppConfigSettings.Value;
    }

    /// <summary>
    /// Gets available parking slots for a given date range.
    /// </summary>
    /// <param name="startDate">The start date of the range.</param>
    /// <param name="endDate">The end date of the range.</param>
    /// <returns>List of available parking slots.</returns>
    [ProducesResponseType(typeof(List<ParkingSlotsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Produces("application/json", "text/json")]
    [HttpGet]
    [Route("Get")]
    public async Task<IActionResult> GetAvailableParkingSlotsByDateRange(DateTime startDate, DateTime endDate)
    {
        _logger.LogInformation($"{CurrentFunctionMethod.GetCaller(this)} - Started");

        if (startDate == default || endDate == default)
        {
            _logger.LogWarning($"{CurrentFunctionMethod.GetCaller(this)} - Start date or end date is not provided");
            return BadRequest("Start date or end date cannot be empty.");
        }

        try
        {
            var availableParkingSlots = await _bookingService.GetAvailableParkingSlotsByDateRangeAsync(startDate, endDate);
            if (availableParkingSlots == null)
            {
                _logger.LogError($"{CurrentFunctionMethod.GetCaller(this)} - No Available parking slots found for date range" + startDate + " - " + endDate);
                return BadRequest($"No Available parking slots found for date range {startDate} + {endDate}");
            }

            return Ok(availableParkingSlots);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"{CurrentFunctionMethod.GetCaller(this)} - {ex}");
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Creates a new parking booking.
    /// </summary>
    /// <param name="bookingDto">The booking details.</param>
    /// <returns>The created booking.</returns>
    [ProducesResponseType(typeof(BookingDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Consumes("application/json", "text/json")]
    [HttpPost]
    [Route("Post")]
    public async Task<IActionResult> Post(BookingDto bookingDto)
    {
        _logger.LogInformation($"{CurrentFunctionMethod.GetCaller(this)} - Started");

        try
        {
            await _bookingService.InsertBookingAsync(bookingDto);
            _logger.LogInformation($"{CurrentFunctionMethod.GetCaller(this)} - Booking added" + bookingDto);
            return Created(string.Empty, bookingDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"{CurrentFunctionMethod.GetCaller(this)} - {ex}");
            return BadRequest(new { error = ex.Message });
        }
    }
}