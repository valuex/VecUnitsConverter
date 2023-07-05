using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VecUnitsConverter
{
    internal class UnitItem
    {     
        public string UnitFullName { get; set; }
        public string UnitShortName { get; set; }
        public Boolean IsBasicUnitName { get; set; }
        public double EqualsToBasicUnit { get; set; } 
        public Boolean IsFavorite { get; set; }

    }
    class UnitType
    {
        public string UnitTypeName { get; set; }
        public List<UnitItem> Units { get; set; }


    }
}
