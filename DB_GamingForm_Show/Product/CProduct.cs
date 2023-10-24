using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping
{
    public class CProduct
    {
        public int ProductID { get; set; }

        public string ProductName { get; set; }

        public decimal Price { get; set; }

        public Image Image { get; set; }

        public int UnitStock { get; set; }

        public int StatusID { get; set; }

        public int MemberID { get; set; }
        public int FrimID { get; set; }


    }
}
