using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VehicleTracking.Api.Models
{
    public class VehicleLocationModel
    {
        public string UniqueVehicleName { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public DateTime? ReportedTime { get; set; }
    }
}
