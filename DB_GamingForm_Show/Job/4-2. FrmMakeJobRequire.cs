using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DB_GamingForm_Show;

namespace Groot
{
    public partial class FrmMakeJobRequire : Form
    {
        DB_GamingFormEntities db = new DB_GamingFormEntities();

        string currentID;

        public FrmMakeJobRequire()
        {
            InitializeComponent();

            Text = "廠商";
            
            LoadID();
            LoadInfo();
            
            LoadED();
            LoadRegion();
            LoadSkillClasses();

            LoadReceiveResume();
            LoadReleaseJob();

            ConfirmResumes();

            LoadTalent();
        }

        private void ConfirmResumes()
        {
            var q = from p in this.db.JobResumes.AsEnumerable()
                    where p.Job_Opportunities.FirmID == int.Parse(currentID) && p.ApplyStatusID == 5
                    select p;
            if (q.Any())
            {
                MessageBox.Show($"您有{q.Count()}份新履歷");
            }
        }

        public string MaskString(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            // 取得第一個字符
            char firstChar = input[0];

            // 使用 LINQ 將其餘字符替換為 *
            string maskedString = new string(input.Skip(1).Select(_ => '*').ToArray());

            // 重新組合第一個字符和遮蔽後的字符
            return firstChar + maskedString;
        }

        private void LoadTalent()
        {
            DataGridViewButtonColumn f = new DataGridViewButtonColumn();
            f.Name = "邀請";
            f.HeaderText = "邀請面試";
            f.DefaultCellStyle.NullValue = "邀請";


            var q= from p in this.db.Resumes.AsEnumerable()
                   where p.ResumeStatusID==1
                   select new
                   {
                       履歷編號=p.ResumeID,
                       會員編號=p.MemberID,
                       會員姓名=MaskString(p.FullName),
                       身份證字號= MaskString(p.IdentityID),
                       電話號碼= MaskString(p.PhoneNumber),
                       擁有技能=p.ResumeSkills.Select(_=>_.Skill.Name).FirstOrDefault()+"等"+ p.ResumeSkills.Select(_ => _.Skill.Name).Count() + "項技能",
                       履歷內容=p.ResumeContent,
                       工作經驗=p.WorkExp+"年",
                       學歷=p.Education.Name,
                       狀態=p.Status.Name
                   };
            this.dataGridView3.DataSource = q.ToList();
            this.dataGridView3.Columns.Add(f);
        }

        

        private void LoadID()
        {
            currentID = Gaming_Forum.ClassUtility.FirmID.ToString();
            this.textBox6.Text = currentID;
        }

        private void LoadReleaseJob()
        {
            var q = from p in this.db.Job_Opportunities.AsEnumerable()
                    where p.FirmID== int.Parse(currentID)
                    select new
                    {
                        公司編號=p.FirmID,
                        公司名稱=p.Firm.FirmName,
                        工作編號=p.JobID,
                        應徵內容=p.JobContent,
                        需求人數= p.RequiredNum+"人",
                        薪水=p.Salary,
                        狀態=p.Status.Name,
                        工作經驗=p.JobExp,
                        更新時間=p.ModifiedDate
                    };
            this.dataGridView2.DataSource = q.ToList();

            this.listBox4.Items.Add($"{"工作編號",-15}-{"應徵內容",-15}-{"狀態",-15}");
            foreach(var i in q)
            {
                this.listBox4.Items.Add($"{i.工作編號,-15}-{i.應徵內容,-15}-{i.狀態,-15}");
            }
        }

