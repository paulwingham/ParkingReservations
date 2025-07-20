using Microsoft.Extensions.Logging;
using Moq;
using Paul.ParkingReservations.Core.Services;
using Paul.ParkingReservations.Dto;
using Paul.ParkingReservations.Infrastructure.SqlLite;
using Xunit;

namespace Paul.ParkingReservations.Core.UnitTests.Services;

public class BookingServiceTests
{
    [Fact]
    public async Task GetAvailableParkingSlotsByDateRangeAsync_ReturnsAvailableSlots()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<BookingService>>();
        var dapperProviderMock = new Mock<IDapperProvider>();

        var startDate = new DateTime(2024, 6, 1);
        var endDate = new DateTime(2024, 6, 2);

        var allParkingSpaces = new List<ParkingSpaceDto>
        {
            new ParkingSpaceDto { ParkingSpaceId = 1, StructureName = "A" },
            new ParkingSpaceDto { ParkingSpaceId = 2, StructureName = "B" }
        };

        var bookedSpaces = new List<ParkingSlotsDto>
        {
            new ParkingSlotsDto { DateOfBooking = startDate, ParkingSpaceId = 1, StructureName = "A" }
        };

        dapperProviderMock.Setup(x => x.GetBookedParkingSlotsByDateRange(startDate, endDate))
            .ReturnsAsync(bookedSpaces);

        dapperProviderMock.Setup(x => x.GetAllParkingSpaces())
            .ReturnsAsync(allParkingSpaces);

        var service = new BookingService(loggerMock.Object, dapperProviderMock.Object);

        // Act
        var result = await service.GetAvailableParkingSlotsByDateRangeAsync(startDate, endDate);

        // Assert
        Assert.NotNull(result);

        // On 2024-06-01, only slot 2 is available; on 2024-06-02, both are available
        Assert.Contains(result, r => r.DateOfBooking == startDate && r.ParkingSpaceId == 2);
        Assert.Contains(result, r => r.DateOfBooking == endDate && r.ParkingSpaceId == 1);
        Assert.Contains(result, r => r.DateOfBooking == endDate && r.ParkingSpaceId == 2);
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public async Task InsertBookingAsync_ShouldSetBookingIdAndReturnBookingDto()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<BookingService>>();
        var dapperProviderMock = new Mock<IDapperProvider>();
        var bookingDto = new BookingDto
        {
            Bookee_Id = 1,
            DateOfBooking = DateTime.Today,
            ParkingSpace_Id = 2
        };
        var expectedBookingId = 42;

        dapperProviderMock
            .Setup(x => x.InsertBookingAsync(It.IsAny<BookingDto>()))
            .ReturnsAsync(expectedBookingId);

        var service = new BookingService(loggerMock.Object, dapperProviderMock.Object);

        // Act
        var result = await service.InsertBookingAsync(bookingDto);

        // Assert
        Assert.Equal(expectedBookingId, result.BookingId);
        Assert.Equal(bookingDto.Bookee_Id, result.Bookee_Id);
        Assert.Equal(bookingDto.DateOfBooking, result.DateOfBooking);
        Assert.Equal(bookingDto.ParkingSpace_Id, result.ParkingSpace_Id);
        dapperProviderMock.Verify(x => x.InsertBookingAsync(It.IsAny<BookingDto>()), Times.Once);
    }
}