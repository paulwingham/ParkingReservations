using Microsoft.AspNetCore.Mvc;

namespace Paul.ParkingReservations.Api.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class BaseApiController : ControllerBase
{
}