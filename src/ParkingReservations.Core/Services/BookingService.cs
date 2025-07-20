using Microsoft.Extensions.Logging;
using Paul.ParkingReservations.Dto;
using Paul.ParkingReservations.Infrastructure.SqlLite;

namespace Paul.ParkingReservations.Core.Services;

public class BookingService : IBookingService
{
    private readonly ILogger<BookingService> _logger;
    private readonly IDapperProvider _dapperProvider;

    public BookingService(ILogger<BookingService> logger, IDapperProvider dapperProvider)
    {
        _logger = logger;
        _dapperProvider = dapperProvider;
    }

    public async Task<List<ParkingSlotsDto>> GetAvailableParkingSlotsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        _logger.LogDebug($"{CurrentFunctionMethod.GetCaller(this)} - Started");

        var bookedSpacesInYourDateRange = await _dapperProvider.GetBookedParkingSlotsByDateRange(startDate, endDate);
        var allParkingSpaces = await _dapperProvider.GetAllParkingSpaces();

        var availableSlotByEachDay = new List<ParkingSlotsDto>();
        for (var dateInFocus = startDate.Date; dateInFocus <= endDate.Date; dateInFocus = dateInFocus.AddDays(1))
        {
            var bookedSpacesForTheDayBeingProcessed = bookedSpacesInYourDateRange.Where(x => x.DateOfBooking == dateInFocus);
            var availableParkingSlots = allParkingSpaces.Where(x => bookedSpacesForTheDayBeingProcessed.All(y => y.ParkingSpaceId != x.ParkingSpaceId)).ToList();

            foreach (var availableParkingSlot in availableParkingSlots)
            {
                availableSlotByEachDay.Add(new ParkingSlotsDto { DateOfBooking = dateInFocus, ParkingSpaceId = availableParkingSlot.ParkingSpaceId, StructureName = availableParkingSlot.StructureName });
            }
        }

        return availableSlotByEachDay;
    }

    ////public async Task<List<ParkingSlotsDto>> GetAvailableParkingSlotsByDateRangeUsingSingleSqlStatementAsync(DateTime startDate, DateTime endDate)
    ////{
    ////    _logger.LogDebug($"{CurrentFunctionMethod.GetCaller(this)} - Started");

    ////    var availableSlots = await _dapperProvider.GetAllAvailableParkingSlotsByDateRange(startDate, endDate);

    ////    return availableSlots.ToList();
    ////}

    public async Task<BookingDto> InsertBookingAsync(BookingDto bookingDto)
    {
        _logger.LogDebug($"{CurrentFunctionMethod.GetCaller(this)} - Started");

        bookingDto.BookingId = await _dapperProvider.InsertBookingAsync(bookingDto);

        return bookingDto;
    }

    public async Task<int> DeleteBookingByIdAsync(int id)
    {
        _logger.LogDebug($"{CurrentFunctionMethod.GetCaller(this)} - Started");

        return await _dapperProvider.DeleteBookingByIdAsync(id);
    }
}