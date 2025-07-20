using Microsoft.Extensions.Logging;
using Moq;
using Paul.ParkingReservations.Core.Services;
using Paul.ParkingReservations.Dto;
using Paul.ParkingReservations.Infrastructure.SqlLite;
using Xunit;

namespace Paul.ParkingReservations.Core.UnitTests.Services;

public class ParkingStructureServiceTests
{
    [Fact]
    public async Task GetAllAsync_ReturnsParkingStructures()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<ParkingStructureService>>();
        var dapperProviderMock = new Mock<IDapperProvider>();
        var expectedStructures = new List<ParkingStructureDto>
        {
            new ParkingStructureDto(1, "Structure A"),
            new ParkingStructureDto(2, "Structure B")
        };
        dapperProviderMock
            .Setup(x => x.GetAllParkingStructures())
            .ReturnsAsync(expectedStructures);

        var service = new ParkingStructureService(loggerMock.Object, dapperProviderMock.Object);

        // Act
        var result = await service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Collection(result,
            item => Assert.Equal("Structure A", item.StructureName),
            item => Assert.Equal("Structure B", item.StructureName));
    }
}