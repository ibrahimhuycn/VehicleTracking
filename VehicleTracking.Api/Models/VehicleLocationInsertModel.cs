using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VehicleTracking.Api.Models
{
    public class VehicleLocationInsertModel
    {
        public int VehicleId { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
