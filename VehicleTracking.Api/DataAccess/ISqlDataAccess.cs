using System.Collections.Generic;

namespace VehicleTracking.Api.DataAccess
{
    public interface ISqlDataAccess
    {
        List<T> LoadData<T, U>(string storedProcedure, U parameters);
        List<T> LoadDataWithoutParameters<T>(string storedProcedure);
        void SaveData<T>(string storedProcedure, T parameters);
        T SaveDataReturnInserted<T, U>(string storedProcedure, U parameters);
    }
}