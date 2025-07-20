namespace Paul.ParkingReservations.Dto;

public class BookingDto
{
    public BookingDto()
    {
    }

    public BookingDto(int bookingId, int bookee_Id, DateTime dateOfBooking, int parkingSpace_Id)
    {
        BookingId = bookingId;
        Bookee_Id = bookee_Id;
        DateOfBooking = dateOfBooking;
        ParkingSpace_Id = parkingSpace_Id;
    }

    public int BookingId { get; set; }

    public int Bookee_Id { get; set; }

    public DateTime DateOfBooking { get; set; }

    public int ParkingSpace_Id { get; set; }
}