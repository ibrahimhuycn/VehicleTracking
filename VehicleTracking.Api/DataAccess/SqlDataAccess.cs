using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;


namespace VehicleTracking.Api.DataAccess
{
    public class SqlDataAccess : ISqlDataAccess
    {
        private string _connectionString;
        public SqlDataAccess(IHelper helper)
        {
            _connectionString = helper.GetConnectionString();
        }

        public List<T> LoadData<T, U>(string storedProcedure, U parameters)
        {

            using (IDbConnection cnx = new SqlConnection(_connectionString))
            {
                var rows = cnx.Query<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                return rows.ToList();
            }
        }
        public List<T> LoadDataWithoutParameters<T>(string storedProcedure)
        {

            using (IDbConnection cnx = new SqlConnection(_connectionString))
            {
                var rows = cnx.Query<T>(storedProcedure, commandType: CommandType.StoredProcedure);
                return rows.ToList();
            }
        }
        public void SaveData<T>(string storedProcedure, T parameters)
        {

            using (IDbConnection cnx = new SqlConnection(_connectionString))
            {
                cnx.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public T SaveDataReturnInserted<T, U>(string storedProcedure, U parameters)
        {
            using (IDbConnection cnx = new SqlConnection(_connectionString))
            {
                return cnx.Query<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
    }
}
