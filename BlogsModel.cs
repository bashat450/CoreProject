using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminDyanamoEnterprises.DTOs.Master
{
    public class BlogsModel
    {
        public int BlogId { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public DateTime PublishDate { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string ImageUrl { get; set; }
        public bool Published { get; set; }
    }

}
