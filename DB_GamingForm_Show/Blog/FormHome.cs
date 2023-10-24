using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1;
using DB_GamingForm_Show;
using Gaming_Forum;
using System.Security.AccessControl;

namespace DBGaming
{
    public partial class FormHome : Form
    {
        public FormHome(int memberid)
        {
            InitializeComponent();
            LoadBlog();
            LoadSubBlog();
        }

        public class Employee
        {
            public int EmployeeId { get; set; }
            public string Name { get; set; }
            public int ManagerId { get; set; }
        }

        //DB_GamingFormEntities1 db = new DB_GamingFormEntities1();
        DB_GamingFormEntities db = new DB_GamingFormEntities();

        private void LoadSubBlog()
        {
            this.menuStrip1.Items.Clear();

            //var q = from p in this.db.SubBlogs
            //        join pp in this.db.SubBlogs
            //        on p.ParentBlogID equals pp.SubBlogID
            //        //select pp;
            //        into SubBlogs
            //        from SubBlog in SubBlogs.DefaultIfEmpty()
            //        select new
            //        {
            //            subTitle = p.Title,
            //            parentTitle = (SubBlog != null) ? SubBlog.Title : "主版"
            //        };

            var q = from p in this.db.SubBlogs
                    join pp in this.db.SubBlogs
                    on p.ParentBlogID equals pp.SubBlogID
                    //select pp;
                    into SubBlogs
                    from SubBlog in SubBlogs.DefaultIfEmpty()
                    group p by p.Title into s
                    select new
                    {
                        s.Key,
                        MyGroup = s
                    };




            foreach (var g in q)
            {
                TreeNode s = this.treeView1.Nodes.Add(g.Key.ToString());
                foreach (var items in g.MyGroup)
                {
                    s.Nodes.Add(items.ToString());
                }
            }


            this.dataGridView3.DataSource = q.ToList();



        }
        private void LoadBlog()
        {
            db = new DB_GamingFormEntities();
            this.menuBlog.Items.Clear();
            var q = db.SubTags.AsEnumerable()
                .Where(s => s.TagID == 4 && s.SubTagID != 14)
                .Select(s => s.Name);
            foreach (var s in q)
            {
                this.menuBlog.Items.Add(s);
                this.cbmBlog.Items.Add(s);
            }
            var q2 = db.Blogs
                .Where(s => s.BlogID != 17)
                .Select(s => s.Title);
            foreach (var s in q2)
            {
                this.cbmBlogselect.Items.Add(s);
            }
            
        }
        private void ALLClear()
        {
            this.menuSubblog.Items.Clear();
            this.txbNew.Text = null;
            this.dataGridView1.DataSource = null;
            this.dataGridView2.DataSource = null;
            this.subBlogImg.Image = null;
            this.txbBlog.Text = null;
            this.cbmBlog.Items.Clear();
            this.cbmBlogselect.Text = null;
            this.txbSubBlog.Text = null;
            this.txbSubBlogNew.Text = null;
            this.txbBlognew.Text = null;
            this.cbmBlogselect.Items.Clear();
        }
        private void SubAllClear()
        {
            this.menuSubblog.Items.Clear();
            this.dataGridView2.DataSource = null;
        }
        string selectblogLast="";
        private void menuBlog_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ALLClear();
            SubAllClear();
            LoadBlog();

            selectblogLast =e.ClickedItem.Text;
            this.cbmBlog.Text = e.ClickedItem.Text;
            var q = db.Blogs.AsEnumerable()
                .Where(b => b.SubTag.Name == e.ClickedItem.Text)
                .Select(s => new
                {
                    進版圖 = s.Image.Image1,
                    版名 = s.Title
                });
            txbTag.Text = e.ClickedItem.Text;
            if (q.Count() == 0)
            {
                MessageBox.Show("此分類還未有子版");

            }
            else
            {
            this.dataGridView1.DataSource = q.ToList();
            }
                
            
            
        }
        private void btnTagInsert_Click(object sender, EventArgs e)
        {
            ALLClear();


            var q = db.SubTags
                .Where(s => s.TagID == 4)
                   .Select(s => s.Name);
            if (txbTag.Text != "")
            {
                if (q.Contains(this.txbTag.Text))
                {
                    MessageBox.Show("已有此分類");
                }
                else
                {
                    SubTag sub = new SubTag()
                    {
                        TagID = 4,
                        Name = this.txbTag.Text
                    };
                    db.SubTags.Add(sub);
                    db.SaveChanges();
                    this.txbBlog.Text = null;
                    this.txbTag.Text = null;
                    LoadBlog();

                }
            }
            else
            {
                MessageBox.Show("必須輸入名稱");
            }

        }

