using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ParkingApi
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class ApiBaseController: ControllerBase
    {
    }
}