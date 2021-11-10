using System;
using System.Collections.Generic;
using System.Linq;

namespace ParkingApi.Model
{
    public class OccupationResult
    {
        public int MaxOccupation { get; set; }
        public int CurrentOccupation { get; set; }
        public List<string> Errors { get; set; }
        public List<string> Messages { get; set; }
        public List<string> Warnings { get; set; }
        public bool Valid => Errors != null || Errors.Any();
    }
}