using AdminDyanamoEnterprises.DTOs;
using AdminDyanamoEnterprises.DTOs.Master;
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
    public class MaterialRepository : IMaterialRepository
    {
        private readonly IConfiguration _config;

        public MaterialRepository(IConfiguration config)
        {
            this._config = config;
        }
        public string sqlConnection()
        {
            return _config.GetConnectionString("DyanamoEnterprises_DB").ToString();
        }


        public string Sp_InsertOrUpdateOrDeleteMaterialType(MaterialTypePageViewModel materialType)
        {
            using (SqlConnection con = new SqlConnection(sqlConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("Sp_InsertOrUpdateOrDeleteMaterialType", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    int id = materialType.AddMaterial.MaterialID;
                    string action = id <= 0 ? "insert" : "update";

                    cmd.Parameters.AddWithValue("@MaterialID", id);
                    cmd.Parameters.AddWithValue("@MaterialName", materialType.AddMaterial.MaterialName);
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



        public List<MaterialType> GetAllListType()
        {
            List<MaterialType> materialnames = new List<MaterialType>();
            using (SqlConnection con = new SqlConnection(sqlConnection()))
            {
                SqlCommand cmd = new SqlCommand("Sp_InsertOrUpdateOrDeleteMaterialType", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MaterialID", 0); // Dummy value
                cmd.Parameters.AddWithValue("@MaterialName", DBNull.Value); // Dummy value
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
                    MaterialType obj = new MaterialType()
                    {
                        MaterialName = dr["MaterialName"].ToString(),
                        MaterialID = Convert.ToInt32(dr["MaterialID"])
                    };
                    materialnames.Add(obj);
                }

                // Optional: check output values
                int errorCode = (int)(errorCodeParam.Value ?? -1);
                string returnMessage = returnMessageParam.Value?.ToString();

                // You can log or return this info if needed

                return materialnames;
            }
        }


        public string DeleteMaterial(int id)
        {
            using (SqlConnection con = new SqlConnection(sqlConnection()))
            {
                SqlCommand cmd = new SqlCommand("Sp_InsertOrUpdateOrDeleteMaterialType", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Action", "delete");
                cmd.Parameters.AddWithValue("@MaterialID", id);
                cmd.Parameters.AddWithValue("@MaterialName", DBNull.Value);

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

