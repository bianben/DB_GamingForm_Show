using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Data.Entity.Validation;
using DB_GamingForm_Show;
using Image = DB_GamingForm_Show.Image;

namespace Gaming_Forum
{
    public partial class NFrmRegistration : Form
    {
        public NFrmRegistration()
        {
            InitializeComponent();
        }

        DB_GamingFormEntities db = new DB_GamingFormEntities();
        Member_Firm mf = new Member_Firm();        

        private void button1_Click(object sender, EventArgs e)
        {
            if (mf.ResgistedName && mf.Email && mf.Phone && mf.Password)
            {
                try
                {
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    this.pictureBox1.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] imgdata = ms.GetBuffer();

                    DB_GamingForm_Show.Image image = new Image { Name = "image", Image1 = imgdata };                       
                    this.db.Images.Add(image);
                    this.db.SaveChanges();

                    string password = Member_Firm.ClassUtility.HashPassword(this.textBox3.Text);                    
                    Member member = new Member { Name = this.textBox1.Text, Email = this.textBox2.Text, Password = password, Phone = this.textBox4.Text, Birth = this.dateTimePicker1.Value, ImageID = image.ImageID };
                    this.db.Members.Add(member);
                    this.db.SaveChanges();
                    MessageBox.Show($"歡迎{this.textBox1.Text}加入");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("輸入資料不正確，請重新檢查");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.pictureBox1.Image = System.Drawing.Image.FromFile(this.openFileDialog1.FileName);  
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.textBox1.Text = "";
            this.textBox2.Text = "";
            this.textBox3.Text = "";
            this.textBox4.Text = "";
            this.dateTimePicker1.Value = DateTime.Now;
            this.pictureBox1.Image = null;            
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            string input = this.textBox1.Text;
            string result = "";
            mf.CheckName(input, ref result);
            this.label6.Text = result;
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            string input = this.textBox2.Text;
            string result = "";
            mf.CheckEmail(input, ref result);
            this.label7.Text = result;
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            string input = this.textBox3.Text;
            string result = "";
            mf.CheckPassword(input, ref result);
            this.label10.Text = result;
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            string input = this.textBox4.Text;
            string result = "";
            mf.CheckPhone(input, ref result);
            this.label9.Text = result;
        }
    }
}
