using System;

namespace ParkingApi.Model
{
    public class Ticket
    {
        public int Id { get; set; }
        public string LicensePlate { get; set; }
        public DateTime ExpiryTime { get; set; }
        public string Guid { get; set; }
        public bool Used { get; set; }
        public bool Valid { get; set; }
        
    }
}