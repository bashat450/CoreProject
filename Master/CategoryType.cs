using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminDyanamoEnterprises.DTOs
{
    public class CategoryType
    {
        public string ? CategoryName { get; set; }
        public int CategoryID { get; set; }
    }
    public class AddCategoryType
    {
        public  string ? CategoryName { get; set; }
        public int CategoryID { get; set; }
    }
   
    public class CategoryTypePageViewModel
    {
        public AddCategoryType? AddCategory { get; set; }
        public List<CategoryType> CategoryList { get; set; } = new();
    }

}
