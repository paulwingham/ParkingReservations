using Dapper;
using Microsoft.Extensions.Logging;
using Paul.ParkingReservations.Dto;

namespace Paul.ParkingReservations.Infrastructure.SqlLite;

public class DapperProvider : IDapperProvider
{
    private readonly ILogger<DapperProvider> _logger;
    private readonly ISqliteConnectionProvider _sqliteConnectionProvider;

    public DapperProvider(ILogger<DapperProvider> logger, ISqliteConnectionProvider sqliteConnectionProvider)
    {
        _logger = logger;
        _sqliteConnectionProvider = sqliteConnectionProvider;
    }

    //// public async Task<List<ParkingSlotsDto>> GetAllAvailableParkingSlotsByDateRange(DateTime startDate, DateTime endDate)
    //// {
    ////     _logger.LogInformation($"{CurrentFunctionMethod.GetCaller(this)} - Started");
    ////     var connection = _sqliteConnectionProvider.GetSqliteConnection();

    ////     var queryString = @"
    ////         WITH DateRange AS (
    ////             SELECT CAST(@StartDate AS DATE) AS dateofbooking
    ////             UNION ALL
    ////             SELECT DATE(DateOfBooking, '+1 day')
    ////             FROM DateRange
    ////             WHERE DateOfBooking < @EndDate
    ////         ),
    ////         Booked AS (
    ////             SELECT DateOfBooking, ParkingSpace_Id
    ////             FROM booking
    ////             WHERE DateOfBooking BETWEEN @StartDate AND @EndDate
    ////         )
    ////         SELECT dr.DateOfBooking, ps.ParkingSpaceId, pst.structurename
    ////         FROM DateRange dr
    ////         CROSS JOIN parkingspace ps
    ////         LEFT JOIN Booked b ON b.DateOfBooking = dr.DateOfBooking AND b.parkingspace_id = ps.parkingspaceid
    ////LEFT JOIN parkingstructure pst ON ps.parkingstructureid = pst.parkingstructure_id
    ////         WHERE b.ParkingSpace_Id IS NULL
    ////         ORDER BY dr.DateOfBooking, ps.ParkingSpaceId";

    ////     var availableSlots = await connection.QueryAsync<ParkingSlotsDto>(queryString, new { StartDate = startDate.Date, EndDate = endDate.Date });
    ////     return availableSlots.ToList();
    //// }

    public async Task<List<ParkingSlotsDto>> GetBookedParkingSlotsByDateRange(DateTime startDate, DateTime endDate)
    {
        _logger.LogInformation($"{ CurrentFunctionMethod.GetCaller(this)} - Started");
        var connection = _sqliteConnectionProvider.GetSqliteConnection();

        var queryString1 = $"SELECT parkingspace_id, dateofbooking FROM booking WHERE dateofbooking >= @StartDate AND dateofbooking <= @EndDate ORDER BY dateofbooking DESC";
        var bookedSpacesInYourDateRange = await connection.QueryAsync(queryString1, new { StartDate = startDate.Date, EndDate = endDate.Date });

        var parkingSlotsDtos = new List<ParkingSlotsDto>();
        foreach (var item in bookedSpacesInYourDateRange)
        {
            parkingSlotsDtos.Add(new ParkingSlotsDto { ParkingSpaceId = (int)item.parkingspace_id, DateOfBooking = (DateTime)item.dateofbooking });
        }

        return parkingSlotsDtos;
    }

    public async Task<IEnumerable<ParkingSpaceDto>> GetAllParkingSpaces()
    {
        _logger.LogInformation($"{CurrentFunctionMethod.GetCaller(this)} - Started");
        var connection = _sqliteConnectionProvider.GetSqliteConnection();

        var queryString = $"SELECT p.parkingspaceid, ps.structurename FROM parkingspace AS p INNER JOIN parkingstructure AS ps ON p.parkingstructureid = ps.parkingstructure_id";
        var allParkingSpaces = await connection.QueryAsync<ParkingSpaceDto>(queryString);

        return allParkingSpaces;
    }

