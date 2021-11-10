using System;
using System.Collections.Generic;
using System.Linq;

namespace ParkingApi.Model
{
    public class TicketResult
    {
        public Ticket Ticket { get; set; }
        public IList<String> Errors { get; set; }
        
        public bool Valid => Errors != null || !Errors.Any();
    }
}