using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminDyanamoEnterprises.DTOs.Master
{
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
}