using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DB_GamingForm_Show;

namespace Shopping
{
    public partial class FrmUpdateProduct : Form
    {
        DB_GamingFormEntities db = new DB_GamingFormEntities();
        public FrmUpdateProduct()
        {
            InitializeComponent();
        }

       
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int productid = int.Parse(this.textBox5.Text);
                Product product = db.Products.First(x => x.ProductID == productid);
                product.ProductID = productid;
                product.ProductName = this.textBox1.Text;
                product.Price = decimal.Parse(this.textBox2.Text);
                product.UnitStock = int.Parse(this.textBox3.Text);
                db.SaveChanges();
                MessageBox.Show("修改成功");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
        }
    }
}
