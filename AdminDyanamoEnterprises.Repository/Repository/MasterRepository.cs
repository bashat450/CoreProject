using AdminDyanamoEnterprises.DTOs;
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
    }
}