        private void LoadRegion()
        {
            var q = from p in this.db.Regions
                    select p;
            foreach(var item in q)
            {
                this.comboBox2.Items.Add(item.City);
            }
        }

        
        private void LoadInfo()
        {
            var q = from p in this.db.Firms.AsEnumerable()
                    where p.FirmID == int.Parse(currentID)  
                    select p;

            if (q.Any(n => n.FirmID == int.Parse(currentID)))
            {
                //統編
                this.textBox1.Text = q.FirstOrDefault().TaxID.ToString();
                this.textBox1.Enabled = false;
                //公司名稱
                this.textBox3.Text = q.FirstOrDefault().FirmName;
                this.textBox3.Enabled = false;
                //聯絡方式
                this.richTextBox2.Text = q.FirstOrDefault().Contact;
                this.richTextBox2.Enabled = false;
                //地址
                this.textBox7.Text = q.FirstOrDefault().FirmAddress;
                this.textBox7.Enabled = false;
                //電子信箱
                this.textBox8.Text = q.FirstOrDefault().Email;
                this.textBox8.Enabled = false;
            }
        }

        private void LoadReceiveResume()
        {   
            db = new DB_GamingFormEntities();
            var q = from p in this.db.JobResumes.AsEnumerable()
                    where p.Job_Opportunities.FirmID == int.Parse(currentID)
                    select new
                    {
                        工作機會編號 = p.JobID,
                        履歷編號 = p.ResumeID,
                        會員編號 = p.Resume.MemberID,
                        姓名 = p.Resume.FullName,
                        身份證字號 = p.Resume.IdentityID,
                        手機號碼 = p.Resume.PhoneNumber,
                        工作經驗 = p.Resume.WorkExp,
                        狀態=p.Status.Name,
                        
                    };
            this.dataGridView1.DataSource = q.ToList();
        }

        

        private void LoadED()
        {
            var q = from p in this.db.Educations
                    select p;
            foreach (var item in q)
            {
                this.comboBox1.Items.Add(item.Name);
            }
        }

        private void LoadSkillClasses()
        {
            var q = from p in db.SkillClasses
                    select p;
            foreach (var i in q)
            {
                this.listBox1.Items.Add(i.Name);
            }
        }

        private void ChangeApplyStatusID(int s)
        {
            //狀態
            var x = (from p in this.db.JobResumes.AsEnumerable()
                     where p.JobID == int.Parse(this.dataGridView1.CurrentRow.Cells[0].Value.ToString())
                     && p.ResumeID==int.Parse(this.dataGridView1.CurrentRow.Cells[1].Value.ToString())
                     select p).FirstOrDefault();

            x.ApplyStatusID = s;

            this.db.SaveChanges();
            LoadReceiveResume();

            
        }

        //============================================================================


        private void button4_Click_1(object sender, EventArgs e)
        {
            this.tabControl1.SelectedIndex += 1;
        }

        
        private void button7_Click_1(object sender, EventArgs e)
        {
            this.tabControl1.SelectedIndex += 1;
        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            this.tabControl1.SelectedIndex -= 1;
        }

       

        private void button11_Click_1(object sender, EventArgs e)
        {
            this.tabControl1.SelectedIndex -= 1;
        }

        
        ListBox llb = new ListBox();

        private void listBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (this.listBox1.SelectedIndex >= 0)
            {
                this.llb.Items.Clear();
                this.listBox2.Items.Clear();
                //===========================
                var id = from p in this.db.SkillClasses
                         select p;

                var q = from p in db.Skills.AsEnumerable()
                        where p.SkillClassID == id.ToList()[this.listBox1.SelectedIndex].SkillClassID
                        select p;

                foreach (var item in q)
                {
                    this.llb.Items.Add(item.Name);
                }
                foreach (var item in llb.Items)
                {
                    this.listBox2.Items.Add(item);
                }
            }
            else { }
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            //=========================
            var q = from p in this.db.Educations
                    select p;

            Resume f = new Resume
            {
                MemberID = int.Parse(currentID),
                FullName = this.textBox3.Text,
                IdentityID = this.textBox1.Text,
                WorkExp = this.textBox5.Text,
                FormID = 1,
                ResumeStatusID = 1,
                EDID = q.ToList()[this.comboBox1.SelectedIndex].EDID,
            };

            this.db.Resumes.Add(f);
            this.db.SaveChanges();
            //=========================
            //個人經歷


            //=========================
            //求職條件

            //=========================
            //技能專長

            //=========================
            //自傳附件

