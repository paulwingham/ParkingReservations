namespace Paul.ParkingReservations.Dto;

public class BookeeDto
{
    public BookeeDto()
    {
    }

    public BookeeDto(int bookeeId, int contact_Id)
    {
        BookeeId = bookeeId;
        Contact_id = contact_Id;
    }

    public int BookeeId { get; set; }

    public int Contact_id { get; set; }
}