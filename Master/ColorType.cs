using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminDyanamoEnterprises.DTOs.Master
{
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
}