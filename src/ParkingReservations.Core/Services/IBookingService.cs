using Paul.ParkingReservations.Dto;

namespace Paul.ParkingReservations.Core.Services;

public interface IBookingService
{
    ////Task<List<ParkingSlotsDto>> GetAvailableParkingSlotsByDateRangeUsingSingleSqlStatementAsync(DateTime startDate, DateTime endDate);

    Task<List<ParkingSlotsDto>> GetAvailableParkingSlotsByDateRangeAsync(DateTime startDate, DateTime endDate);

    Task<BookingDto> InsertBookingAsync(BookingDto bookingDto);

    Task<int> DeleteBookingByIdAsync(int id);
}