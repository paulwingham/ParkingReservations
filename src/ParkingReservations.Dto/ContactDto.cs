namespace Paul.ParkingReservations.Dto;

public class ContactDto
{
    public ContactDto()
    {
    }

    public ContactDto(int contactId, string firstName, string lastName, int contactInformation_Id)
    {
        ContactId = contactId;
        FirstName = firstName;
        LastName = lastName;
        ContactInformation_Id = contactInformation_Id;
    }

    public int ContactId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public int ContactInformation_Id { get; set; }
}