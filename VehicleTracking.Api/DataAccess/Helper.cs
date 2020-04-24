using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VehicleTracking.Api.DataAccess
{
    public class Helper : IHelper
    {
        private IConfiguration _connectionString;
        public Helper(IConfiguration configuartion)
        {
            _connectionString = configuartion;
        }
        public string GetConnectionString(string connectionString = "ConnectionString")
        {
            return _connectionString.GetSection("Data").GetSection("ConnectionString").Value;
        }
    }
}
