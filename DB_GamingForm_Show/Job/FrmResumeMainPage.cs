using DB_GamingForm_Show.Job;
using Groot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DB_GamingForm_Show
{
    public partial class FrmResumeMainPage : Form
    {
        DB_GamingFormEntities entities = new DB_GamingFormEntities();

        public FrmResumeMainPage()
        {

            InitializeComponent();
            this.dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            this.button1.Enabled = false;
            ComboLoad();
            LoadData();
            HotSearch();
        }
        #region OfficalCode
        public bool flag = true;
        public int count = 1;
        public int sourcecount = 0;
        public int page = 0;
        public int pagecount = 25;
        List<ResumeResult> list = new List<ResumeResult>();
        public class ResumeResult
        {
            public int ResumeID { get; set; }
            public int MemberID { get; set; }
            public int Age { get; set; }
            public string PhoneNumber { get; set; }
            public string ResumeContent { get; set; }
            public string WorkExp { get; set; }
            public string Educations { get; set; }

        }
        private void HotSearch()
        {
            try
            {
                var value = (from n in this.entities.SerachRecords.AsEnumerable()
                             where n.IsMember == false
                             group n by n.Name into q
                             orderby q.Count() descending
                             select q.Key).ToList();
                this.linkLabel1.Text = value.ToList()[0].ToString();
                this.linkLabel2.Text = value.ToList()[1].ToString();
                this.linkLabel3.Text = value.ToList()[2].ToString();
            }
            catch (Exception)
            {

                this.linkLabel1.Visible = false;
                this.linkLabel2.Visible = false;
                this.linkLabel3.Visible = false;


            }

        }

        private void ComboLoad()
        {

            this.comboBox1.Items.Add("請選擇...");
            this.comboBox2.Items.Add("請選擇...");
            this.comboBox3.Items.Add("請選擇...");
            this.comboBox1.SelectedIndex = 0;
            this.comboBox2.SelectedIndex = 0;
            this.comboBox3.SelectedIndex = 0;
            this.comboBox5.SelectedIndex = 0;
            this.comboBox6.SelectedIndex = 0;
            var q = from n in this.entities.Educations
                    select n.Name;
            foreach (var educations in q)
            {
                this.comboBox1.Items.Add(educations);
            }
            var q2 = from n in this.entities.Certificates
                     select n.Name;
            foreach (var cert in q2)
            {
                this.comboBox2.Items.Add(cert);
            }
            var q3 = from n in this.entities.SkillClasses
                     select n.Name;
            foreach (var skillClass in q3)
            {
                this.comboBox3.Items.Add(skillClass);
            }




        }

        private void LoadData()
        {
            this.button1.Enabled = true;
            this.bindingSource1.Clear();
            var data = from n in this.entities.Resumes.AsEnumerable()
                       select new
                       {
                           n.ResumeID,
                           n.MemberID,
                           Age = (DateTime.Now.Year - n.Member.Birth.Year),
                           n.PhoneNumber,
                           n.ResumeContent,
                           n.WorkExp,
                           Education = n.Education.Name
                       };
            this.bindingSource1.DataSource = data.ToList();
            this.dataGridView1.DataSource = this.bindingSource1;
            sourcecount = this.dataGridView1.RowCount;
            ListLoad(sourcecount);

        }



        private void Clear()
        {
            this.comboBox1.SelectedIndex = 0;
            this.comboBox2.SelectedIndex = 0;
            this.comboBox3.SelectedIndex = 0;
            this.comboBox5.SelectedIndex = 0;
            this.comboBox6.SelectedIndex = 0;

        }




        

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text == "")
            {
                LoadData();
            }
            Search(this.textBox1.Text);
        }

        private void Remove()
        {
            //var value = (from p in this.entities.SerachRecords
            //               where p.CreateDays.Day <=DateTime.Now.Day-7
            //               select p).FirstOrDefault();

            //if (value == null) return;

            this.entities.SerachRecords.Remove
                                (new SerachRecord { ID = 2 });
            this.entities.SaveChanges();

        }
        private void Search(string source)
        {
            if (source == "")
            {
                Clear();
                LoadData();
            }
            else
            {
                this.entities.SerachRecords.Add
                                (new SerachRecord { Name = source, CreateDays = (DateTime.Now.Date), IsMember = false });
                this.entities.SaveChanges();
                var data = from n in this.entities.Resumes.AsEnumerable()
                           where n.ResumeContent.ToLower().Contains(source.ToLower())
                           select new
                           {
                               n.ResumeID,
                               n.MemberID,
                               Age = (DateTime.Now.Year - n.Member.Birth.Year),
                               n.PhoneNumber,
                               n.ResumeContent,
                               n.WorkExp,
                               Education = n.Education.Name
                           };
                this.bindingSource1.Clear();
                this.bindingSource1.DataSource = data.ToList();
                this.dataGridView1.DataSource = this.bindingSource1;
                this.label12.Text = $"{page * pagecount}/{this.dataGridView1.RowCount}筆";
                sourcecount = data.Count();
                ListLoad(sourcecount);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Search(this.linkLabel1.Text);
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Search(this.linkLabel2.Text);
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Search(this.linkLabel3.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Filter();

        }




        private void button4_Click_1(object sender, EventArgs e)
        {
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBox1.SelectedIndex == 0)
            {
                Clear();
                LoadData();

            }
            else
            {
                var value = from n in list.AsEnumerable()
                            where n.Educations == this.comboBox1.Text
                            select n;
                this.bindingSource1.Clear();
                this.bindingSource1.DataSource = value.ToList();
                this.dataGridView1.DataSource = this.bindingSource1;
                sourcecount = value.Count();
                ListLoad(sourcecount);
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBox2.SelectedIndex == 0)
            {
                Clear();
                LoadData();

            }
            else
            {
                var value = from n in list.AsEnumerable()
                            where n.ResumeContent.Contains(this.comboBox2.Text)
                            select n;
                this.bindingSource1.Clear();
                this.bindingSource1.DataSource = value.ToList();
                this.dataGridView1.DataSource = this.bindingSource1;
                sourcecount = value.Count();
                ListLoad(sourcecount);
            }

        }
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            var q4 = from n in this.entities.Skills.AsEnumerable()
                     where n.SkillClass.Name == this.comboBox3.Text
                     select n.Name;
            this.comboBox4.DataSource = q4.ToList();
            if (this.comboBox3.SelectedIndex == 0)
            {
                this.comboBox4.Text = "";
                Clear();
                LoadData();
            }

        }



        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            var value = from n in list.AsEnumerable()
                        where n.ResumeContent.Contains(this.comboBox4.Text)
                        select n;
            this.bindingSource1.Clear();
            this.bindingSource1.DataSource = value.ToList();
            this.dataGridView1.DataSource = this.bindingSource1;
            sourcecount = value.Count();
            ListLoad(sourcecount);
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (this.comboBox5.SelectedIndex == 0)
            {
                Clear();
                LoadData();
            }
            else
            {
                var value = from n in list.AsEnumerable()
                            where int.Parse(n.WorkExp) >= int.Parse(this.comboBox5.Text)
                            select n;

                this.bindingSource1.Clear();
                this.bindingSource1.DataSource = value.ToList();
                this.dataGridView1.DataSource = this.bindingSource1;
                sourcecount = value.Count();
                ListLoad(sourcecount);
            }
            //this.label12.Text = $"10/{this.dataGridView1.RowCount}筆";
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBox6.SelectedIndex == 0)
            {
                Clear();
                LoadData();
            }
            else
            {
                var value2 = this.list.AsEnumerable()
                            .Where(n => (n.Age <= int.Parse(this.comboBox6.Text)))
                            .Select(n => n
                            );

                //this.label12.Text = $"10/{this.dataGridView1.RowCount}筆";

            }
        }



        private void button4_Click_2(object sender, EventArgs e)
        {
            Clear();
            LoadData();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Remove();
        }
        #endregion


        private void ListLoad(int sourcecount)
        {
            count += 1;
            list.Clear();
            page = 0;
            for (int i = 0; i < sourcecount; i++)
            {
                list.Add(new ResumeResult
                {
                    ResumeID = (int)this.dataGridView1.Rows[i].Cells[0].Value,
                    MemberID = (int)this.dataGridView1.Rows[i].Cells[1].Value,
                    Age = (int)this.dataGridView1.Rows[i].Cells[2].Value,
                    PhoneNumber = (string)this.dataGridView1.Rows[i].Cells[3].Value,
                    ResumeContent = (string)this.dataGridView1.Rows[i].Cells[4].Value,
                    WorkExp = (string)this.dataGridView1.Rows[i].Cells[5].Value,
                    Educations = (string)this.dataGridView1.Rows[i].Cells[6].Value,

                });
            }
            this.bindingSource1.Clear();
            this.bindingSource1.DataSource = list.ToList();
            this.dataGridView1.DataSource = this.bindingSource1;
            if (list.Count == 0 && count != 1)
            {
                MessageBox.Show("No Match");
                Clear();
                LoadData();

            }
            else if (list.Count <= pagecount)
            {
                this.label12.Text = $"{list.Count} /{list.Count}筆";
                this.button1.Enabled = false;
                this.button8.Enabled = false;
            }
            else
            {
                this.label12.Text = $"{list.Count} /{list.Count}筆";
                this.button1.Enabled = false;
                this.button8.Enabled = true;
            }
            

        }


        private void button3_Click(object sender, EventArgs e)
        {
            Blog();
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FrmMakeJobRequire jb = new FrmMakeJobRequire();
            jb.Show();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            page -= 1;
            if (page < 0)
            {
                page = 0;
                this.bindingSource1.Clear();
                this.bindingSource1.DataSource = list.ToList().Skip(page  * pagecount).Take(pagecount);
                this.dataGridView1.DataSource = this.bindingSource1;
                this.label12.Text = $"已是第一頁";
                this.button1.Enabled = false;
                


            }
            else
            {
                this.bindingSource1.Clear();
                this.bindingSource1.DataSource = list.ToList().Skip(page * pagecount).Take(pagecount);
                this.dataGridView1.DataSource = this.bindingSource1;
                this.button8.Enabled = true;
                this.label12.Text = $"{page * pagecount} /{list.Count}筆";

            }
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.bindingSource1.Position -= 1;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.bindingSource1.Position += 1;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            page += 1;
            if (page * pagecount >= list.Count)
            {
                page -= 1;
                this.bindingSource1.Clear();
                this.bindingSource1.DataSource = list.ToList().Skip(page * pagecount).Take(pagecount);
                this.dataGridView1.DataSource = this.bindingSource1;
                this.label12.Text = $"已是最後一頁";
                this.button8.Enabled = false;

            }
            else
            {
                this.button1.Enabled = true;
                this.bindingSource1.Clear();
                this.bindingSource1.DataSource = list.ToList().Skip((page-1) * pagecount).Take(pagecount);
                this.dataGridView1.DataSource = this.bindingSource1;
                this.label12.Text = $"{page * pagecount} /{list.Count}筆";

            }
        }

        #region 再研究
        private void Filter()
        {
            string f1 = this.comboBox1.Text;
            string f2 = this.comboBox2.Text;
            string f3 = this.comboBox3.Text;
            string f4 = this.comboBox4.Text;
            string f5 = this.comboBox5.Text;
            string f6 = this.comboBox6.Text;
            var value2 = this.entities.Job_Opportunities.AsEnumerable()
                .Where(n => n.Education.Name == f1 &&
                        n.JobCertificates.All(c => c.Certificate.Name == f2) &&
                        n.JobSkills.All(s => s.Skill.Name == f3 && s.Skill.SkillClass.Name == f4) &&
                        n.Region.City == f5 &&
                        int.Parse(n.Salary) >= int.Parse(f6)
                        ).Select(n => n);
            this.dataGridView1.DataSource = value2.ToList();



        }


        #endregion

        #region Pratice
        private void Blog()
        {

            //var q = from n in this.entities.Blogs
            //        where n.BlogID ==25
            //        select n.BlogID;
            var q = (from p in this.entities.Blogs
                     where p.BlogID == 25
                     select p).FirstOrDefault();

            this.entities.Blogs.Remove(q);
            //this.entities.Blogs.Remove
            //                    (new Blog { BlogID = 25 });
            this.entities.SaveChanges();


        }
    }
    #endregion
}


