
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using VehicleTracking.Models;

namespace VehicleTracking.Services
{
    public class RestService
    {
        public RestService()
        {
            if (ApiHelper.ApiClient is null) { ApiHelper.InitalizeClient(); }
        }

        public async Task<List<VehicleLocationModel>> GetAllVehicalLocations()
        {
            Debug.WriteLine($"{DateTime.Now} - Location uploading");

            try
            {
                return await ApiHelper.ApiClient.GetRequest("vehiclelocations")
                    .ExecuteAsync<List<VehicleLocationModel>>();

            }
            catch (System.Exception)
            {
                throw;
            }

        }

        public async Task<bool> ReportVehicleLocation(VehicleLocationModel vehicleLocation)
        {
            Debug.WriteLine($"{DateTime.Now} - Location data downloading");

            try
            {
                await ApiHelper.ApiClient.PostRequest("vehiclelocations",vehicleLocation)
                    .ExecuteAsync();

                return true;

            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
