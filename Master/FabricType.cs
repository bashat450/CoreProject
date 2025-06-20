using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminDyanamoEnterprises.DTOs.Master
{
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
}