using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shopping;
using WindowsFormsApp1;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using DB_GamingForm_Show;
using Gaming_Forum;

namespace Shopping
{
    public partial class FrmPurchase : Form
    {
        DB_GamingFormEntities db = new  DB_GamingFormEntities();    
        List<CShoppingCar> car = new List<CShoppingCar>();
        bool flag = true;
        public int ID { get; set; }
        public bool IsFirm { get; set; }

        private void MemberFirm()
        {
            if (ClassUtility.FirmID != 0)
            {
                ID = ClassUtility.FirmID;
                IsFirm = true;
            }
            else
            {
                ID = ClassUtility.MemberID;
                IsFirm = false;
            }
        }
        public FrmPurchase()
        {
            InitializeComponent();
            MemberFirm();
            //這段可能用不到
            var StringTName = (from p in this.db.ProductTags.AsEnumerable()    
                              select new { 商品ID = p.ProductID, 商品名 = p.Product.ProductName, Picture = p.Product.Image.Image1, 售價 = p.Product.Price, /*標籤 = p.SubTag.Name*/ }).Distinct();
            this.bindingSource1.DataSource = StringTName.ToList();
            this.dataGridView1.DataSource = this.bindingSource1;

            this.pictureBox1.DataBindings.Add("Image", this.bindingSource1, "Picture", true);
            if (IsFirm)
            { this.button4.Visible = false; }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmCar f = new FrmCar();
            f.dataGridView1.DataSource = car;
            f.label6.Text = this.label1.Text;
            f.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var q = from x in db.Products.AsEnumerable()
                    where x.ProductID == (int)this.dataGridView1.CurrentRow.Cells[0].Value
                    select new { x.ProductID, x.ProductName, x.Price, Quantity = 1 };
            if (car.Count() == 0)
            {
                car.Add(new CShoppingCar
                {
                    ProductID = q.First().ProductID,
                    ProductName = q.First().ProductName,
                    Price = q.First().Price,
                    Quantity = 1
                });
            }
            else
            {
                foreach (var n in car)
                {
                    if (n.ProductID == q.First().ProductID)
                    {
                        n.Quantity++;
                        flag = false;
                        break;
                    }
                    else
                    {
                        flag = true;
                    }
                }
                if (flag)
                {
                    car.Add(new CShoppingCar
                    {
                        ProductID = q.First().ProductID,
                        ProductName = q.First().ProductName,
                        Price = q.First().Price,
                        Quantity = 1

                    });
                }
            }

            this.dataGridView2.DataSource = car.ToList();
            this.dataGridView2.Columns["ProductID"].Visible = false;
            int p = (int)car.Sum(x => x.Price * x.Quantity);
            this.label1.Text = "總共:" + p.ToString() + "元";
        }

        private void splitContainer2_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer2_Panel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

         
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e) { }
      

        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            string searchProduct = textBox1.Text;
            
            var StringTName = from p in this.db.ProductTags.AsEnumerable()
                                 where p.Product.ProductName.Contains($"{searchProduct}") || p.SubTag.Name.Contains($"{searchProduct}")
                              select new { 商品ID = p.ProductID, 商品名 = p.Product.ProductName, Picture = p.Product.Image.Image1, 售價 = p.Product.Price, /*標籤 = p.SubTag.Name*/ };
         
            this.bindingSource1.DataSource = StringTName.ToList();
            this.dataGridView1.DataSource = this.bindingSource1;

        }
   
        private void FrmPurchase_Load(object sender, EventArgs e)
        {//讀入comboBox
            var SubTags = from p in db.SubTags
                          where p.TagID == 1
                          select new { p.Name };
            comboBox1.DataSource = SubTags.ToList();
            comboBox1.DisplayMember = "Name";

          

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        { 
            string searchProduct=comboBox1.Text;
            //選標籤後更新商城內容
            var StringTName = from p in this.db.ProductTags.AsEnumerable()
                              where  p.SubTag.Name.Contains($"{searchProduct}")&&p.Product.StatusID ==1
                              orderby p.ProductID
                              select new { 商品ID = p.ProductID, 商品名 = p.Product.ProductName, Picture = p.Product.Image.Image1, 售價 = p.Product.Price, /*標籤 = p.SubTag.Name*/ };

            this.bindingSource1.DataSource = StringTName.ToList();
            this.dataGridView1.DataSource = this.bindingSource1;

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer2_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            var q = (from x in db.WishLists.AsEnumerable()
                     where x.MemberID == ID
                     select new { x.ProductID }).ToList();

            int ptid = (int)this.dataGridView1.CurrentRow.Cells[0].Value;
            bool flag1 = false;
            List<int> qy = new List<int>();
            foreach (var x in q)
            {
                if (x.ProductID != ptid)
                {
                    flag1 = false;
                }
                else
                {
                    flag1 = true;
                    break;
                }
            }
            if (flag1)
            {
                try
                {
                    var wishlist = (from w in db.WishLists
                                    where w.ProductID == ptid
                                    && w.MemberID == ID
                                    select w).First();
                    if (wishlist == null) return;
                    db.WishLists.Remove(wishlist);
                    db.SaveChanges();
                    MessageBox.Show("已移除願望清單:" + this.dataGridView1.CurrentRow.Cells[1].Value.ToString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else
            {
                try
                {
                    var wishlist = new WishList()
                    {
                        ProductID = ptid,
                        MemberID = ID
                    };
                    db.WishLists.Add(wishlist);
                    db.SaveChanges();
                    MessageBox.Show("已加入願望清單:" + this.dataGridView1.CurrentRow.Cells[1].Value.ToString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }
        /// <summary>
        /// /
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        { 
            //{   //獲取選取值
        //    int SelectProID = (int)dataGridView1.Rows[e.RowIndex].Cells["商品ID"].Value;
        //    int MemcerID = 30;


        //    FrmProductEvaluate frmProductEvaluate = new FrmProductEvaluate(SelectProID, MemcerID);
        //    frmProductEvaluate.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
       
        int SelectProID = (int)dataGridView1.CurrentRow.Cells[0].Value;


        FrmProductEvaluate frmProductEvaluate = new FrmProductEvaluate(SelectProID, ID);
        frmProductEvaluate.Show();

    }
    }
}
