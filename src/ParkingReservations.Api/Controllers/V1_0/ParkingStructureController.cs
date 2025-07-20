using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Paul.ParkingReservations.Core.Services;
using Paul.ParkingReservations.Dto;
using Paul.ParkingReservations.Dto.DomainSettings;

namespace Paul.ParkingReservations.Api.Controllers.V1_0;

public class ParkingStructureController : BaseApiController
{
    private readonly ILogger<ParkingStructureController> _logger;
    private readonly IParkingStructureService _parkingStructureService;
    private readonly ParkingReservationsAppConfigSettings _parkingReservationsAppConfigSettings;

    public ParkingStructureController(ILogger<ParkingStructureController> logger, IParkingStructureService parkingStructureService, IOptions<ParkingReservationsAppConfigSettings> parkingReservationsAppConfigSettings)
    {
        _logger = logger;
        _parkingStructureService = parkingStructureService;
        _parkingReservationsAppConfigSettings = parkingReservationsAppConfigSettings.Value;
    }

    /// <summary>
    /// Retrieves all parking structures.
    /// </summary>
    /// <remarks>
    /// Returns a list of all parking structures available in the system.
    /// </remarks>
    /// <response code="200">Returns the list of parking structures.</response>
    /// <response code="400">If an error occurs while fetching parking structures.</response>
    [ProducesResponseType(typeof(List<ParkingStructureDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Produces("application/json", "text/json")]
    [HttpGet]
    [Route("Get")]
    public async Task<IActionResult> Get()
    {
        _logger.LogInformation($"{CurrentFunctionMethod.GetCaller(this)} - Started");

        try
        {
            var parkingStructureDtos = await _parkingStructureService.GetAllAsync();

            return Ok(parkingStructureDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"{CurrentFunctionMethod.GetCaller(this)} - Error occurred while fetching parking structures.");
            return BadRequest(new { error = ex.Message });
        }
    }
}