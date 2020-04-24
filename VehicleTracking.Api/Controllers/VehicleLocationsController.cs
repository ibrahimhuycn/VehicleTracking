using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VehicleTracking.Api.DataAccess;
using VehicleTracking.Api.Models;

namespace VehicleTracking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleLocationsController : ControllerBase
    {
        private IVehicleAccessData _vehicleData;

        public VehicleLocationsController(IVehicleAccessData vehicleData)
        {
            _vehicleData = vehicleData;
        }
        // GET: api/VehicleLocations
        [HttpGet]
        public async Task<List<VehicleLocationModel>> Get()
        {

            return await Task.Run(() =>
            {
                return _vehicleData.GetAllLocations();
            });

        }

        // POST: api/VehicleLocations
        [HttpPost]
        public async Task Post(VehicleLocationModel locationData)
        {

            #region Control Flow Explanation
                /*
                 INSERT CONTROL FLOW
                 -------------------------------
                 1. Is Vehicle on database?
                    YES: Got the vehicleId? Proceed with inserting latest location
                    NO:  Insert vehicle, get the Vehicle Id, Insert location

                 2. The end. 

                 */
            #endregion


            try
            {
                var _vehicalId = 0;

                var vehicle = await Task.Run(() =>
                {
                    return _vehicleData.GetVehicle(locationData.UniqueVehicleName);
                });

                if (vehicle is null)
                {
                    var insertedData = await Task.Run(() =>
                    {
                        return _vehicleData.InsertVehicle(new VehicleInsertModel() { Description = locationData.UniqueVehicleName });
                    });
                    _vehicalId = insertedData.Id;
                }
                else
                {
                    _vehicalId = vehicle.Id;
                }

                _ = await Task.Run(() =>
                {
                    return _vehicleData.InsertOrUpdateLocation(new VehicleLocationInsertModel()
                    {
                        VehicleId = _vehicalId,
                        Longitude = locationData.Longitude,
                        Latitude = locationData.Latitude
                    });

                });
            }
            catch (Exception)
            {
                //Ignore for now
            }
        }

    }
}
