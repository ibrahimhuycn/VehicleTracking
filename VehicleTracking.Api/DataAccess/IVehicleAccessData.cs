using System.Collections.Generic;
using VehicleTracking.Api.Models;

namespace VehicleTracking.Api.DataAccess
{
    public interface IVehicleAccessData
    {
        List<VehicleLocationModel> GetAllLocations();
        Vehicle GetVehicle(string description);
        VehicleLocationInsertModel InsertOrUpdateLocation(VehicleLocationInsertModel vehicleLocation);
        Vehicle InsertVehicle(VehicleInsertModel vehicle);
    }
}