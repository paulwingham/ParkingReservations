namespace Paul.ParkingReservations.Dto;

public class ParkingSpaceDto
{
    public ParkingSpaceDto()
    {
    }

    public ParkingSpaceDto(int parkingSpaceId, string structureName)
    {
        ParkingSpaceId = parkingSpaceId;
        StructureName = structureName;
    }

    public int ParkingSpaceId { get; set; }

    public string StructureName { get; set; }
}