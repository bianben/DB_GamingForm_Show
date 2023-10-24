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
using Gaming_Forum;

namespace Shopping
{
    public partial class FrmCar : Form
    {
        DB_GamingFormEntities db = new DB_GamingFormEntities();
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
        public FrmCar()
        {
            InitializeComponent();
            this.label4.Visible = false;
            this.textBox2.Visible = false;
            this.textBox3.Visible = false;
            this.textBox5.Visible = false;
            this.button2.Visible = false;
            this.button4.Visible = false;
            LoadToComboBox();
            MemberFirm();
        }

        private void LoadToComboBox()
        {
            var q = (from x in db.Payments
                     select x.Name).ToList();
            foreach (var x in q)
            {
                this.comboBox1.Items.Add(x);
            }

            var q2 = (from x in db.ShipMethods
                      select x.Name).ToList();
            foreach (var x in q2)
            {
                this.comboBox2.Items.Add(x);
            }

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBox2.Text == "郵局" || this.comboBox2.Text == "黑貓宅急便")
            {
                this.label4.Visible = true;
                this.textBox2.Visible = true;
                this.textBox3.Visible = true;
                this.textBox5.Visible = true;
                this.button2.Visible = true;
                this.button4.Visible = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.textBox2.Text == null)
            {
                MessageBox.Show("請輸入郵遞區號");
            }
            else
            {
                int zip = int.Parse(this.textBox2.Text);
                var q = (from x in db.RegionDistricts.AsEnumerable()
                         where x.Zipcode == zip
                         select new { x.Region.City, x.District }).ToList();
                foreach (var x in q)
                {
                    this.textBox5.Text = x.City.ToString().Trim() + x.District.ToString();
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool flag = false;
            var paymant = from x in db.Payments.AsEnumerable()
                          where x.Name == this.comboBox1.Text
                          select new { x.PaymentID, x.Name };
            var ship = from x in db.ShipMethods.AsEnumerable()
                       where x.Name == this.comboBox2.Text
                       select new { x.ShipID, x.Name };
            for (int i1 = 0; i1 < this.dataGridView1.Rows.Count; i1++)
            {
                var qu = from x in db.Products.AsEnumerable()
                         where x.ProductID == (int)this.dataGridView1.Rows[i1].Cells[0].Value
                         select new { x.UnitStock };
                int prush = (int)this.dataGridView1.Rows[i1].Cells[3].Value;
                int quan = qu.First().UnitStock;
                int a = quan - prush;

                if (a < 0)
                {
                    MessageBox.Show("庫存不足");
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
                if (IsFirm)
                {
                    try
                    {
                        if (this.comboBox2.Text == "郵局" || this.comboBox2.Text == "黑貓宅急便")
                        {
                            var order = new Order
                            {
                                FirmID = ID,
                                ShipName = this.textBox1.Text,
                                OrderDate = DateTime.Now,
                                PaymentID = paymant.First().PaymentID,
                                ShipID = ship.First().ShipID,
                                Zipcode = int.Parse(this.textBox2.Text),
                                ShipAddress = this.textBox5.Text + this.textBox3.Text,
                                Note = "",
                                StatusID = 13,
                            };
                            this.db.Orders.Add(order);
                            db.SaveChanges();
                        }
                        else
                        {
                            var order = new Order
                            {
                                FirmID = ID,
                                ShipName = this.textBox1.Text,
                                OrderDate = DateTime.Now,
                                PaymentID = paymant.First().PaymentID,
                                ShipID = ship.First().ShipID,
                                Note = "",
                                StatusID = 13,
                            };
                            this.db.Orders.Add(order);
                            db.SaveChanges();
                        }
                        List<COrderProduct> orderProducts = new List<COrderProduct>();
                        for (int i = 0; i < this.dataGridView1.RowCount; i++)
                        {
                            orderProducts.Add(new COrderProduct
                            {
                                ProductID = (int)this.dataGridView1.Rows[i].Cells[0].Value,
                                UnitPrice = (decimal)this.dataGridView1.Rows[i].Cells[2].Value,
                                Quantinty = (int)this.dataGridView1.Rows[i].Cells[3].Value,
                                Discount = 1
                            });
                        }

                        int orderid = db.Orders.OrderByDescending(x => x.OrderID).Select(x => x.OrderID).FirstOrDefault();
                        for (int i = 0; i < orderProducts.Count(); i++)
                        {
                            var orderproduct = new OrderProduct
                            {
                                OrderID = orderid,
                                ProductID = orderProducts[i].ProductID,
                                UnitPrice = orderProducts[i].UnitPrice,
                                Quantinty = orderProducts[i].Quantinty,
                                Disconut = orderProducts[i].Discount
                            };
                            this.db.OrderProducts.Add(orderproduct);

                            var us = from x in db.Products
                                     where x.ProductID == orderproduct.ProductID
                                     select x;
                            Product unitstock = db.Products.First(x => x.ProductID == orderproduct.ProductID);
                            unitstock.UnitStock = unitstock.UnitStock - orderproduct.Quantinty;
                            db.SaveChanges();
                        }
                            MessageBox.Show("訂單成功");
                        }
            catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    try
                    {
                        if (this.comboBox2.Text == "郵局" || this.comboBox2.Text == "黑貓宅急便")
                        {
                            var order = new Order
                            {
                                MemberID = ID,
                                ShipName = this.textBox1.Text,
                                OrderDate = DateTime.Now,
                                PaymentID = paymant.First().PaymentID,
                                ShipID = ship.First().ShipID,
                                Zipcode = int.Parse(this.textBox2.Text),
                                ShipAddress = this.textBox5.Text + this.textBox3.Text,
                                Note = "",
                                StatusID = 13,
                            };
                            this.db.Orders.Add(order);
                            db.SaveChanges();
                        }
                        else
                        {
                            var order = new Order
                            {
                                MemberID = ID,
                                ShipName = this.textBox1.Text,
                                OrderDate = DateTime.Now,
                                PaymentID = paymant.First().PaymentID,
                                ShipID = ship.First().ShipID,
                                Note = "",
                                StatusID = 13,
                            };
                            this.db.Orders.Add(order);
                            db.SaveChanges();
                        }


                        List<COrderProduct> orderProducts = new List<COrderProduct>();
                        for (int i = 0; i < this.dataGridView1.RowCount; i++)
                        {
                            orderProducts.Add(new COrderProduct
                            {
                                ProductID = (int)this.dataGridView1.Rows[i].Cells[0].Value,
                                UnitPrice = (decimal)this.dataGridView1.Rows[i].Cells[2].Value,
                                Quantinty = (int)this.dataGridView1.Rows[i].Cells[3].Value,
                                Discount = 1
                            });
                        }

                        int orderid = db.Orders.OrderByDescending(x => x.OrderID).Select(x => x.OrderID).FirstOrDefault();
                        for (int i = 0; i < orderProducts.Count(); i++)
                        {
                            var orderproduct = new OrderProduct
                            {
                                OrderID = orderid,
                                ProductID = orderProducts[i].ProductID,
                                UnitPrice = orderProducts[i].UnitPrice,
                                Quantinty = orderProducts[i].Quantinty,
                                Disconut = orderProducts[i].Discount
                            };
                            this.db.OrderProducts.Add(orderproduct);

                            var us = from x in db.Products
                                     where x.ProductID == orderproduct.ProductID
                                     select x;
                            Product unitstock = db.Products.First(x => x.ProductID == orderproduct.ProductID);
                            unitstock.UnitStock = unitstock.UnitStock - orderproduct.Quantinty;
                            db.SaveChanges();
                        }

                        MessageBox.Show("訂單成功");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

            }
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            MessageBox.Show(this.dataGridView1.CurrentRow.Cells[0].Value.ToString());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.textBox2.Text = "106";
            this.textBox5.Text = "臺北市大安區";
            this.textBox3.Text = "和平東路二段106號";
        }
    }
}
