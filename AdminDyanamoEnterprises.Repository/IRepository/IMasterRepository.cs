using AdminDyanamoEnterprises.DTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminDyanamoEnterprises.Repository
{
    public interface IMasterRepository
    {
       
        public void InsertorUpdateCategoryType(CategoryTypePageViewModel addCategoryType);
        public List<CategoryType>GetAllListType();

        public void DeleteCategory(int id);

         
    }
}
