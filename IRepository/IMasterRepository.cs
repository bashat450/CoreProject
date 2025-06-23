using AdminDyanamoEnterprises.DTOs;
using AdminDyanamoEnterprises.DTOs.Common;
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
       
        public MasterResponse InsertorUpdateCategoryType(CategoryTypePageViewModel addCategoryType);
        public List<CategoryType>GetAllCategoryType();

        public void DeleteCategory(int id);
        public MasterResponse InsertOrUpdateOrDeletePattern(PatternTypePageViewModel addPatternType);

        public List<PatternType> GetAllPatternType();

        public void DeletePattern(int id);



        public MasterResponse InsertOrUpdateSubCategory(SubAddCategoryType model);
        public string DeleteSubCategory(int subCategoryId);

        public List<SubCategoryType> GetAllSubCategoriesWithCategoryName();

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
}