    public async Task<int> InsertBookingAsync(BookingDto bookingDto)
    {
        _logger.LogInformation($"{CurrentFunctionMethod.GetCaller(this)} - Started");
        var connection = _sqliteConnectionProvider.GetSqliteConnection();

        //// this has been done  like this, so I don't have to include the parkingstructureid on the BookingDto
        var parkingstructureid = await GetParkingStructureId(bookingDto.ParkingSpace_Id);
        if (await DuplicateParkingSpaceBooking(bookingDto, parkingstructureid))
        {
            throw new InvalidOperationException("This parking space is already booked for the selected date.");
        }

        var queryString = $"INSERT INTO booking (bookee_id, dateofbooking, parkingspace_id, parkingstructure_id) VALUES (@Bookee_Id, @DateOfBooking, @ParkingSpace_Id, @ParkingStructure_Id); SELECT last_insert_rowid();";
        var output = await connection.QuerySingleAsync<int>(queryString, new
        {
            Bookee_Id = bookingDto.Bookee_Id,
            DateOfBooking = bookingDto.DateOfBooking.Date,
            ParkingSpace_Id = bookingDto.ParkingSpace_Id,
            ParkingStructure_Id = parkingstructureid
        });

        return output;
    }

    public async Task<IEnumerable<ParkingStructureDto>> GetAllParkingStructures()
    {
        _logger.LogInformation($"{CurrentFunctionMethod.GetCaller(this)} - Started");
        var connection = _sqliteConnectionProvider.GetSqliteConnection();
        var queryString = $"SELECT parkingstructure_id, structurename FROM parkingstructure";
        var parkingStructureDtos = await connection.QueryAsync<ParkingStructureDto>(queryString);

        return parkingStructureDtos;
    }

    public async Task<int> DeleteBookingByIdAsync(int id)
    {
        _logger.LogInformation($"{CurrentFunctionMethod.GetCaller(this)} - Started");
        var connection = _sqliteConnectionProvider.GetSqliteConnection();
        var queryString = $"DELETE FROM Product WHERE bookingid = {id}";

        return await connection.ExecuteAsync(queryString);
    }

    public async Task<UserDto> GetUserDetails()
    {
        _logger.LogInformation($"{CurrentFunctionMethod.GetCaller(this)} - Started");

        var userDto = new UserDto("ATH", new ContactDto(1, "Alice", "Thompson", 1), 1);

        return userDto;
    }

    public async Task<ContactDto> GetContactById(int id)
    {
        _logger.LogInformation($"{CurrentFunctionMethod.GetCaller(this)} - Started");
        var connection = _sqliteConnectionProvider.GetSqliteConnection();
        var queryString = $"SELECT * FROM contact WHERE contactid={id}";
        var contactDto = await connection.QueryAsync<ContactDto>(queryString);

        return contactDto.FirstOrDefault();
    }

    private async Task<int> GetParkingStructureId(int id)
    {
        _logger.LogInformation($"{CurrentFunctionMethod.GetCaller(this)} - Started");
        var connection = _sqliteConnectionProvider.GetSqliteConnection();
        var queryString = $"SELECT parkingstructureid FROM parkingspace WHERE parkingspaceid={id}";
        var parkingstructureid = await connection.QueryAsync<int>(queryString);

        return parkingstructureid.FirstOrDefault();
    }

    private async Task<bool> DuplicateParkingSpaceBooking(BookingDto bookingDto, int parkingStructureId)
    {
        _logger.LogInformation($"{CurrentFunctionMethod.GetCaller(this)} - Started");
        var connection = _sqliteConnectionProvider.GetSqliteConnection();

        var queryString = $"SELECT bookingid FROM booking WHERE parkingspace_id=@ParkingSpaceId AND parkingstructure_id=@ParkingStructureId AND dateofbooking=@DateOfBooking";
        var result = await connection.QueryAsync(queryString, new { ParkingSpaceId = bookingDto.ParkingSpace_Id, ParkingStructureId = parkingStructureId, DateOfBooking = bookingDto.DateOfBooking.Date });

        return result.Any();
    }
}