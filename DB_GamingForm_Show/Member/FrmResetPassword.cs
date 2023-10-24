using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using DB_GamingForm_Show;

namespace Gaming_Forum
{
    public partial class FrmResetPassword : Form
    {
        public FrmResetPassword()
        {
            InitializeComponent();
            this.label5.Visible= false;
            this.label8.Visible= false;
            this.textBox5.Visible= false;
            this.button2.Visible= false;
            this.button3.Visible= false;
            this.label6.Visible= false;
            this.button4.Visible= false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DB_GamingFormEntities db = new DB_GamingFormEntities();
            var FP = from firm in db.Firms
                     where firm.FirmID == ClassUtility.FirmID
                     select firm.FirmName;

            if (this.textBox1.Text == FP.ToList().First().ToString())
            {
                label9.Text="公司名稱正確";
                ClassUtility.FirmName = true;
            }
            else
            {
                label9.Text = "公司名稱錯誤";
                ClassUtility.FirmName = false;
            }

            var FP2 = from firm in db.Firms
                     where firm.FirmID == ClassUtility.FirmID
                     select firm.Email;

            if (this.textBox2.Text == FP2.ToList().FirstOrDefault())
            {
                label10.Text="信箱正確";
                ClassUtility.Email = true;
            }
            else
            {
                this.label10.Text = "信箱錯誤";
                ClassUtility.Email = false;
            }

            var FP3 = from firm in db.Firms
                      where firm.FirmID == ClassUtility.FirmID
                      select firm.TaxID;
            if (this.textBox3.Text == FP3.ToList().FirstOrDefault().ToString())
            {
                this.label11.Text = "統編正確";
                ClassUtility.TaxID = true;
            }
            else 
            {
                this.label11.Text = "統編錯誤";
                ClassUtility.TaxID = false;
            }

            var FP4 = from firm in db.Firms
                      where firm.FirmID == ClassUtility.FirmID
                      select firm.Password;
            if (ClassUtility.HashPassword(this.textBox4.Text) == FP4.ToList().FirstOrDefault())
            {
              this.label12.Text = "密碼正確";
              ClassUtility.Password = true;
            }
            else
            {
                this.label12.Text = "密碼錯誤";
                ClassUtility.Password = false;
            }

            if (ClassUtility.FirmName && ClassUtility.Email && ClassUtility.TaxID && ClassUtility.Password == true)
            {
                
                MessageBox.Show("資料驗證成功");
                this.label5.Visible   = true;
                this.textBox5.Visible = true;
                this.label6.Visible = true;
                this.button3.Visible = true;
                this.button4.Visible = true;
            }
            else
            {
                MessageBox.Show("資料驗證錯誤");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DB_GamingFormEntities db = new DB_GamingFormEntities();
            if (textBox4.Text == textBox5.Text)
            {
                MessageBox.Show("新舊密碼不可相同");
            }
            else
            {
                string result = "";
                ClassUtility Cs = new ClassUtility();
                Cs.CheckPassword(this.textBox5.Text, ref result);
                if (ClassUtility.Password)
                {
                    try
                    {
                        Firm firm = new Firm();
                        firm = (Firm)(from f in db.Firms
                                      where f.FirmID == ClassUtility.FirmID
                                      select f).FirstOrDefault();
                        firm.Password = ClassUtility.HashPassword(this.textBox5.Text);
                        db.Firms.AddOrUpdate(firm);
                        db.SaveChanges();
                        MessageBox.Show("密碼修改成功");
                        button2.Visible = true;
                        this.label8.Visible = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
                else 
                {
                    MessageBox.Show(result);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("信件已寄送");
        }
        public bool flag = true;
        public bool flag1 = true;
        private void button5_Click(object sender, EventArgs e)
        {
            if(flag)
            {
                this.textBox4.PasswordChar = '\0';
            }
            else
            {
                this.textBox4.PasswordChar = '*';
            }
            flag = !flag;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (flag1)
            {
                this.textBox5.PasswordChar = '\0';
            }
            else
            {
                this.textBox5.PasswordChar = '*';
            }
            flag1 = !flag1;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.textBox1.Text = "test";
            this.textBox2.Text = "test@gmail.com";
            this.textBox3.Text = "11223456";
            this.textBox4.Text = "As1234";
        }
    }
}
