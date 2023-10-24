using Gaming_Forum;
using Shopping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
//using 其中專題;

namespace DB_GamingForm_Show
{
    internal static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new FrmJobMainPage());
            //Application.Run(new FrmResumeMainPage());
            //Application.Run(new AddProduct());
            Application.Run(new HomePage());
            //Application.Run(new FrmPurchase());
            //Application.Run(new FrmMemberShop());
        }
    }
}
