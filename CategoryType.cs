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

    #region===============Color Type ========================================
    public class ColorType
    {
        public int ColorID { get; set; }
        public string ColorName { get; set; }
    }
    public class AddColorType
    {
        public int ColorID { get; set; }
        public string? ColorName { get; set; }

    }

    public class ColorTypePageViewModel
    {
        public AddColorType? AddColor { get; set; }
        public List<ColorType> ColorList { get; set; } = new();
    }
    #endregion

    #region===============Fabric Type ========================================
    public class FabricType
    {
        public int FabricID { get; set; }
        public string FabricName { get; set; }
    }
    public class AddFabricType
    {
        public int FabricID { get; set; }
        public string? FabricName { get; set; }

    }

    public class FabricTypePageViewModel
    {
        public AddFabricType? AddFabric { get; set; }
        public List<FabricType> FabricList { get; set; } = new();
    }
    #endregion
    #region===============Material Type ========================================
    public class MaterialType
    {
        public int MaterialID { get; set; }
        public string MaterialName { get; set; }
    }
    public class AddMaterialType
    {
        public int MaterialID { get; set; }
        public string? MaterialName { get; set; }

    }

    public class MaterialTypePageViewModel
    {
        public AddMaterialType? AddMaterial { get; set; }
        public List<MaterialType> MaterialList { get; set; } = new();
    }
    #endregion

}
