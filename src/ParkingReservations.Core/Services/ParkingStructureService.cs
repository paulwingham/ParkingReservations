using Microsoft.Extensions.Logging;
using Paul.ParkingReservations.Dto;
using Paul.ParkingReservations.Infrastructure.SqlLite;

namespace Paul.ParkingReservations.Core.Services;

public class ParkingStructureService : IParkingStructureService
{
    private readonly ILogger<ParkingStructureService> _logger;
    private readonly IDapperProvider _dapperProvider;

    public ParkingStructureService(ILogger<ParkingStructureService> logger, IDapperProvider dapperProvider)
    {
        _logger = logger;
        _dapperProvider = dapperProvider;
    }

    public async Task<IEnumerable<ParkingStructureDto>> GetAllAsync()
    {
        _logger.LogDebug($"{CurrentFunctionMethod.GetCaller(this)} - Started");

        var parkingStructureDtos = await _dapperProvider.GetAllParkingStructures();

        return parkingStructureDtos;
    }
}