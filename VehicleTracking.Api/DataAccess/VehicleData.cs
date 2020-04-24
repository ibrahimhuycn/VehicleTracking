using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VehicleTracking.Api.Models;

namespace VehicleTracking.Api.DataAccess
{
    public class VehicleData : IVehicleAccessData
    {
        private ISqlDataAccess _dataAccess;
        public VehicleData(ISqlDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }
        public VehicleLocationInsertModel InsertOrUpdateLocation(VehicleLocationInsertModel vehicleLocation)
        {
             return  _dataAccess.SaveDataReturnInserted<VehicleLocationInsertModel, dynamic>("usp_InsertVehicleLocation", vehicleLocation);
        }

        public Vehicle InsertVehicle(VehicleInsertModel vehicle)
        {
            var inserted =  _dataAccess.SaveDataReturnInserted<Vehicle, dynamic>("[dbo].[usp_InsertVehicle]", vehicle);
            return inserted;
        }

        public Vehicle GetVehicle(string description)
        {
            var p = new { VehicleDescription = description };
            return  _dataAccess.LoadData<Vehicle, dynamic>("usp_GetVehicleByDescription", p).FirstOrDefault();
        }

        public List<VehicleLocationModel> GetAllLocations()
        {
            return _dataAccess.LoadDataWithoutParameters<VehicleLocationModel>("[dbo].[usp_GetLocationAllVehicles]");
        }

    }
}
