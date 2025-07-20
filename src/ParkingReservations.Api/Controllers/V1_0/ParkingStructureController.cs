using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Paul.ParkingReservations.Core.Services;
using Paul.ParkingReservations.Dto;
using Paul.ParkingReservations.Dto.DomainSettings;

namespace Paul.ParkingReservations.Api.Controllers.V1_0;

[Route("api/[controller]")]
[ApiVersion("1.0")]
public class ParkingStructureController : BaseApiController
{
    private readonly ILogger<LoginController> _logger;
    private readonly IParkingStructureService _parkingStructureService;
    private readonly ParkingReservationsAppConfigSettings _parkingReservationsAppConfigSettings;

    public ParkingStructureController(ILogger<LoginController> logger, IParkingStructureService parkingStructureService, IOptions<ParkingReservationsAppConfigSettings> parkingReservationsAppConfigSettings)
    {
        _logger = logger;
        _parkingStructureService = parkingStructureService;
        _parkingReservationsAppConfigSettings = parkingReservationsAppConfigSettings.Value;
    }

    [ProducesResponseType(typeof(List<ParkingStructureDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet]
    [Route("Get")]
    public async Task<IActionResult> Get()
    {
        _logger.LogDebug($"{CurrentFunctionMethod.GetCaller(this)} - Started");

        try
        {
            var parkingStructureDtos = await _parkingStructureService.GetAllAsync();

            return Ok(parkingStructureDtos);
        }
        catch (Exception ex)
        {
            {
                _logger.LogError(ex, $"{CurrentFunctionMethod.GetCaller(this)} - Error occurred while fetching parking structures.");
                return BadRequest(ex);
            }
        }
    }
}