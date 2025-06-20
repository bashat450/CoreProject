using AdminDyanamoEnterprises.DTOs;
using AdminDyanamoEnterprises.DTOs.Master;
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

    #region==================IColor Repository==============================================
    public interface IColorRepository
    {
        List<ColorType> GetAllListColorType();

        string Sp_InsertOrUpdateOrDeleteColor(ColorTypePageViewModel colorType);

        string DeleteColor(int id);
    }
    #endregion

    #region==================IFabric Repository==============================================
    public interface IFabricRepository
    {
        List<FabricType> GetAllListFabricType();

        string Sp_InsertOrUpdateOrDeleteFabric(FabricTypePageViewModel fabricType);

        string DeleteFabric(int id);
    }
    #endregion
    #region==================IMaterial Repository==============================================
    public interface IMaterialRepository
    {
        List<MaterialType> GetAllListMaterialType();

        string Sp_InsertOrUpdateOrDeleteMaterialType(MaterialTypePageViewModel materialType);

        string DeleteMaterial(int id);
    }
    #endregion
   
}
