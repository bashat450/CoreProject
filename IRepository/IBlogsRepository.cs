using AdminDyanamoEnterprises.DTOs.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminDyanamoEnterprises.Repository
{
    public interface IBlogsRepository
    {
        IEnumerable<BlogsModel> GetAllBlogs();
        BlogsModel GetBlogById(int id);
        string InsertBlog(BlogsModel model);
        string UpdateBlog(BlogsModel model);
        string DeleteBlog(int id);
    }

}
