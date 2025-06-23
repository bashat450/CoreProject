using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminDyanamoEnterprises.DTOs.Master
{
       public class PatternType
       {   
            public string? PatternName { get; set; }
            public int PatternID { get; set; }
        }
        public class AddPatternType
    {
            public string? PatternName { get; set; }
            public int PatternID { get; set; }
        }

        public class PatternTypePageViewModel
        {
            public AddPatternType? AddPattern { get; set; }
            public List<PatternType> PatternList { get; set; } = new();
        }
}

