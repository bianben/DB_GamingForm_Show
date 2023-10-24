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
using DB_GamingForm_Show;

namespace Gaming_Forum
{
    public partial class ResetPassword : Form
    {
        public ResetPassword()
        {
            InitializeComponent();
            this.textBox3.ReadOnly = true;
            this.button2.Enabled = false;
        }
        DB_GamingFormEntities db = new DB_GamingFormEntities();
        Member_Firm mb = new Member_Firm();
        
        bool Uname {  get; set; }
        bool Uemail { get; set; }
        bool Upassword { get; set; }
        bool Uphone { get; set; }
        bool Ubirth { get; set; }

        private void button1_Click(object sender, EventArgs e)
        {
            var un = from n in this.db.Members
                     where n.MemberID == Member_Firm.ClassUtility.MemberID
                     select n.Name;

            if (this.textBox1.Text == un.ToList().FirstOrDefault())
            {
                this.label6.Text = "暱稱正確";
                Uname = true;
            }
            else
            {
                this.label6.Text = "暱稱錯誤";
                Uname = false;
            }

            var ue = from em in this.db.Members
                     where em.MemberID == Member_Firm.ClassUtility.MemberID
                     select em.Email;

            if (this.textBox2.Text == ue.ToList().FirstOrDefault())
            {
                this.label7.Text = "信箱正確";
                Uemail = true;
            }
            else
            {
                this.label7.Text = "信箱錯誤";
                Uemail = false;
            }

            var upa = from pa in this.db.Members
                      where pa.MemberID == Member_Firm.ClassUtility.MemberID
                      select pa.Password;

            if (Member_Firm.ClassUtility.HashPassword(this.textBox5.Text) == upa.ToList().FirstOrDefault())
            {
                this.label14.Text = "密碼正確";
                Upassword = true;
            }
            else
            {
                this.label14.Text = "密碼錯誤";
                Upassword = false;
            }

            var up = from p in this.db.Members
                     where p.MemberID == Member_Firm.ClassUtility.MemberID
                     select p.Phone;

            if (this.textBox4.Text == up.ToList().FirstOrDefault())
            {
                this.label9.Text = "手機正確";
                Uphone = true;
            }
            else
            {
                this.label9.Text = "手機錯誤";
                Uphone = false;
            }

            var ub = from b in this.db.Members
                     where b.MemberID == Member_Firm.ClassUtility.MemberID
                     select b.Birth;            

            if (this.dateTimePicker1.Value.ToString("yyyy/MM/dd") == ub.ToList().FirstOrDefault().ToString("yyyy/MM/dd"))
            {
                this.label13.Text = "生日正確";
                Ubirth = true;
            }
            else
            {
                this.label13.Text = "生日錯誤";
                Ubirth = false;
            }

            if (Uname && Uemail && Upassword && Uphone && Ubirth)
            {
                this.textBox3.ReadOnly = false;
                this.button2.Enabled = true;
                MessageBox.Show("資料驗證成功");
            }
            else
            {
                MessageBox.Show("資料驗證錯誤");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.textBox3.Text == this.textBox5.Text)
            {
                MessageBox.Show("新舊密碼不可相同");
            }
            else
            {
                string result = "";
                mb.CheckPassword(this.textBox3.Text, ref result);
                if (mb.Password)
                {
                    try
                    {
                        Member member = new Member();
                        member = (Member)(from m in this.db.Members
                                          where m.MemberID == Member_Firm.ClassUtility.MemberID
                                          select m).FirstOrDefault();
                        member.Password = Member_Firm.ClassUtility.HashPassword(this.textBox3.Text);
                        this.db.Members.AddOrUpdate(member);
                        this.db.SaveChanges();
                        MessageBox.Show("密碼修改成功");

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

        private void button3_Click(object sender, EventArgs e)
        {
            this.textBox1.Text = "虎斑貓小趴";
            this.textBox2.Text = "123123@gmail.com";
            this.textBox5.Text = "asas12345678";
            this.textBox4.Text = "0912345667";
            DateTime dateTime = new DateTime(2023, 10, 18);
            this.dateTimePicker1.Value = dateTime;
        }
        public bool flag =true;
        public bool flag1 = true;
        private void button4_Click(object sender, EventArgs e)
        {   
            if (flag)
            {
                this.textBox5.PasswordChar = '\0';
            }
            else
            {
                this.textBox5.PasswordChar = '*';
            }
            flag = !flag;

        }

        private void button5_Click(object sender, EventArgs e)
        {   
            if (flag1)
            {
                this.textBox3.PasswordChar = '\0';
            }
            else
            {
                this.textBox3.PasswordChar = '*';
            }
            flag1 = !flag1;
        }
    }
}
