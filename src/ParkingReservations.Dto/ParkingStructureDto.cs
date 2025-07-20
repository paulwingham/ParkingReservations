namespace Paul.ParkingReservations.Dto;

public class ParkingStructureDto
{
    public ParkingStructureDto()
    {
    }

    public ParkingStructureDto(int parkingStructure_Id, string structureName)
    {
        ParkingStructure_Id = parkingStructure_Id;
        StructureName = structureName;
    }

    public int ParkingStructure_Id { get; set; }

    public string StructureName { get; set; }
}