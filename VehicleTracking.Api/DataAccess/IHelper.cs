namespace VehicleTracking.Api.DataAccess
{
    public interface IHelper
    {
        string GetConnectionString(string connectionString = "ConnectionString");
    }
}