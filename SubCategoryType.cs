using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminDyanamoEnterprises.DTOs.Master
{
    public class SubCategoryType
    {
            public int ? SubCategoryID { get; set; }
            public string? SubCategoryName { get; set; }
            public int CategoryID { get; set; }
            public CategoryType? CategoryName { get; set; }
    }
        public class SubAddCategoryType
        {
            public int? SubCategoryID { get; set; }
            public string? SubCategoryName { get; set; }
            public int CategoryID { get; set; }
        }
        public class SubCategoryTypeJoinModel
        {
            public SubAddCategoryType? SubAddCategory { get; set; }
            public List<SubCategoryType> SubCategoryList { get; set; } = new();
            public List<CategoryType> CategoryList { get; set; } = new();
    }
    
}
