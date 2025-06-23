using AdminDyanamoEnterprises.DTOs.Master;
using AdminDyanamoEnterprises.Repository;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

public class BlogsRepository : IBlogsRepository
{
    private readonly IConfiguration _configuration;

    public BlogsRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    private string ConnectionString => _configuration.GetConnectionString("DyanamoEnterprises_DB");

    public IEnumerable<BlogsModel> GetAllBlogs()
    {
        var blogs = new List<BlogsModel>();

        using (SqlConnection con = new SqlConnection(ConnectionString))
        {
            SqlCommand cmd = new SqlCommand("Dynamo.SP_ManageBlogs", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Action", "GET");

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                blogs.Add(new BlogsModel
                {
                    BlogId = dr["BlogId"] != DBNull.Value ? Convert.ToInt32(dr["BlogId"]) : 0,
                    Title = dr["Title"]?.ToString(),
                    Category = dr["Category"]?.ToString(),
                    PublishDate = dr["PublishDate"] != DBNull.Value ? Convert.ToDateTime(dr["PublishDate"]) : DateTime.MinValue,
                    Description = dr["Description"]?.ToString(),
                    Author = dr["Author"]?.ToString(),
                    ImageUrl = dr["ImageUrl"]?.ToString(),
                    Published = dr["Published"] != DBNull.Value && Convert.ToBoolean(dr["Published"])
                });
            }
        }

        return blogs;
    }

    public BlogsModel GetBlogById(int id)
    {
        BlogsModel blog = null;
        using (SqlConnection con = new SqlConnection(ConnectionString))
        {
            SqlCommand cmd = new SqlCommand("Dynamo.SP_ManageBlogs", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Action", "GETBYID");
            cmd.Parameters.AddWithValue("@BlogId", id);

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                blog = new BlogsModel
                {
                    BlogId = Convert.ToInt32(dr["BlogId"]),
                    Title = dr["Title"]?.ToString(),
                    Category = dr["Category"]?.ToString(),
                    PublishDate = dr["PublishDate"] != DBNull.Value ? Convert.ToDateTime(dr["PublishDate"]) : DateTime.MinValue,
                    Description = dr["Description"]?.ToString(),
                    Author = dr["Author"]?.ToString(),
                    ImageUrl = dr["ImageUrl"]?.ToString(),
                    Published = dr["Published"] != DBNull.Value && Convert.ToBoolean(dr["Published"])
                };
            }
        }
        return blog;
    }

    public string InsertBlog(BlogsModel model)
    {
        using (SqlConnection con = new SqlConnection(ConnectionString))
        {
            SqlCommand cmd = new SqlCommand("Dynamo.SP_ManageBlogs", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Action", "INSERT");
            cmd.Parameters.AddWithValue("@Title", model.Title);
            cmd.Parameters.AddWithValue("@Category", model.Category);
            cmd.Parameters.AddWithValue("@PublishDate", model.PublishDate);
            cmd.Parameters.AddWithValue("@Description", model.Description);
            cmd.Parameters.AddWithValue("@Author", model.Author);
            cmd.Parameters.AddWithValue("@ImageUrl", model.ImageUrl ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Published", model.Published);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        return "Blog inserted successfully.";
    }

    public string UpdateBlog(BlogsModel model)
    {
        using (SqlConnection con = new SqlConnection(ConnectionString))
        {
            SqlCommand cmd = new SqlCommand("Dynamo.SP_ManageBlogs", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Action", "UPDATE");
            cmd.Parameters.AddWithValue("@BlogId", model.BlogId);
            cmd.Parameters.AddWithValue("@Title", model.Title);
            cmd.Parameters.AddWithValue("@Category", model.Category);
            cmd.Parameters.AddWithValue("@PublishDate", model.PublishDate);
            cmd.Parameters.AddWithValue("@Description", model.Description);
            cmd.Parameters.AddWithValue("@Author", model.Author);
            cmd.Parameters.AddWithValue("@ImageUrl", model.ImageUrl ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Published", model.Published);

            con.Open();
            cmd.ExecuteNonQuery();
        }
        return "Blog updated successfully.";
    }

    public string DeleteBlog(int id)
    {
        using (SqlConnection con = new SqlConnection(ConnectionString))
        {
            SqlCommand cmd = new SqlCommand("Dynamo.SP_ManageBlogs", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Action", "DELETE");
            cmd.Parameters.AddWithValue("@BlogId", id);

            con.Open();
            cmd.ExecuteNonQuery();
        }
        return "Blog deleted successfully.";
    }
}
