using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using DB_GamingForm_Show;
using Image = DB_GamingForm_Show.Image;

namespace Gaming_Forum
{
    public partial class Member_Detail : Form
    {
        public Member_Detail()
        {
            InitializeComponent();             
        }
        DB_GamingFormEntities db = new DB_GamingFormEntities();
        Member_Firm mf = new Member_Firm();
        IEnumerable<Member> _User;
        public void User_Detail(string email)
        {
            _User = from u in db.Members.AsEnumerable()
                    where u.Email == email
                    select u;

            var Un = from u in _User.AsEnumerable()
                     select u.Name;
            this.textBox1.Text = Un.ToList().First().ToString();
            this.textBox1.ReadOnly = true;

            var Ue = from u in _User.AsEnumerable()
                     select u.Email;
            this.textBox2.Text = Ue.ToList().First().ToString();
            this.textBox2.ReadOnly = true;

            var Up = from u in _User.AsEnumerable()
                     select u.Phone;
            this.textBox4.Text = Up.ToList().First().ToString();
            this.textBox4.ReadOnly = true;

            var Ub = from u in _User.AsEnumerable()
                     select u.Birth;
            this.textBox3.Text = Ub.ToList().First().ToString("yyyy/MM/dd");
            this.dateTimePicker1.Hide();
            this.textBox3.ReadOnly = true;

            var Upi = from u in _User.AsEnumerable()
                      select u.Image.Image1;
            byte[] bytes = (byte[])Upi.ToList().First();
            System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
            this.pictureBox1.Image = System.Drawing.Image.FromStream(ms);
            this.button3.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.textBox1.ReadOnly = false;                         
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string input = this.textBox1.Text;
            string result = "";
            mf.CheckName(input, ref result);
            if (mf.ResgistedName)
            {
                try
                {                    
                    Member member = new Member();
                    GetMember(ref member);
                    member.Name = this.textBox1.Text;
                    this.db.Members.AddOrUpdate(member);
                    this.db.SaveChanges();
                    MessageBox.Show("修改暱稱成功");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show(result);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.pictureBox1.Image = System.Drawing.Image.FromFile(this.openFileDialog1.FileName);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.textBox4.ReadOnly = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.textBox3.Hide();
            this.dateTimePicker1.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.button3.Enabled = true;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string input = this.textBox4.Text;
            string result = "";
            mf.CheckPhone(input, ref result);
            if (mf.Phone)
            {
                try
                {
                    Member member = new Member();
                    GetMember(ref member);
                    member.Phone = this.textBox4.Text;
                    this.db.Members.AddOrUpdate(member);
                    this.db.SaveChanges();
                    MessageBox.Show("修改手機成功");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show(result);
            }
        }
        private Member GetMember(ref Member member)
        {
            var Ui = from u in _User.AsEnumerable()
                     select u.MemberID;
            member = (Member)db.Members.Where(x => x.MemberID == Ui.ToList().FirstOrDefault()).Select(x => x).FirstOrDefault();
            return member;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                Member member = new Member();
                GetMember(ref member);
                member.Birth = this.dateTimePicker1.Value;
                this.db.Members.AddOrUpdate(member);
                this.db.SaveChanges();
                MessageBox.Show("修改生日成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (this.pictureBox1.Image != null)
            {
                try
                {
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    this.pictureBox1.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] imgdata = ms.GetBuffer();
                    DB_GamingForm_Show.Image image = new Image { Name = "image", Image1 = imgdata };
                    this.db.Images.Add(image);
                    this.db.SaveChanges();

                    Member member = new Member();
                    GetMember(ref member);

                    var oldimage = db.Images.Where(p => p.ImageID == member.ImageID).Select(p => p).FirstOrDefault();
                    this.db.Images.Remove(oldimage);
                    member.ImageID = image.ImageID;
                    this.db.Members.AddOrUpdate(member);
                    this.db.SaveChanges();
                    MessageBox.Show("修改大頭貼成功");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("未上傳圖片");
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            var Ui = from u in _User.AsEnumerable()
                     select u.MemberID;
            ResetPassword resetPassword = new ResetPassword();
            Member_Firm.ClassUtility.MemberID = Ui.FirstOrDefault();
            resetPassword.Show();
        }
    }
}
