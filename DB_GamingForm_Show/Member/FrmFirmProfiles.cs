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
using Image = DB_GamingForm_Show.Image;

namespace Gaming_Forum
{
    public partial class FrmFirmProfiles : Form
    {
        public FrmFirmProfiles()
        {
            InitializeComponent();
            DB_GamingFormEntities db = new DB_GamingFormEntities();
            var Q = from firm in db.Firms
                    where firm.FirmID == ClassUtility.FirmID
                    select firm.FirmName;
            textBox9.Text = (ClassUtility.FirmID).ToString();
            string FirmName = Q.ToList().First().ToString();
            label20.Text = $"{FirmName}的資料頁面";
        }
        IEnumerable<Firm> _Firms;
        DB_GamingFormEntities db = new DB_GamingFormEntities();
        public void FirmProfile(string email)
        {
            _Firms = from firm in db.Firms.AsEnumerable()
                     where firm.Email == email
                     select firm;

            var FP1 = from firm in _Firms.AsEnumerable()
                      select firm.FirmName;
            this.textBox1.Text = FP1.ToList().First().ToString();
            this.textBox1.ReadOnly = true;

            var FP2 = from firm in _Firms.AsEnumerable()
                      select firm.TaxID;
            this.textBox2.Text = FP2.ToList().First().ToString();
            this.textBox2.ReadOnly = true;

            var FP3 = from firm in _Firms.AsEnumerable()
                      select firm.Contact;
            this.textBox3.Text = FP3.ToList().First().ToString();
            this.textBox3.ReadOnly = true;

            var FP4 = from firm in _Firms.AsEnumerable()
                      select firm.FirmAddress;
            this.textBox4.Text = FP4.ToList().First().ToString();
            this.textBox4.ReadOnly = true;

            var FP5 = from firm in _Firms.AsEnumerable()
                      select firm.FirmIntro;
            this.textBox5.Text = FP5.ToList().First().ToString();
            this.textBox5.ReadOnly = true;

            var FP6 = from firm in _Firms.AsEnumerable()
                      select firm.FirmScale;
            this.textBox6.Text = FP6.ToList().First().ToString();
            this.textBox6.ReadOnly = true;

            var FP7 = from firm in _Firms.AsEnumerable()
                      select firm.Email;
            this.textBox7.Text = FP7.ToList().First().ToString();
            this.textBox7.ReadOnly = true;

            var FP8 = from firm in _Firms.AsEnumerable()
                      select firm.Image.Image1;
            byte[] bytes = (byte[])FP8.ToList().First();
            System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
            this.pictureBox1.Image = System.Drawing.Image.FromStream(ms);
            this.button3.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.textBox4.ReadOnly = false;
            this.textBox5.ReadOnly = false;
            this.textBox6.ReadOnly = false;
            this.textBox1.ReadOnly = false;
            this.textBox3.ReadOnly = false;
            this.button3.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ClassUtility classUtility = new ClassUtility();
            string input = this.textBox1.Text;
            string result = "";
            classUtility.CheckName(input, ref result);
            if (ClassUtility.FirmName)
            {
                try
                {
                    ClassUtility Cs = new ClassUtility();
                    Firm firm = new Firm();
                    Cs.GetFirm(ref firm);
                    firm.FirmName = this.textBox1.Text;
                    this.db.Firms.AddOrUpdate(firm);
                    this.db.SaveChanges();
                    MessageBox.Show("名稱修改成功");
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

        private void button6_Click(object sender, EventArgs e)
        {
            ClassUtility Cs = new ClassUtility();
            string input = this.textBox3.Text;
            string result = "";
            Cs.CheckPhone(input, ref result);
            if (ClassUtility.Phone)
            {
                try
                {
                    Firm firm = new Firm();
                    Cs.GetFirm(ref firm);
                    firm.Contact = this.textBox3.Text;
                    this.db.Firms.AddOrUpdate(firm);
                    this.db.SaveChanges();
                    MessageBox.Show("電話修改成功");
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

        private void button7_Click(object sender, EventArgs e)
        {
            ClassUtility Cs = new ClassUtility();
            string input = this.textBox4.Text;
            string result = "";
            Cs.CheckAddress(input, ref result);
            if (ClassUtility.FirmAddress)
            {
                try
                {
                    Firm firm = new Firm();
                    Cs.GetFirm(ref firm);
                    firm.FirmAddress = this.textBox4.Text;
                    this.db.Firms.AddOrUpdate(firm);
                    this.db.SaveChanges();
                    MessageBox.Show("地址修改成功");
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

        private void button9_Click(object sender, EventArgs e)
        {
            ClassUtility Cs = new ClassUtility();
            string input = this.textBox6.Text;
            string result = "";
            Cs.CheckFirmScale(input, ref result);
            if (ClassUtility.FirmScale) 
            {
                try
                {
                    Firm firm = new Firm();
                    Cs.GetFirm(ref firm);
                    firm.FirmScale = this.textBox6.Text;
                    this.db.Firms.AddOrUpdate(firm);
                    this.db.SaveChanges();
                    MessageBox.Show("人數規模修改成功");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (this.pictureBox1.Image != null)
                try
                {
                    ClassUtility Cs = new ClassUtility();
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    this.pictureBox1.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] Imgdata = ms.GetBuffer();
                    DB_GamingForm_Show.Image image = new Image { Name = "image", Image1 = Imgdata };
                    this.db.Images.Add(image);
                    this.db.SaveChanges();

                    Firm firm = new Firm();
                    Cs.GetFirm(ref firm);

                    var oldimage = db.Images.Where(p => p.ImageID == firm.ImageID).Select(p => p).FirstOrDefault();
                    this.db.Images.Remove(oldimage);
                    firm.ImageID = image.ImageID;
                    this.db.Firms.AddOrUpdate(firm);
                    this.db.SaveChanges();
                    MessageBox.Show("公司圖片修改成功");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            else 
            {
                MessageBox.Show("請上傳圖片");
            }

        }

        private void button11_Click(object sender, EventArgs e)
        {
            FrmResetPassword f = new FrmResetPassword();
            f.Show(this);
        }

        
    }
}
