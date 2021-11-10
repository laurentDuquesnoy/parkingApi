using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ParkingApi.Model;
using ParkingApi.Repository;
using ParkingApi.Services;

namespace ParkingApi.Controllers
{
    [ApiController]
    public class TicketController : ApiBaseController
    {
        private readonly TicketService _ticketService;

        public TicketController(TicketService ticketService)
        {
            _ticketService = ticketService;
        }
    
        [HttpPost("buyTicket")]
        [AllowAnonymous]
        public IActionResult BuyTicket(int hours, string licensePlate)
        {
            var result =  _ticketService.BuyTicket(licensePlate, hours);
            return Ok(result);
        }

        [HttpPost("leaveParking")]
        [AllowAnonymous]
        public IActionResult LeaveParking(string ticket)
        {
            var result = _ticketService.LeaveParking(ticket);
            return Ok(result);
        }

        [HttpGet("Occupation")]
        [AllowAnonymous]
        public IActionResult GetOccupation()
        {
            var result = _ticketService.GetOccupation();
            return Ok(result);
        }
        
    }
}