            //=========================
        }

        ListBox lb = new ListBox();



        private void listBox2_DoubleClick(object sender, EventArgs e)
        {
            //===============================
            //listbox
            this.listBox3.Items.Clear();

            var x = from p in this.db.Skills
                    where p.Name == this.listBox2.Text
                    select p;

            foreach (var g in x)
            {
                this.lb.Items.Add($"{g.SkillClass.Name}-{g.Name}");
            }

            foreach (var j in lb.Items)
            {
                this.listBox3.Items.Add(j);
            }



            List<string> skics = new List<string>();
            var a = from p in this.db.SkillClasses
                    select p;
            foreach(var g in a)
            {
                skics.Add(g.Name);
            }
            var b = from p in this.db.Skills
                    select p;

            //=================================

            this.listBox2.Items.Remove(this.listBox2.SelectedItem);



        }

        private void button1_Click(object sender, EventArgs e)
        {

            var q = (from p in this.db.Resumes.AsEnumerable()
                     where p.ResumeID == int.Parse(this.dataGridView1.CurrentRow.Cells[0].Value.ToString())
                     select p).FirstOrDefault();

            if (q == null) { return; }

            this.db.Resumes.Remove(q);
            this.db.SaveChanges();
            LoadReceiveResume();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            //=========================
            //基本資料
            var q = from p in this.db.Educations
                    select p;
            var r= from p in this.db.Regions
                   select p;

            Job_Opportunity f = new Job_Opportunity
            {
                FirmID = int.Parse(currentID),
                RegionID = r.ToList()[this.comboBox2.SelectedIndex].RegionID,
                RequiredNum = int.Parse(this.textBox2.Text),
                ModifiedDate = DateTime.Now,
                Salary = this.textBox4.Text,
                JobExp = this.textBox5.Text,
                JobContent = this.richTextBox3.Text,
                JobStatusID = 3,
                EDID = q.ToList()[this.comboBox1.SelectedIndex].EDID,
            };


            this.db.Job_Opportunities.Add(f);
            this.db.SaveChanges();

            //=========================
            //技能專長
            int lb3Length = this.listBox3.Items.Count;
            string[] lb3items = new string[lb3Length];

            for (var l = 0; l < lb3Length; l++)
            {
                lb3items[l] = this.listBox3.Items[l].ToString();
            }

            for (var o = 0; o < lb3items.Length; o++)
            {
                string[] skillskill = lb3items[o].Split('-');
                var s = this.db.Skills.AsEnumerable().Where(p => p.Name == skillskill[1]).Select(p => p.SkillID);

                int skillid = s.SingleOrDefault();
                JobSkill jobSkill = new JobSkill
                {
                    JobID = f.JobID,
                    SkillID = skillid,
                };
                this.db.JobSkills.Add(jobSkill);
            }
            this.db.SaveChanges();
            //=========================
            MessageBox.Show("新增成功");
            this.tabControl2.SelectedIndex = 2;
            LoadReceiveResume();
            LoadReleaseJob();
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            if (this.checkBox1.Checked)
            {
                this.button8.Enabled = true;
            }
            else
            {
                this.button8.Enabled = false;
            }
        }


        

        

