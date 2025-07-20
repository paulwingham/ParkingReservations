using Microsoft.Extensions.Logging;
using Moq;
using Paul.ParkingReservations.Core.Services;
using Paul.ParkingReservations.Dto;
using Paul.ParkingReservations.Infrastructure.SqlLite;
using Xunit;

namespace Paul.ParkingReservations.Core.UnitTests.Services;

public class LoginServiceTests
{
    [Fact]
    public async Task LoginAsync_ReturnsUserDto()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<LoginService>>();
        var dapperProviderMock = new Mock<IDapperProvider>();
        var expectedUser = new UserDto("E123", new ContactDto(1, "John", "Doe", 2), 42);

        dapperProviderMock
            .Setup(x => x.GetUserDetails())
            .ReturnsAsync(expectedUser);

        var service = new LoginService(loggerMock.Object, dapperProviderMock.Object);

        // Act
        var result = await service.LoginAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedUser.EmployeeId, result.EmployeeId);
        Assert.Equal(expectedUser.BookeeId, result.BookeeId);
        Assert.Equal(expectedUser.Contact.ContactId, result.Contact.ContactId);
        Assert.Equal(expectedUser.Contact.FirstName, result.Contact.FirstName);
        Assert.Equal(expectedUser.Contact.LastName, result.Contact.LastName);
        Assert.Equal(expectedUser.Contact.ContactInformation_Id, result.Contact.ContactInformation_Id);
    }

    [Fact]
    public async Task GetContactByIdAsync_ReturnsContactDto()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<LoginService>>();
        var dapperProviderMock = new Mock<IDapperProvider>();
        var expectedContact = new ContactDto(1, "John", "Doe", 100);

        dapperProviderMock
            .Setup(x => x.GetContactById(1))
            .ReturnsAsync(expectedContact);

        var service = new LoginService(loggerMock.Object, dapperProviderMock.Object);

        // Act
        var result = await service.GetContactByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedContact.ContactId, result.ContactId);
        Assert.Equal(expectedContact.FirstName, result.FirstName);
        Assert.Equal(expectedContact.LastName, result.LastName);
        Assert.Equal(expectedContact.ContactInformation_Id, result.ContactInformation_Id);
    }
}