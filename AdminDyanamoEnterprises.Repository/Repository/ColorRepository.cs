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
    public class ColorRepository : IColorRepository
    {
        private readonly IConfiguration _config;

        public ColorRepository(IConfiguration config)
        {
            this._config = config;
        }
        public string sqlConnection()
        {
            return _config.GetConnectionString("DyanamoEnterprises_DB").ToString();
        }


        public string Sp_InsertOrUpdateOrDeleteColor(ColorTypePageViewModel colorType)
        {
            using (SqlConnection con = new SqlConnection(sqlConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("Sp_InsertOrUpdateOrDeleteColor", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    int id = colorType.AddColor.ColorID;
                    string action = id <= 0 ? "insert" : "update";

                    cmd.Parameters.AddWithValue("@ColorID", id);
                    cmd.Parameters.AddWithValue("@ColorName", colorType.AddColor.ColorName);
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



        public List<ColorType> GetAllListType()
        {
            List<ColorType> colornames = new List<ColorType>();
            using (SqlConnection con = new SqlConnection(sqlConnection()))
            {
                SqlCommand cmd = new SqlCommand("Sp_InsertOrUpdateOrDeleteColor", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ColorID", 0); // Dummy value
                cmd.Parameters.AddWithValue("@ColorName", DBNull.Value); // Dummy value
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
                    ColorType obj = new ColorType()
                    {
                        ColorName = dr["ColorName"].ToString(),
                        ColorID = Convert.ToInt32(dr["ColorID"])
                    };
                    colornames.Add(obj);
                }

                // Optional: check output values
                int errorCode = (int)(errorCodeParam.Value ?? -1);
                string returnMessage = returnMessageParam.Value?.ToString();

                // You can log or return this info if needed

                return colornames;
            }
        }


        public string DeleteColor(int id)
        {
            using (SqlConnection con = new SqlConnection(sqlConnection()))
            {
                SqlCommand cmd = new SqlCommand("Sp_InsertOrUpdateOrDeleteColor", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Action", "delete");
                cmd.Parameters.AddWithValue("@ColorID", id);
                cmd.Parameters.AddWithValue("@ColorName", DBNull.Value);

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

