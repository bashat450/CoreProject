using AdminDyanamoEnterprises.DTOs;
using AdminDyanamoEnterprises.DTOs.Master;
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
    public class MasterRepository:IMasterRepository
    {
        private readonly IConfiguration _config;

        public MasterRepository(IConfiguration config)
        {
            this._config = config;
        }
        public string sqlConnection()
        {
            return _config.GetConnectionString("DyanamoEnterprises_DB").ToString();
        }
       
       
        public void InsertorUpdateCategoryType(CategoryTypePageViewModel categoryType)
        {
            using (SqlConnection con = new SqlConnection(sqlConnection()))
            {
              
                using (SqlCommand cmd = new SqlCommand("Sp_InsertOrUpdateOrDelete", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@CategoryId", categoryType.AddCategory.CategoryID <= 0 ? 0 : categoryType.AddCategory.CategoryID);
                    cmd.Parameters.AddWithValue("@CategoryName", categoryType.AddCategory.CategoryName);
                    cmd.Parameters.AddWithValue("@Action", DBNull.Value); // not needed unless delete
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<CategoryType>GetAllListType()
        {
            List<CategoryType> categorynames = new List<CategoryType>();
            using (SqlConnection con = new SqlConnection(sqlConnection()))
            {
                SqlCommand cmd = new SqlCommand("SP_InsertCategory", con);
               
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.Parameters.AddWithValue("@Action", "select");
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    // Add each row's value to your list
                    CategoryType obj = new CategoryType()
                    {
                        CategoryName = dr["CategoryName"].ToString(),
                        CategoryID = Convert.ToInt32(dr["CategoryID"])
                    };
                    
                    categorynames.Add(obj);
                    /*categorynames.Add(new CategoryType
                    {
                        Name = dr["CategoryName"].ToString()
                    });*/
                }
            }
            return categorynames;
        }

        public void DeleteCategory(int id)
        {
            using (SqlConnection con = new SqlConnection(sqlConnection()))
            {
                SqlCommand cmd = new SqlCommand("SP_InsertCategory", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Action", "delete");
                cmd.Parameters.AddWithValue("@CategoryId", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        #region============================Color Repository================================
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
                    return $"Status: {errorCode}, Message: {returnMessage}, ActionType: {action}";
                }
            }
        }

        public List<ColorType> GetAllListColorType()
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
        #endregion

        #region============================Fabric Repository================================
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
                    return $"Status: {errorCode}, Message: {returnMessage}, ActionType: {action}";
                }
            }
        }
        public List<FabricType> GetAllListFabricType()
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
        #endregion

        #region==================================Material Repository=================================
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
                    return $"Status: {errorCode}, Message: {returnMessage}, ActionType: {action}";

                }
            }
        }
        public List<MaterialType> GetAllListMaterialType()
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
        #endregion
        
    }
}