        private void button3_Click(object sender, EventArgs e)
        {
            //待定
            ChangeApplyStatusID(7);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //拒絕
            ChangeApplyStatusID(8);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //面試邀請
            ChangeApplyStatusID(9);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            //錄取
            ChangeApplyStatusID(10);
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            //開啟
            var q = (from p in this.db.Job_Opportunities.AsEnumerable()
                    where p.JobID == int.Parse(this.dataGridView2.CurrentRow.Cells[2].Value.ToString())
                    select p).FirstOrDefault();

            if (q == null) return;

            q.JobStatusID = 3;
            q.ModifiedDate= DateTime.Now;

            this.db.SaveChanges();
            LoadReleaseJob();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            //關閉
            var q = (from p in this.db.Job_Opportunities.AsEnumerable()
                     where p.JobID == int.Parse(this.dataGridView2.CurrentRow.Cells[2].Value.ToString())
                     select p).FirstOrDefault();

            if (q == null) return;

            q.JobStatusID = 4;
            q.ModifiedDate = DateTime.Now;

            this.db.SaveChanges();
            LoadReleaseJob();


            
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var q = (from p in this.db.JobResumes.AsEnumerable()
                    where p.Job_Opportunities.JobID == int.Parse(this.dataGridView1.CurrentRow.Cells[0].Value.ToString()) 
                    && p.ApplyStatusID == 5
                    select p).FirstOrDefault();
            if (q == null) return;
            ChangeApplyStatusID(6);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            int jobdd = int.Parse(this.dataGridView2.CurrentRow.Cells[2].Value.ToString());

            //=============================================
            //JobCertificates
            var qq = from p in this.db.JobCertificates.AsEnumerable()
                     where p.JobID == jobdd
                     select p;
            foreach (var i in qq)
            {
                this.db.JobCertificates.Remove(i);
            }

            //=============================================
            //JobAdvertises
            var qqq = from p in this.db.JobAdvertises.AsEnumerable()
                      where p.JobID == jobdd
                      select p;
            foreach (var i in qqq)
            {
                this.db.JobAdvertises.Remove(i);
            }

            //=============================================
            //JobSkills
            var qqqq = from p in this.db.JobSkills.AsEnumerable()
                       where p.JobID == jobdd
                       select p;
            foreach (var i in qqqq)
            {
                this.db.JobSkills.Remove(i);
            }

            //=============================================
            //JobResumes
            var qqqqq =from p in this.db.JobResumes.AsEnumerable()
                      where p.JobID == jobdd
                      select p;
            foreach (var i in qqqqq)
            {
                this.db.JobResumes.Remove(i);
            }

            this.db.SaveChanges();
            //=============================================
            //Job_Opportunities(PK)
            var q = (from p in this.db.Job_Opportunities.AsEnumerable()
                    where p.JobID == int.Parse(this.dataGridView2.CurrentRow.Cells[2].Value.ToString())
                    select p).FirstOrDefault();

            this.db.Job_Opportunities.Remove(q);
            this.db.SaveChanges();

            LoadReleaseJob();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            var q =from p in this.db.JobResumes.AsEnumerable()
                   where p.JobID == int.Parse(this.dataGridView1.CurrentRow.Cells[0].Value.ToString()) 
                   &&p.ResumeID== int.Parse(this.dataGridView1.CurrentRow.Cells[0].Value.ToString())
                   select p;

            foreach(var r in q)
            {
                this.db.JobResumes.Remove(r);
            }
            
            this.db.SaveChanges();

            LoadReceiveResume();
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView3.Columns[e.ColumnIndex].Name == "邀請" && e.RowIndex >= 0)
            {
                if (this.listBox4.SelectedItems != null)
                {

                    
                    string selectJobID;
                    selectJobID = this.listBox4.SelectedItem.ToString().Split('-')[0].Trim();



                    var q = (from p in this.db.JobResumes.AsEnumerable()
                             where p.JobID == int.Parse(selectJobID)
                             && p.ResumeID == int.Parse(this.dataGridView3.CurrentRow.Cells[1].Value.ToString())
                             //Todo MakeJobRequire為什麼欄位0取不到值???????????????????????????????
                             select p).FirstOrDefault();

                    if (q != null)
                    {
                        MessageBox.Show("該會員已在您的應徵清單中");
                        return;
                    };

                    if (selectJobID != null)
                    {
                        JobResume jr = new JobResume
                        {
                            JobID = int.Parse(selectJobID),
                            ResumeID = int.Parse(this.dataGridView3.CurrentRow.Cells[1].Value.ToString()),
                            ApplyStatusID = 9,
                        };
                        this.db.JobResumes.Add(jr);
                    }

                    MessageBox.Show("邀請成功");
                    this.db.SaveChanges();
                    LoadReceiveResume();
                }
            }
        }
    }
}
