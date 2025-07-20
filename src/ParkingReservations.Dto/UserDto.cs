namespace Paul.ParkingReservations.Dto;

public class UserDto
{
    public UserDto()
    {
    }

    public UserDto(string employeeId, ContactDto contact, int bookeeId)
    {
        EmployeeId = employeeId;
        Contact = contact;
        BookeeId = bookeeId;
    }

    public string EmployeeId { get; set; }

    public int BookeeId { get; set; }

    public ContactDto Contact { get; set; }
}