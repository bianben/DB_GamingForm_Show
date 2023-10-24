
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DB_GamingForm_Show;
using DBGaming;
using Shopping;

namespace Gaming_Forum
{
    public partial class HomePage : Form
    {
        DB_GamingFormEntities db = new DB_GamingFormEntities();
        public HomePage()
        {
            InitializeComponent();
            this.button8.Visible = false;//廠商個人資料
            this.button9.Visible = false;//會員個人資料 登入後設成true
            label8.Visible = false;
            this.button10.Visible = false;
        }
        
        
        private void button5_Click(object sender, EventArgs e)
        {
            FirmFrmRegistration Freg = new FirmFrmRegistration();
            Freg.Show(this);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            NFrmRegistration reg = new NFrmRegistration();
            reg.Show(this);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (db.Members.Any(m => m.Email == this.textBox1.Text))
            {
                var q = from p in db.Members
                        where p.Email == this.textBox1.Text
                        select p.Password;
                var c = q.ToList().FirstOrDefault();

                var q1 = from p in db.Members
                         where p.Email == this.textBox1.Text
                         select p.Name;
                string MemberName = q1.ToList().FirstOrDefault().ToString();
                if (Member_Firm.ClassUtility.HashPassword(this.textBox2.Text) == c)
                {
                    MessageBox.Show("登入成功");
                    button9.Visible = true;
                    label8.Visible=true;
                    label8.Text = $"歡迎回來,{MemberName}";
                    splitContainer1.Visible = false;
                    button10.Visible = true;
                    var q2 = from p in db.Members
                             where p.Email == this.textBox1.Text
                             select p.MemberID;
                    ClassUtility.MemberID = q2.ToList().FirstOrDefault();
                }
                else
                {
                    MessageBox.Show("密碼錯誤");
                }
            }
            else
            {
                MessageBox.Show("此帳號尚未註冊");
            }
        }
        private void button7_Click(object sender, EventArgs e)
        {
            string FirmAccount = this.textBox1.Text;
            string Password = this.textBox2.Text;

            
            
             if (db.Firms.Any(Firm => Firm.Email == textBox1.Text))
            {
                var Q = from firm in db.Firms
                        where firm.Email == FirmAccount
                        select firm.Password;
                var C = Q.ToList().FirstOrDefault();

                if (ClassUtility.HashPassword(this.textBox2.Text) == C)
                {
                    var N = from firm in db.Firms
                            where firm.Email == FirmAccount
                            select firm.FirmName;
                    this.label8.Text = $"歡迎回來 {N.ToList().First()}";

                    var ID = from firm in db.Firms
                             where firm.Email == FirmAccount
                             select firm.FirmID;
                    MessageBox.Show("登入成功");
                    //=====================================Test Succeeded
                    label8.Visible = true;
                    splitContainer1.Visible = false;
                    this.button1.Visible = false;
                    this.button8.Visible = true;
                    this.button9.Visible = false;
                    this.button10.Visible = true;
                    this.label1 .Visible = false;
                    this.pictureBox1.Visible = false;
                    ClassUtility.FirmID = ID.ToList().First();
                }
                else if (ClassUtility.HashPassword(this.textBox2.Text) != C)
                {
                    MessageBox.Show("請重新確認密碼");
                }
           
            }
            else
            {
                MessageBox.Show("此帳號尚未註冊");
            }

        }
        //=========================廠商資料頁面
        private void button8_Click(object sender, EventArgs e)
        {
            FrmFirmProfiles FP = new FrmFirmProfiles();
            var Q = from firm in db.Firms
                    where firm.FirmID == ClassUtility.FirmID
                    select firm.Email;
            string email = Q.ToList().First().ToString();
            FP.FirmProfile(email);
            FP.Show(this);
        }
        //=======================以下為各平台入口 預設廠商/會員ID皆為0 若登入則程式將自動擷取註冊成員之ID以供各平台串接使用
        private void button1_Click(object sender, EventArgs e)
        {
            //================================ 討論版入口
            if(ClassUtility.FirmID == 0 && ClassUtility.MemberID==0)
            {
                MessageBox.Show("請先登入會員");
            }
            else
            {
                MessageBox.Show("正在帶您前往頁面");
                FormHome f = new FormHome(ClassUtility.MemberID);
                f.Show();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //===================================商城入口
            FrmPurchase p = new FrmPurchase();
            
            if (ClassUtility.FirmID == 0 && ClassUtility.MemberID == 0)
            {
                MessageBox.Show("請先登入會員");
            }
            else
            {
                //MessageBox.Show("正在帶您前往頁面");
                p.Show();
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //==================================媒合平台入口
            bool Flag1 = false;
            bool Flag2 = false;
            if (ClassUtility.FirmID != 0 && ClassUtility.MemberID == 0) 
            {
                Flag1 = true;
                Flag2 = false;
            }

            if (ClassUtility.FirmID == 0 && ClassUtility.MemberID != 0)
            { 
                Flag1 = false;
                Flag2 = true;
            }

            if (ClassUtility.FirmID == 0 && ClassUtility.MemberID == 0)
            {
                MessageBox.Show("請先登入會員");
            }

            else if (Flag1 == true && Flag2 == false)
            {
                MessageBox.Show("親愛的廠商您好,正帶您前往人才搜尋平台");
                FrmResumeMainPage FRP = new FrmResumeMainPage();
                FRP.Show();
            }

            else if (Flag1 == false && Flag2 == true)
            {
                MessageBox.Show("親愛的會員您好,正帶您前往履歷投遞平台");                
                FrmJobMainPage FJB = new FrmJobMainPage();
                FJB.Show();
            }
            else 
            {
                MessageBox.Show("請重新確認是否登入成功");
            }
           
        }

        private void button10_Click(object sender, EventArgs e)
        {
            ClassUtility.FirmID = 0;
            ClassUtility.MemberID = 0;
            MessageBox.Show("正在登出");
            splitContainer1.Visible = true;
            this.button8.Visible = false;
            this.button10.Visible = false;
            this.label8.Visible = false;
            this.pictureBox1.Visible = true;
            this.label1.Visible = true;
            this.button1.Visible = true;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Member_Home member_Home = new Member_Home();
            member_Home.MemberWho(this.textBox1.Text);
            member_Home.Show(this);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            this.textBox1.Text = "123123@gmail.com";
            this.textBox2.Text = "asas12345678";
        }

        private void button12_Click(object sender, EventArgs e)
        {
            this.textBox1.Text = "ben@gmail.com";
            this.textBox2.Text = "As123456";
        }
    }
}
