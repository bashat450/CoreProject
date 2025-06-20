using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminDyanamoEnterprises.DTOs.Master
{
    public class SubCategoryType
    {
        public class SubsCategoryType
        {
            public int ? SubCategoryID { get; set; }
            public string? SubCategoryName { get; set; }
            public int CategoryID { get; set; }
        }
        public class SubAddCategoryType
        {
            public int? SubCategoryID { get; set; }
            public string? SubCategoryName { get; set; }
            public int CategoryID { get; set; }
        }
        public class SubCategoryTypeJoinModel
        {
            public SubAddCategoryType? SubCategory { get; set; }
            public List<SubsCategoryType> SubCategoryList { get; set; } = new();
        }
    }
}
