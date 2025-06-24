using AdminDyanamoEnterprises.DTOs;
using AdminDyanamoEnterprises.DTOs.Common;
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
using static AdminDyanamoEnterprises.Repository.IMasterRepository;

namespace AdminDyanamoEnterprises.Repository
{
    public class MasterRepository: IMasterRepository, IColorRepository, IFabricRepository, IMaterialRepository
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

        public MasterResponse InsertorUpdateCategoryType(CategoryTypePageViewModel categoryType)
        {
            MasterResponse response = new MasterResponse();

            using (SqlConnection con = new SqlConnection(sqlConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("Dynamo.Sp_InsertOrUpdateOrDeleteCategory", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    int categoryId = categoryType.AddCategory.CategoryID <= 0 ? 0 : categoryType.AddCategory.CategoryID;
                    string action = categoryId == 0 ? "insert" : "update";

                    cmd.Parameters.AddWithValue("@CategoryID", categoryId);
                    cmd.Parameters.AddWithValue("@CategoryName", categoryType.AddCategory.CategoryName);
                    cmd.Parameters.AddWithValue("@Action", action);

                    // Add OUTPUT parameters
                    SqlParameter errorCodeParam = new SqlParameter("@ErrorCode", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    SqlParameter returnMsgParam = new SqlParameter("@ReturnMessage", SqlDbType.NVarChar, 200)
                    {
                        Direction = ParameterDirection.Output
                    };

                    cmd.Parameters.Add(errorCodeParam);
                    cmd.Parameters.Add(returnMsgParam);

                    con.Open();
                    cmd.ExecuteNonQuery();

                    response.ErrorCode = (int)errorCodeParam.Value;
                    response.ReturnMessage = returnMsgParam.Value.ToString();
                }
            }

            return response;
        }


        public List<CategoryType> GetAllCategoryType()
        {
            List<CategoryType> categorynames = new List<CategoryType>();
            using (SqlConnection con = new SqlConnection(sqlConnection()))
            {
                SqlCommand cmd = new SqlCommand("Dynamo.Sp_InsertOrUpdateOrDeleteCategory", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CategoryID", DBNull.Value);
                cmd.Parameters.AddWithValue("@CategoryName", DBNull.Value);
                cmd.Parameters.AddWithValue("@Action", "select");

                // Add dummy output parameters (since SP expects them)
                cmd.Parameters.Add("@ErrorCode", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@ReturnMessage", SqlDbType.NVarChar, 200).Direction = ParameterDirection.Output;

                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    CategoryType obj = new CategoryType()
                    {
                        CategoryName = dr["CategoryName"].ToString(),
                        CategoryID = Convert.ToInt32(dr["CategoryID"])
                    };

                    categorynames.Add(obj);
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
        public MasterResponse InsertOrUpdateOrDeletePattern(PatternTypePageViewModel patternTypeViewModel)
        {
            MasterResponse response = new MasterResponse();

            using (SqlConnection con = new SqlConnection(sqlConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("Dynamo.Sp_InsertOrUpdateOrDeletePattern", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    int patternId = patternTypeViewModel.AddPattern.PatternID <= 0 ? 0 : patternTypeViewModel.AddPattern.PatternID;
                    string action = patternId == 0 ? "insert" : "update";

                    cmd.Parameters.AddWithValue("@PatternID", patternId == 0 ? (object)DBNull.Value : patternId); // Pass DBNull for insert to match SP's NULL default
                    cmd.Parameters.AddWithValue("@PatternName", patternTypeViewModel.AddPattern.PatternName ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Action", action);

                    // Add OUTPUT parameters
                    SqlParameter errorCodeParam = new SqlParameter("@ErrorCode", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    SqlParameter returnMsgParam = new SqlParameter("@ReturnMessage", SqlDbType.NVarChar, 200)
                    {
                        Direction = ParameterDirection.Output
                    };

                    cmd.Parameters.Add(errorCodeParam);
                    cmd.Parameters.Add(returnMsgParam);

                    con.Open();
                    cmd.ExecuteNonQuery();

                    response.ErrorCode = (int)errorCodeParam.Value;
                    response.ReturnMessage = returnMsgParam.Value.ToString();
                }
            }

            return response;
        }
        public List<PatternType> GetAllPatternType()
        {
            List<PatternType> patternNames = new List<PatternType>();
            using (SqlConnection con = new SqlConnection(sqlConnection()))
            {
                SqlCommand cmd = new SqlCommand("Dynamo.Sp_InsertOrUpdateOrDeletePattern", con);
                cmd.CommandType = CommandType.StoredProcedure;

                // Pass appropriate values for 'select' action
                cmd.Parameters.AddWithValue("@PatternID", DBNull.Value);
                cmd.Parameters.AddWithValue("@PatternName", DBNull.Value);
                cmd.Parameters.AddWithValue("@Action", "select");

                // Add dummy output parameters as the SP expects them
                cmd.Parameters.Add("@ErrorCode", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@ReturnMessage", SqlDbType.NVarChar, 200).Direction = ParameterDirection.Output;

                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    PatternType obj = new PatternType()
                    {
                        PatternName = dr["PatternName"].ToString(),
                        PatternID = Convert.ToInt32(dr["PatternID"])
                    };

                    patternNames.Add(obj);
                }
            }
            return patternNames;
        }
        public void DeletePattern(int id)
        {
            using (SqlConnection con = new SqlConnection(sqlConnection()))
            {
                SqlCommand cmd = new SqlCommand("SP_InsertOrUpdateOrDeletePattern", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Action", "delete");
                cmd.Parameters.AddWithValue("@PatternID", id);
                cmd.Parameters.AddWithValue("@PatternName", DBNull.Value);

                SqlParameter errorCodeParam = new SqlParameter("@ErrorCode", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                SqlParameter returnMsgParam = new SqlParameter("@ReturnMessage", SqlDbType.NVarChar, 200)
                {
                    Direction = ParameterDirection.Output
                };

                cmd.Parameters.Add(errorCodeParam);
                cmd.Parameters.Add(returnMsgParam);


                con.Open();
                cmd.ExecuteNonQuery();

            }


        }

        public MasterResponse InsertOrUpdateSubCategory(SubAddCategoryType model)
        {
            MasterResponse response = new MasterResponse();

            using (SqlConnection conn = new SqlConnection(sqlConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("Sp_InsertOrUpdateOrDelete_MasterSubCategory", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@SubCategoryId", model.SubCategoryID ?? 0);
                    cmd.Parameters.AddWithValue("@CategoryId", model.CategoryID);
                    cmd.Parameters.AddWithValue("@SubCategoryName", model.SubCategoryName ?? string.Empty);

                    // Output parameters
                    SqlParameter errorCodeParam = new SqlParameter("@ErrorCode", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    SqlParameter returnMsgParam = new SqlParameter("@ReturnMessage", SqlDbType.NVarChar, 200) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(errorCodeParam);
                    cmd.Parameters.Add(returnMsgParam);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    response.ErrorCode = Convert.ToInt32(errorCodeParam.Value);
                    response.ReturnMessage = returnMsgParam.Value.ToString();
                }
            }

            return response;
        }


        public List<SubCategoryType> GetAllSubCategoriesWithCategoryName()
        {
            var list = new List<SubCategoryType>();

            using (SqlConnection con = new SqlConnection(sqlConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("Sp_InsertOrUpdateOrDelete_MasterSubCategory", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Mandatory parameters
                    cmd.Parameters.AddWithValue("@Action", "select");

                    // Output parameters (required by the procedure)
                    SqlParameter errorCodeParam = new SqlParameter("@ErrorCode", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    SqlParameter returnMessageParam = new SqlParameter("@ReturnMessage", SqlDbType.NVarChar, 200)
                    {
                        Direction = ParameterDirection.Output
                    };

                    cmd.Parameters.Add(errorCodeParam);
                    cmd.Parameters.Add(returnMessageParam);

                    con.Open();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);

                    foreach (DataRow dr in dt.Rows)
                    {
                        list.Add(new SubCategoryType
                        {
                            SubCategoryName = dr["SubCategoryName"].ToString(),
                            SubCategoryID = Convert.ToInt32(dr["SubCategoryID"]),
                            CategoryID = Convert.ToInt32(dr["CategoryID"]),
                            CategoryName = new CategoryType
                            {
                                CategoryName = dr["CategoryName"].ToString()
                            }
                        });
                    }

                    
                    var errorCode = errorCodeParam.Value;
                    var returnMsg = returnMessageParam.Value?.ToString();
                }
            }

            return list;
        }

        public string DeleteSubCategory(int subCategoryId)
        {
            using (SqlConnection con = new SqlConnection(sqlConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("Sp_InsertOrUpdateOrDelete_MasterSubCategory", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Required inputs
                    cmd.Parameters.AddWithValue("@SubCategoryId", subCategoryId);
                    cmd.Parameters.AddWithValue("@Action", "delete");

                    // Dummy inputs for unused fields
                    cmd.Parameters.AddWithValue("@CategoryId", 0);
                    cmd.Parameters.AddWithValue("@SubCategoryName", DBNull.Value);

                    // Output parameters
                    SqlParameter errorCodeParam = new SqlParameter("@ErrorCode", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    SqlParameter returnMessageParam = new SqlParameter("@ReturnMessage", SqlDbType.NVarChar, 200)
                    {
                        Direction = ParameterDirection.Output
                    };

                    cmd.Parameters.Add(errorCodeParam);
                    cmd.Parameters.Add(returnMessageParam);

                    con.Open();
                    cmd.ExecuteNonQuery();

                    string returnMessage = returnMessageParam.Value?.ToString();
                    return returnMessage;
                }
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
