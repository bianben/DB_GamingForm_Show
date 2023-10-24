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

namespace WindowsFormsApp1
{
    public partial class NewART : Form
    {
        public NewART(/*int memberid*/)
        {
            InitializeComponent();
            LoadCombobox1();
           
            comboBox1.SelectedIndex = 0;
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            
        }

        private void LoadCombobox2()
        {
           
              comboBox2.Items.Clear();
            comboBox2.Text = string.Empty;
            var ID = from A in this.db.SubBlogs
                     where  A.Blog.Title == comboBox1.SelectedItem.ToString()
                     select new { A.Title, A.SubBlogID };

            foreach (var A in ID)
            {
                this.comboBox2.Items.Add(A.Title);
            }
            
        }

        private void LoadCombobox1()
        {
            var ID = from g in this.db.Blogs
                     where g.BlogID != 0 && g.BlogID!=17
                     select new { g.Title,g.BlogID };
            foreach (var g in ID)
            {
                this.comboBox1.Items.Add(g.Title);
            }
        }

        

        DB_GamingFormEntities db = new DB_GamingFormEntities();
        private void button2_Click(object sender, EventArgs e)
        {
            var q = this.db.SubBlogs.AsEnumerable().Where(p => p.Title == this.comboBox2.Text&&p.Blog.Title==this.comboBox1.Text)
                .Select(p => p.SubBlogID);
            if (comboBox2.SelectedItem == null)
            {
                MessageBox.Show("沒選分類");
                return;
            }

            if (textBox1.Text == "")
            {
                MessageBox.Show("您尚未輸入標題");
                return;
            }

            if (textBox2.Text == "")
            {
                MessageBox.Show("您尚未輸入內容");
                return;
            }
    

            Article article = new Article
            {
                
                SubBlogID =q.First(),              
                Title = textBox1.Text,
                ArticleContent = textBox2.Text,
                ModifiedDate = DateTime.Now,
                MemberID = ClassUtility.MemberID,                                
            };
            
            this.db.Articles.Add(article);
            this.db.SaveChanges();

            ClassUtility.aid = article.ArticleID;
            MessageBox.Show(ClassUtility.aid.ToString());
            //----------------------------
            MessageBox.Show("發文成功");

            int memberid = ClassUtility.MemberID;

            Art_Reply artReplyForm = new Art_Reply(textBox1.Text, textBox2.Text,ClassUtility.aid);
            artReplyForm.Show();
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadCombobox2();
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            //LoadCombobox2();
        }


    }
}
