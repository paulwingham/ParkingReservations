using System.Net;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Paul.ParkingReservations.Core.Services;
using Paul.ParkingReservations.Dto;
using Paul.ParkingReservations.Dto.DomainSettings;

namespace Paul.ParkingReservations.Api.Controllers.V1_0;

[Route("api/[controller]")]
[ApiVersion("1.0")]
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

    [ProducesResponseType(typeof(List<ParkingSlotsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet]
    [Route("Get")]
    public async Task<IActionResult> GetAvailableParkingSlotsByDateRange(DateTime startDate, DateTime endDate)
    {
        _logger.LogDebug($"{CurrentFunctionMethod.GetCaller(this)} - Started");

        try
        {
            var availableParkingSlots = await _bookingService.GetAvailableParkingSlotsByDateRangeAsync(startDate, endDate);
            if (availableParkingSlots == null)
            {
                _logger.LogError($"{CurrentFunctionMethod.GetCaller(this)} - No Available parking slots found for date range" + startDate + " - " + endDate);
                return BadRequest();
            }

            return Ok(availableParkingSlots);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"{CurrentFunctionMethod.GetCaller(this)} - Error occurred while getting available parking slots");
            return BadRequest();
        }
    }

    [HttpPost]
    [Route("Post")]
    public async Task<IActionResult> Post(BookingDto bookingDto)
    {
        try
        {
            await _bookingService.InsertBookingAsync(bookingDto);
            _logger.LogInformation($"{CurrentFunctionMethod.GetCaller(this)} - Booking added" + bookingDto);
            return Created(string.Empty, bookingDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"{CurrentFunctionMethod.GetCaller(this)} - Error occurred while posting a booking");
            return BadRequest(ex);
        }
    }
}