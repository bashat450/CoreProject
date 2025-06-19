using AdminDyanamoEnterprises.DTOs;
using AdminDyanamoEnterprises.DTOs.Master;
using AdminDyanamoEnterprises.Repository;
using AdminDyanamoEnterprises.Repository;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdminDyanamoEnterprises.Repository
{
    public class FabricRepository : IFabricRepository
    {
        private readonly IConfiguration _config;

        public FabricRepository(IConfiguration config)
        {
            this._config = config;
        }
        public string sqlConnection()
        {
            return _config.GetConnectionString("DyanamoEnterprises_DB").ToString();
        }


        public string Sp_InsertOrUpdateOrDeleteFabric(FabricTypePageViewModel fabricType)
        {
            using (SqlConnection con = new SqlConnection(sqlConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("Sp_InsertOrUpdateOrDeleteFabric", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    int id = fabricType.AddFabric.FabricID;
                    string action = id <= 0 ? "insert" : "update";

                    cmd.Parameters.AddWithValue("@FabricID", id);
                    cmd.Parameters.AddWithValue("@FabricName", fabricType.AddFabric.FabricName);
                    cmd.Parameters.AddWithValue("@Action", action);

                    // Add output parameters
                    SqlParameter errorCodeParam = new SqlParameter("@ErrorCode", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(errorCodeParam);

                    SqlParameter returnMessageParam = new SqlParameter("@ReturnMessage", SqlDbType.NVarChar, 200)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(returnMessageParam);

                    con.Open();
                    cmd.ExecuteNonQuery();

                    // Read output values
                    int errorCode = (int)errorCodeParam.Value;
                    string returnMessage = returnMessageParam.Value.ToString();

                    // You can return it or log it
                    return $"Status: {errorCode}, Message: {returnMessage}";
                }
            }
        }



        public List<FabricType> GetAllListType()
        {
            List<FabricType> fabricnames = new List<FabricType>();
            using (SqlConnection con = new SqlConnection(sqlConnection()))
            {
                SqlCommand cmd = new SqlCommand("Sp_InsertOrUpdateOrDeleteFabric", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@FabricID", 0); // Dummy value
                cmd.Parameters.AddWithValue("@FabricName", DBNull.Value); // Dummy value
                cmd.Parameters.AddWithValue("@Action", "select");

                // Output parameters (must always be provided)
                SqlParameter errorCodeParam = new SqlParameter("@ErrorCode", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(errorCodeParam);

                SqlParameter returnMessageParam = new SqlParameter("@ReturnMessage", SqlDbType.NVarChar, 200)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(returnMessageParam);

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    FabricType obj = new FabricType()
                    {
                        FabricName = dr["FabricName"].ToString(),
                        FabricID = Convert.ToInt32(dr["FabricID"])
                    };
                    fabricnames.Add(obj);
                }

                // Optional: check output values
                int errorCode = (int)(errorCodeParam.Value ?? -1);
                string returnMessage = returnMessageParam.Value?.ToString();

                // You can log or return this info if needed

                return fabricnames;
            }
        }


        public string DeleteFabric(int id)
        {
            using (SqlConnection con = new SqlConnection(sqlConnection()))
            {
                SqlCommand cmd = new SqlCommand("Sp_InsertOrUpdateOrDeleteFabric", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Action", "delete");
                cmd.Parameters.AddWithValue("@FabricID", id);
                cmd.Parameters.AddWithValue("@FabricName", DBNull.Value);

                SqlParameter errorCodeParam = new SqlParameter("@ErrorCode", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(errorCodeParam);

                SqlParameter returnMessageParam = new SqlParameter("@ReturnMessage", SqlDbType.NVarChar, 200)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(returnMessageParam);

                con.Open();
                cmd.ExecuteNonQuery();

                int errorCode = (int)errorCodeParam.Value;
                string returnMessage = returnMessageParam.Value.ToString();

                return $"Status: {errorCode}, Message: {returnMessage}";
            }
        }

    }
}

