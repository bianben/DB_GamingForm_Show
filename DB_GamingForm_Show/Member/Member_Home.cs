using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DB_GamingForm_Show;
//using DB_GamingForm_Show.Member;
using Groot;
using Shopping;

namespace Gaming_Forum
{
    public partial class Member_Home : Form
    {
        public Member_Home()
        {
            InitializeComponent();
        }
        DB_GamingFormEntities db = new DB_GamingFormEntities();
        IEnumerable<Member> _User;

        public IEnumerable<Member> MemberWho(string email)
        {
            _User = from u in db.Members.AsEnumerable()
                    where u.Email == email
                    select u;
            
            var user = from u in _User
                       select u.Name;
            this.label1.Text = $"你好，{user.ElementAt(0)}";

            var Upi = from u in _User.AsEnumerable()
                      select u.Image.Image1;
            byte[] bytes = (byte[])Upi.ToList().First();
            System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
            this.pictureBox1.Image = System.Drawing.Image.FromStream(ms);

            return _User;            
        }     

        private void button1_Click(object sender, EventArgs e)
        {
            var user = from u in _User
                       select u.Email;
            Member_Detail member_Detail = new Member_Detail();
            member_Detail.User_Detail(user.ElementAt(0).ToString());
            member_Detail.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var q = from u in _User
                    select u.MemberID;

            Member_Firm.ClassUtility.MemberID = q.FirstOrDefault();

            var user = from u in this.db.Articles.AsEnumerable()
                       where u.MemberID == Member_Firm.ClassUtility.MemberID
                       select new { 發表板塊 = u.SubBlog.Title, 文章編號 = u.ArticleID, 文章標題 = u.Title, 發表時間 = u.ModifiedDate.Date };
            this.dataGridView1.DataSource = user.ToList();
            this.dataGridView1.Columns["文章編號"].Visible = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var q = from u in _User
                    select u.MemberID;

            Member_Firm.ClassUtility.MemberID = q.FirstOrDefault();

            var user = from u in this.db.Replies.AsEnumerable()
                       where u.MemberID == Member_Firm.ClassUtility.MemberID
                       select new { 回覆文章 = u.Article.Title, 回覆內容 = u.ReplyContents, 回覆時間 = u.ModifiedDate, 回覆編號 = u.ReplyID };
            this.dataGridView1.DataSource = user.ToList();
            this.dataGridView1.Columns["回覆編號"].Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FrmMemberShop f = new FrmMemberShop();
            f.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FrmMakeResume f = new FrmMakeResume();
            f.Show();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var q = from a in this.db.Articles.AsEnumerable()
                    where a.ArticleID == (int)this.dataGridView1.CurrentRow.Cells["文章編號"].Value
                    select new {文章標題 = a.Title, 文章內容 = a.ArticleContent };
            ShowArticle aa = new ShowArticle();
            aa.textBox1.Text = q.FirstOrDefault().文章標題;
            aa.textBox2.Text = q.FirstOrDefault().文章內容;
            aa.Show();
        }
    }
}
