using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping
{
    public class COrder
    {
        public string Status { get; set; }
        public int OrderID { get; set; }

        public int MemberID { get; set; }

        public string ProductName { get; set; }
        public string ShipName { get; set; }
        public int Quantinty { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? PaymentDate { get; set; }

        public DateTime? ShippingDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public int PaymentID { get; set; }
        public string PaymentName { get; set; }
        public int ShipID { get; set; }
        public string ShipMethod { get; set; }
        public int? Zipcode { get; set; }

        public string ShipAddress { get; set; }
        public string Note { get; set; }

        
        public int StatusID { get; set; }

    }
}
