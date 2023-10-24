using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping
{
    public class COrderProduct
    {
        //public int ID { get; set; }
        //public int OrderID { get; set; }
        public string StatusName { get; set; }
        public int ProductID { get; set; }

        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }

        public int Quantinty { get; set; }

        public int UnitStock { get; set; }
        public int Discount { get; set; }

    }
}