        private void btnTagUpdate_Click(object sender, EventArgs e)
        {
            var sub = db.SubTags
                 .Where(s => s.Name == this.txbTag.Text && s.TagID == 4)
                 .Select(s => s).FirstOrDefault();

            var q = db.SubTags
                .Where(s => s.TagID == 4)
                .Select(s => s.Name);
            if (txbNew.Text != "")
            {
                if (q.Contains(this.txbTag.Text))
                {
                    if (q.Contains(this.txbNew.Text))
                    {
                        MessageBox.Show("重複名稱");
                    }
                    else
                    {

                        sub.Name = this.txbNew.Text;
                        db.SaveChanges();
                    }
                }
                else
                {
                    MessageBox.Show("沒有此分類");
                }
            }
            else
            {
                MessageBox.Show("必須輸入名稱");
            }


            this.txbBlog.Text = null;
            this.txbNew.Text = null;
            this.cbmBlog.Text = null;
            this.txbTag.Text = null;
            ALLClear();
            LoadBlog();
        }

        private void btnTagDelete_Click(object sender, EventArgs e)
        {
            ALLClear();
            var delRe = db.Replies.AsEnumerable()
                .Where(r => r.Article.SubBlog.Blog.SubTag.Name == this.txbTag.Text && r.Article.SubBlog.Blog.SubTag.TagID == 4)
                .Select(r => r);
            if (delRe == null) return;
            foreach (var item in delRe)
            {
                db.Replies.Remove(item);
            };
            var delArt = db.Articles.AsEnumerable()
                .Where(a => a.SubBlog.Blog.SubTag.Name == this.txbTag.Text && a.SubBlog.Blog.SubTag.TagID == 4)
                .Select(a => a);
            if (delArt == null) return;
            foreach (var item in delArt)
            {
                db.Articles.Remove(item);
            }
            var delSubBlog = db.SubBlogs.AsEnumerable()
                .Where(s => s.Blog.SubTag.Name == this.txbTag.Text && s.Blog.SubTag.TagID == 4)
                .Select(s => s);
            if (delSubBlog == null) return;
            foreach (var item in delSubBlog)
            {
                db.SubBlogs.Remove(item);
            }
            var delBlog = db.Blogs.AsEnumerable()
                .Where(b => b.SubTag.Name == this.txbTag.Text && b.SubTag.TagID == 4)
                .Select(b => b);
            if (delBlog == null) return;
            foreach (var item in delBlog)
            {
                db.Blogs.Remove(item);
            }
            var sub = db.SubTags
                .Where(s => s.Name == this.txbTag.Text && s.TagID == 4)
                .Select(s => s).FirstOrDefault();
            if (sub == null) return;
            db.SubTags.Remove(sub);
            db.SaveChanges();
            ALLClear();
            LoadBlog();
            this.cbmBlog.Text = null;
            this.txbTag.Text = null;
        }
        string selectblog;
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            this.menuSubblog.Items.Clear();
            var q = db.SubBlogs.AsEnumerable()
                   .Where(s => s.Blog.Title == this.dataGridView1.CurrentRow.Cells["版名"].Value.ToString())
                   .Select(s => s.Title);
            if (q.Count() == 0)
            
                MessageBox.Show("沒有分類");
            
            
            

