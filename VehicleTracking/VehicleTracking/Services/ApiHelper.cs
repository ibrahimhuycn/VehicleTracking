using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Tiny.RestClient;

namespace VehicleTracking.Services
{
    public class ApiHelper
    {
        public static TinyRestClient ApiClient { get; set; }

        public static void InitalizeClient()
        {
            ApiClient = new TinyRestClient(new HttpClient(), "https://vehiclelocations.azurewebsites.net/api/");
        }

    }
}