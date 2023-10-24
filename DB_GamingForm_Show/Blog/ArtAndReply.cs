using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using DB_GamingForm_Show;
using Gaming_Forum;
using System.Diagnostics.Eventing.Reader;

namespace WindowsFormsApp1
{
    public partial class Art_Reply : Form
    {
        DB_GamingFormEntities db = new DB_GamingFormEntities();
        //DB_GamingFormEntities db = new DB_GamingFormEntities();
        //輸入標題，內文，aid抓會員ID
        public Art_Reply(string title, string articleContent, int aid)
        {

            InitializeComponent();


            textBox2.Text = title;
            textBox3.Text = articleContent;
            //------------------------------
            var re = from q in db.Replies
                     where q.ArticleID == ClassUtility.aid
                     select new { 會員編號 = q.MemberID, 作者名稱 = q.Member.Name, 帳號 = q.MemberID, 內容 = q.ReplyContents, 更新時間 = q.ModifiedDate , 回文 = q.ReplyID };

            dataGridView1.DataSource = re.ToList();
            //------------------------------
            this.textBox2.Enabled = false;
            this.textBox3.Enabled = false;


            //var memberInfo = from q in db.Members
            //                 where q.MemberID == ClassUtility.MemberID
            //                 select q.Email;

            //MessageBox.Show($"{Class1.aid}");

            //MessageBox.Show(memberInfo.ToList().FirstOrDefault());

            var memberInfo = from q in db.Articles
                             where q.ArticleID == aid
                             select q.Member.Email;

            label3.Text = memberInfo.ToList().FirstOrDefault();

            //if (memberInfo.Any()) // 檢查是否有找到符合條件的資料
            //{
            //    var firstMember = memberInfo.First(); 

            //    label3.Text = firstMember.Email; 
            //}
            //------------------------------

            var sub = db.Articles
                        .Where(s => s.ArticleID == ClassUtility.aid)
                        .Select(s => s).FirstOrDefault();


            if (sub.MemberID == ClassUtility.MemberID)
            {
                textBox2.Enabled = true;
                textBox3.Enabled = true;

            }




            if (textBox2.Text == "該文章已刪除")
            {
                this.button1.Enabled = false;
                this.button2.Enabled = false;
                this.button3.Enabled = false;
                this.textBox1.Enabled = false;
            }

            //--------------------------------
            if (dataGridView1.RowCount == 0)
            {
                this.button4.Enabled = false;
                this.button5.Enabled = false;
            }
            //--------------------------------

            var su = db.Articles
            .Where(s => s.ArticleID == ClassUtility.aid)
            .Select(s => s).FirstOrDefault();


            if (sub.MemberID != ClassUtility.MemberID)
            {
                button2.Enabled = false;
                button3.Enabled = false;

            }



        }

        private void reload()
        {
            var re = from q in db.Replies
                     where q.ArticleID == ClassUtility.aid
                     select new { 會員編號 = q.MemberID, 作者名稱 = q.Member.Name, 帳號 = q.Member.Email, 內容 = q.ReplyContents, 更新時間 = q.ModifiedDate, 回文 = q.ReplyID };

            dataGridView1.DataSource = re.ToList();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Reply reply = new Reply
            {
                ReplyContents = textBox1.Text,
                ModifiedDate = DateTime.Now,
                MemberID = ClassUtility.MemberID,
                ArticleID = ClassUtility.aid
            };

            this.db.Replies.Add(reply);
            this.db.SaveChanges();
            //--------------------------------
            //var re = from q in db.Replies
            //         where q.ArticleID == ClassUtility.aid
            //         select new { 會員編號 = q.MemberID, 作者名稱 = q.Member.Name, 帳號 = q.Member.Email, 內容 = q.ReplyContents, 更新時間 = q.ModifiedDate, 回文 = q.ReplyID };

            //dataGridView1.DataSource = re.ToList();
            reload();
            this.textBox1.Text = null;
            MessageBox.Show("留言完成");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //var a = from q in db.Articles
            //        where q.MemberID == ClassUtility.MemberID
            //        select q;

            var sub = db.Articles
            .Where(s => s.ArticleID == ClassUtility.aid)
            .Select(s => s).FirstOrDefault();


            if (sub.MemberID == ClassUtility.MemberID)
            {
                sub.Title = "該文章已刪除";
                sub.ArticleContent = "該文章已刪除";
                //db.Articles.Remove(sub);
                db.SaveChanges();

                MessageBox.Show("刪文成功");
                this.Close();
            }
            else
            {
                MessageBox.Show("刪除失敗，不是你的文章");
                return;
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            db =new DB_GamingFormEntities();
            var sub = db.Articles
             .Where(s => s.ArticleID == ClassUtility.aid)
             .Select(s => s).FirstOrDefault();


            if (sub.MemberID == ClassUtility.MemberID)
            {
                sub.Title = textBox2.Text;
                sub.ArticleContent = textBox3.Text;
                //db.Articles.Remove(sub);
                

                MessageBox.Show("修改成功");
                Art_Reply art = new Art_Reply(textBox2.Text, textBox3.Text, ClassUtility.aid);
                db.SaveChanges();
                art.Show();
                Close();

                //string title, string articleContent, int aid
            }
            else
            {
                MessageBox.Show("修改失敗，不是你的文章");
                return;
            }
            
        }
        int reID;
        private void button4_Click(object sender, EventArgs e)
        {

            var r = (from a in db.Replies
                    where a.ReplyID == reID
                    select a).First();
            if (r.MemberID == ClassUtility.MemberID)
            {
                r.ReplyContents = this.textBox1.Text;
                db.SaveChanges();
                MessageBox.Show("修改完成");
                reload();
            }
            else 
            {
                MessageBox.Show("修改失敗，這不是你的回覆");
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            db = new DB_GamingFormEntities();
            //// 獲取選取數據
            ///
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {

                string Content = dataGridView1.Rows[e.RowIndex].Cells["內容"].Value.ToString();
                int m = (int)dataGridView1.Rows[e.RowIndex].Cells["會員編號"].Value;
                reID = (int)dataGridView1.Rows[e.RowIndex].Cells["回文"].Value;

                if (ClassUtility.MemberID != m)
                {
                    this.button4.Enabled = false;
                    this.button5.Enabled = false;
                }
                else
                {
                    this.button4.Enabled = true;
                    this.button5.Enabled = true;

                    this.textBox1.Text = Content;
                }


            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            var sub = db.Replies
           .Where(s => s.ReplyID == reID)
           .Select(s => s).FirstOrDefault();


            if (sub.MemberID == ClassUtility.MemberID)
            {
                //sub.Title = "該文章已刪除";
                //sub.ArticleContent = "該文章已刪除";
                db.Replies.Remove(sub);
                db.SaveChanges();

                MessageBox.Show("刪回復成功");
               
            }
            else
            {
                MessageBox.Show("刪除失敗，不是你的回復");
                return;
            }

            reload();

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