            foreach (var s in q)
            {
                this.menuSubblog.Items.Add(s);
            }

            
            this.dataGridView2.DataSource = null;
            this.txbBlog.Text = this.dataGridView1.CurrentRow.Cells["版名"].Value.ToString();
            this.cbmBlogselect.Text = this.dataGridView1.CurrentRow.Cells["版名"].Value.ToString();
            var q2 = db.Blogs.AsEnumerable()
                  .Where(s => s.Title == this.dataGridView1.CurrentRow.Cells["版名"].Value.ToString())
                  .Select(s => s.Image.Image1);
            byte[] bytes = q2.FirstOrDefault();
            System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
            this.subBlogImg.Image = System.Drawing.Image.FromStream(ms);
            selectblog = this.dataGridView1.CurrentRow.Cells["版名"].Value.ToString();

        }
        private void InsertImg()
        {

            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.subBlogImg.Image = System.Drawing.Image.FromFile(this.openFileDialog1.FileName);
            }
            else
            {
                this.subBlogImg.Image = null;
            }
        }
        private void btnImage_Click(object sender, EventArgs e)
        {
            InsertImg();
        }
        private void btnBlogInsert_Click(object sender, EventArgs e)
        {
            var q = db.Blogs.AsEnumerable()
                .Where(b => b.SubTag.Name == this.cbmBlog.Text)
                .Select(b => b.BlogID).FirstOrDefault();
            var q2 = db.SubTags.AsEnumerable()
                .Where(s => s.Name == this.cbmBlog.Text)
                .Select(s => s.SubTagID).FirstOrDefault();
            var q3 = db.Blogs.AsEnumerable()
                    .Where(b => b.SubTag.Name == this.cbmBlog.Text)
                    .Select(b => b.Title);

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            if (txbBlog.Text != "")
            {
                if (q3.Contains(this.txbBlog.Text))
                {
                    MessageBox.Show("已有此討論版");
                }
                else
                {
                    if (this.subBlogImg.Image == null)
                    {
                        Blog blog = new Blog()
                        {
                            ImageID = 2,
                            BlogID = q,
                            Title = this.txbBlog.Text,
                            SubTagID = q2
                        };
                        db.Blogs.Add(blog);
                        //db.SaveChanges();
                    }
                    else
                    {
                        this.subBlogImg.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        byte[] bytes = ms.GetBuffer();
                        DB_GamingForm_Show.Image img = new DB_GamingForm_Show.Image()
                        {
                            Name = "img",
                            Image1 = bytes
                        };
                        db.Images.Add(img);
                        db.SaveChanges();
                        Blog blog = new Blog()
                        {
                            ImageID = img.ImageID,
                            BlogID = q,
                            Title = this.txbBlog.Text,
                            SubTagID = q2
                        };
                        db.Blogs.Add(blog);

                    }
                    db.SaveChanges();
                    dataGridView1.DataSource = null;
                }
            }
            else
            {
                MessageBox.Show("必須輸入名稱");
            }

            ALLClear();
            LoadBlog();
        }

        private void BtnBlogUpdate_Click(object sender, EventArgs e)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            var updateBlog = db.Blogs.AsEnumerable()
                .Where(b => b.Title == this.txbBlog.Text && b.SubTag.Name == this.cbmBlog.Text)
                .Select(b => b).FirstOrDefault();

            var compareBlog = db.Blogs.AsEnumerable()
                .Where(b => b.SubTag.Name == this.cbmBlog.Text)
                .Select(b => b.Title);
            if (txbBlognew.Text != "")
            {

                if (compareBlog.Contains(this.txbBlog.Text))
                {
                    if (!compareBlog.Contains(this.txbBlognew.Text))
                    {

                        if (this.subBlogImg.Image == null)
                        {
                            updateBlog.Title = this.txbBlognew.Text;
                            updateBlog.ImageID = 2;
                            MessageBox.Show("沒上傳圖片，設為預設圖");
                        }
                        else
                        {
                            this.subBlogImg.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                            byte[] bytes = ms.GetBuffer();

                            updateBlog.Title = this.txbBlognew.Text;
                            updateBlog.Image.Image1 = bytes;
                        }
                        db.SaveChanges();
                    }
                    else
                    {
                        MessageBox.Show("重複名稱");
                    }
                }
                else
                {
                    MessageBox.Show("沒有此討論版");
                }
            }
            else
            {
                MessageBox.Show("必須輸入名稱");
            }
            dataGridView1.DataSource = null;
            ALLClear();
            LoadBlog();
        }

        private void btnBlogDelete_Click(object sender, EventArgs e)
        {
            var delRe = db.Replies
                .Where(r => r.Article.SubBlog.Blog.Title == this.txbBlog.Text && r.Article.SubBlog.Blog.SubTag.Name == this.cbmBlog.Text)
                .Select(r => r);
            if (delRe == null) return;
            foreach (var item in delRe)
            {
                db.Replies.Remove(item);
            };
            var delArt = db.Articles
                .Where(a => a.SubBlog.Blog.Title == this.txbBlog.Text && a.SubBlog.Blog.SubTag.Name == this.cbmBlog.Text)
                .Select(a => a);
            if (delArt == null) return;
            foreach (var item in delArt)
            {
                db.Articles.Remove(item);
            }
            var delSubBlog = db.SubBlogs
                .Where(s => s.Blog.Title == this.txbBlog.Text && s.Blog.SubTag.Name == this.cbmBlog.Text)
                .Select(s => s);
            if (delSubBlog == null) return;
            foreach (var item in delSubBlog)
            {
                db.SubBlogs.Remove(item);
            }
            var delBlog = db.Blogs.AsEnumerable()
                .Where(b => b.Title == this.txbBlog.Text)
                .Select(b => b).FirstOrDefault();
            if (delBlog == null) return;
            db.Blogs.Remove(delBlog);
            db.SaveChanges();
            dataGridView1.DataSource = null;
            ALLClear();
            LoadBlog();
        }

      
        private void menuSubblog_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            var q = db.Articles.AsEnumerable()
                .Where(a => a.SubBlog.Title ==e.ClickedItem.Text&&a.SubBlog.Blog.Title==selectblog && a.SubBlog.Blog.SubTag.Name == selectblogLast)
                .Select(s => new
                {
                    文章編號 = s.ArticleID,
                    標題 = s.Title,
                    內文預覽 = s.ArticleContent
                });
            if (q.Count() == 0)
            {
                MessageBox.Show("還未有文章");
            }
            else
            {                
            this.dataGridView2.DataSource = q.ToList();
            }
            this.txbSubBlog.Text = e.ClickedItem.Text;
        }

        
        private void btnSubBlogInsert_Click(object sender, EventArgs e)
        {
            var q = db.Blogs.AsEnumerable()
                  .Where(s => s.Title == this.cbmBlogselect.Text)
                  .Select(s => s.BlogID).FirstOrDefault();

            var q2 = db.SubBlogs.AsEnumerable()
                .Where(s => s.Blog.Title == this.cbmBlogselect.Text)
                .Select(s => s.Title);
            if (txbSubBlog.Text != "")
            {
                if (q2.Contains(this.txbSubBlog.Text))
                {
                    MessageBox.Show("已有此版子分類");
                }
                else
                {

                    SubBlog subBlog = new SubBlog()
                    {
                        BlogID = q,
                        Title = this.txbSubBlog.Text,
                    };
                    db.SubBlogs.Add(subBlog);
                    db.SaveChanges();
                }
            }
            else
            {
                MessageBox.Show("必須輸入名稱");
            }

            LoadBlog();
            SubAllClear();


        }

        private void btnSubUpdate_Click(object sender, EventArgs e)
        {
            this.menuSubblog.Items.Clear();
            var updateSubBlog = db.SubBlogs.AsEnumerable()
                .Where(s => s.Title == this.txbSubBlog.Text)
                .Select(s => s).FirstOrDefault();

            var compare = db.SubBlogs.AsEnumerable()
                .Where(s => s.Blog.Title == this.cbmBlogselect.Text)
                .Select(s => s.Title);
            if (txbSubBlogNew.Text != "")
            {
                if (compare.Contains(this.txbSubBlog.Text))
                {
                    if (compare.Contains(this.txbSubBlogNew.Text))
                    {
                        MessageBox.Show("重複名稱");
                    }
                    else
                    {

                        updateSubBlog.Title = this.txbSubBlogNew.Text;
                    }
                    this.db.SaveChanges();
                }
                else
                {
                    MessageBox.Show("沒有此版的子分類");
                }
            }
            else
            {
                MessageBox.Show("必須輸入名稱");
            }

            SubAllClear();
            LoadBlog();
        }

        private void btnSubDelete_Click(object sender, EventArgs e)
        {

            var delRe = db.Replies.AsEnumerable()
                .Where(x => x.Article.SubBlog.Title == this.txbSubBlog.Text && x.Article.SubBlog.Blog.Title == this.cbmBlogselect.Text)
                .Select(x => x);
            if (delRe == null) return;
            foreach (var x in delRe)
            {
                db.Replies.Remove(x);
            }
            db.SaveChanges();

            var delArt = db.Articles.AsEnumerable()
                .Where(b => b.SubBlog.Title == this.txbSubBlog.Text && b.SubBlog.Blog.Title == this.cbmBlogselect.Text)
                .Select(b => b);
            if (delArt == null) return;
            foreach (var x in delArt)
            {
                db.Articles.Remove(x);
            }
            db.SaveChanges();

            var delSubBlog = db.SubBlogs.AsEnumerable()
                .Where(s => s.Title == this.txbSubBlog.Text && s.Blog.Title == this.cbmBlogselect.Text)
                .Select(s => s).FirstOrDefault();
            if (delSubBlog == null) return;
            db.SubBlogs.Remove(delSubBlog);
            db.SaveChanges();
            this.dataGridView2.DataSource = null;
            MessageBox.Show("Successful");
            SubAllClear();
            LoadBlog();
        }

        private void btnArticleInsert_Click(object sender, EventArgs e)
        {
            NewART aRT = new NewART();
            aRT.ShowDialog();
            SubAllClear();
            LoadBlog();
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // 獲取選取數據
            string selectedTitle = dataGridView2.Rows[e.RowIndex].Cells["標題"].Value.ToString();
            string selectedContent = dataGridView2.Rows[e.RowIndex].Cells["內文預覽"].Value.ToString();
            int articleID = (int)dataGridView2.Rows[e.RowIndex].Cells["文章編號"].Value;
            //int memberID = (int)dataGridView3.Rows[e.RowIndex].Cells["會員編號"].Value;

            //int memberID = Class1.memberid2;

            ClassUtility.aid = articleID;

            // 數據傳遞到下一個視窗
            Art_Reply artReplyForm = new Art_Reply(selectedTitle, selectedContent, articleID);
            artReplyForm.ShowDialog();

        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
