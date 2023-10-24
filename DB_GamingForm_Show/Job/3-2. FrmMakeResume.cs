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
using Gaming_Forum;
using Image = DB_GamingForm_Show.Image;

namespace Groot
{
    public partial class FrmMakeResume : Form
    {
        DB_GamingFormEntities db = new DB_GamingFormEntities();

        string currentID;

        ListBox llb = new ListBox();

        ListBox lb = new ListBox();

        private bool shouldSetCurrentCell = true;

        public FrmMakeResume()
        {
            InitializeComponent();

            Text = "會員";

            LoadID();
            LoadEmail();
            LoadSkills();
            LoadED();
            LoadArticle();

            LoadCreatePage();
            
            LoadMyResume();
            LoadMySendResumes();
            LoadJobOffers();

            confirmInvite();

        }

        private void confirmInvite()
        {
            var q = from p in this.db.JobResumes.AsEnumerable()
                    where p.Resume.MemberID== int.Parse(currentID) && p.ApplyStatusID == 9
                    select p;
            if (q.Any())
            {
                MessageBox.Show($"您有{q.Count()}份面試邀請，祝您面試順利");
            }
        }

        private void LoadID()
        {
            currentID = ClassUtility.MemberID.ToString();
            this.textBox6.Text = currentID;
        }

        private void LoadJobOffers()
        {
            DataGridViewButtonColumn f= new DataGridViewButtonColumn();
            f.Name = "我要應徵";
            f.HeaderText = "應徵";
            f.DefaultCellStyle.NullValue = "我要應徵";


            var q = from p in this.db.Job_Opportunities.AsEnumerable()
                    select new
                    {
                        公司名稱 = p.Firm.FirmName,
                        工作內容 = p.JobContent,
                        薪資 = p.Salary,
                        工作縣市 = p.Region.City,
                        具備技能 = p.JobSkills.Select(pp => pp.Skill.Name).FirstOrDefault() + "等" + p.JobSkills.Count()+"項",
                        學歷要求 = p.Education.Name,
                        工作經驗 = p.JobExp + "年",
                        需求人數 = p.RequiredNum + "人",
                        開放應徵 = p.Status.Name,
                        更新日期 = p.ModifiedDate,
                    };


            this.dataGridView4.DataSource = q.ToList();
            this.dataGridView4.Columns.Add(f);
            
        }


        private void LoadMySendResumes()
        {
            //Todo 純粹沒renew entities
            db = new DB_GamingFormEntities();
            var q = from p in this.db.JobResumes.AsEnumerable()
                    where p.Resume.MemberID == int.Parse(currentID)
                    select new
                    {
                        履歷編號 = p.ResumeID,
                        會員編號 = p.Resume.MemberID,
                        工作編號 = p.JobID,
                        公司名稱 = p.Job_Opportunities.Firm.FirmName,
                        狀態 = p.Status.Name,
                        大頭照 = p.Resume.Image.Image1,
                        更新時間=p.Job_Opportunities.ModifiedDate
                    };

            this.bindingSource1.Clear();
            this.pictureBox2.DataBindings.Clear();
            this.bindingSource1.DataSource = q.ToList();
            this.dataGridView1.DataSource = this.bindingSource1;
            this.pictureBox2.DataBindings.Add("Image", bindingSource1, "大頭照", true);

        }

        private void LoadEmail()
        {
            var q = from p in this.db.Members.AsEnumerable()
                    where p.MemberID == int.Parse(currentID)
                    select new { email = p.Email };

            this.textBox8.Text = q.FirstOrDefault().email.ToString();
        }
        private void LoadCreatePage()
        {
            var q = from p in this.db.Resumes.AsEnumerable()
                    where p.MemberID == int.Parse(currentID)
                    select p;
            if (q.Any(n => n.MemberID == int.Parse(currentID)))
            {
                this.textBox3.Text = q.FirstOrDefault().FullName;
                
                this.textBox1.Text = q.FirstOrDefault().IdentityID;
                
                this.textBox2.Text = q.FirstOrDefault().PhoneNumber;

                //todo (已解決)combox 用 index設定不到值 loaditems的程式碼要在前
                this.comboBox1.SelectedIndex = (int)(q.FirstOrDefault().EDID - 1);
                //this.comboBox1.Text = q.FirstOrDefault().Education.Name;


                this.textBox5.Text = q.FirstOrDefault().WorkExp;
            }
        }

        

        private void LoadMyResumeDetials()
        {
            var qq = (from p in this.db.Resumes.AsEnumerable()
                      where p.ResumeID == int.Parse(this.dataGridView2.CurrentRow.Cells[0].Value.ToString())
                      select new
                      {
                          履歷編號 = p.ResumeID,
                          會員編號 = p.MemberID,
                          狀態 = p.ResumeStatusID,
                          身份證字號 = p.IdentityID,
                          手機號碼 = p.PhoneNumber,
                          教育程度編號 = p.EDID,
                          工作經驗 = p.WorkExp,
                          自我介紹 = p.ResumeContent,
                          電子信箱 = p.Member.Email,
                          大頭照 = p.Image.Image1
                      }).FirstOrDefault();

            //=====================================

            var q = from p in this.db.Educations
                    select p.Name;
            this.comboBox2.Items.Clear();
            foreach(var g in q)
            {
                this.comboBox2.Items.Add(g.ToString());
            }
            //=====================================

            this.textBox26.Text = qq.履歷編號.ToString();
            this.textBox25.Text = qq.會員編號.ToString();
            if (qq.狀態 == 1)
            {
                checkBox4.Checked = true;
                checkBox5.Checked = false;
            }
            else
            {
                checkBox4.Checked = false;
                checkBox5.Checked = true;
            }
            this.textBox21.Text = qq.身份證字號;
            this.textBox20.Text = qq.手機號碼;
            this.comboBox2.SelectedIndex = (int)qq.教育程度編號-1;
            this.textBox15.Text = qq.工作經驗;
            this.textBox7.Text = qq.電子信箱;
            this.richTextBox3.Text = qq.自我介紹;
            System.IO.MemoryStream ms = new System.IO.MemoryStream(qq.大頭照);
            this.pictureBox3.Image = System.Drawing.Image.FromStream(ms);
        }

        private void LoadMyResume()
        {
            try
            {
                var s = from p in this.db.Resumes.AsEnumerable()
                        where p.MemberID == int.Parse(currentID)
                        select p;
                //判斷有無履歷，沒有則提示未建立履歷
                if (s.Any())
                {   
                    //Todo 新增履歷第一筆失敗 #加入下面一行後已修正
                    db = new DB_GamingFormEntities();
                    //==================================================
                    //datagridview
                    var q = from p in this.db.Resumes.AsEnumerable()
                            where p.MemberID == int.Parse(currentID)
                            select new
                            {
                                履歷編號 = p.ResumeID,
                                會員編號 = p.MemberID,
                                狀態 = p.Status.Name,
                                //狀態=p.ResumeStatusID,
                                姓名 = p.FullName,
                                身份證字號 = p.IdentityID,
                                手機號碼 = p.PhoneNumber,
                                工作經驗 = p.WorkExp + "年",
                                技能 = p.ResumeSkills.Select(sk => sk.Skill.Name).FirstOrDefault() + "等" + p.ResumeSkills.Count + "項",
                            };
                    if (q.ToList() == null) { return; }

                    this.dataGridView2.DataSource = q.ToList();
                    this.dataGridView3.DataSource = q.ToList();
                    //================================================
                    //詳細資訊

                    //僅在視窗初始化時，預設選取第一個儲存格，以將值傳給qq
                    if (shouldSetCurrentCell)
                    {
                        this.dataGridView2.CurrentCell = dataGridView2.Rows[0].Cells[0];
                        shouldSetCurrentCell = false;
                    }

                    LoadMyResumeDetials();

                }
                else
                {   
                    //Todo 刪除個人履歷沒有刷新 加入下兩行後已排除
                    MessageBox.Show("尚無履歷資料，將跳至建立履歷頁面");
                    this.dataGridView2.DataSource = null;
                    this.dataGridView3.DataSource = null;
                    this.tabControl2.SelectedIndex = 1;
                }
            }
            catch(Exception ex){ MessageBox.Show(ex + ""); }
        }

        private void LoadArticle()
        {
            var q = from p in db.Articles.AsEnumerable()
                    where p.MemberID == int.Parse(currentID)
                    select p;
            foreach (var item in q)
            {
                this.checkedListBox1.Items.Add(item.Title);
            }
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

        private void LoadSkills()
        {
            var q = from p in db.SkillClasses
                    select p;
            foreach (var i in q)
            {
                this.listBox1.Items.Add(i.Name);
            }
        }
        private void enabledFalse()
        {
            this.checkBox4.Enabled = false;
            this.checkBox5.Enabled = false;
            this.textBox20.Enabled = false;
            this.comboBox2.Enabled = false;
            this.textBox15.Enabled = false;
            this.richTextBox3.Enabled = false;
            this.button19.Enabled = false;
            this.button16.Enabled = false;
            this.button17.Enabled = false;
        }



        private void button4_Click(object sender, EventArgs e)
        {
            this.tabControl1.SelectedIndex += 1;
        }

        private void button5_Click_1(object sender, EventArgs e)
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

        private void button12_Click_1(object sender, EventArgs e)
        {
            this.tabControl1.SelectedIndex -= 1;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.pictureBox1.Image = System.Drawing.Image.FromFile(this.openFileDialog1.FileName);
            }
        }
        
        

        private void button8_Click_1(object sender, EventArgs e)
        {
            //=========================
            //基本資料

            //大頭照
            if (pictureBox1.Image != null)
            {
                byte[] bytes;
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                this.pictureBox1.Image.Save(ms, ImageFormat.Jpeg);
                bytes = ms.GetBuffer();


                //linq-insertImage

                Image i = new Image { Name = "resume", Image1 = bytes };

                this.db.Images.Add(i);
                this.db.SaveChanges();
                //=========================

                var q = from p in this.db.Educations
                        select p;

                Resume f = new Resume
                {
                    MemberID = int.Parse(currentID),
                    FullName = this.textBox3.Text,
                    IdentityID = this.textBox1.Text,
                    PhoneNumber = this.textBox2.Text,
                    ResumeContent = this.richTextBox1.Text,
                    WorkExp = this.textBox5.Text,
                    FormID = 1,
                    ResumeStatusID = 1,
                    EDID = q.ToList()[this.comboBox1.SelectedIndex].EDID,
                    ImageID = i.ImageID,
                };


                this.db.Resumes.Add(f);
                this.db.SaveChanges();
                //=========================
                //個人經歷


                //=========================
                //求職條件

                //=========================
                //技能專長
                //todo
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
                    ResumeSkill resumeskill = new ResumeSkill
                    {
                        ResumeID = f.ResumeID,
                        SkillID = skillid
                    };
                    this.db.ResumeSkills.Add(resumeskill);
                }
                this.db.SaveChanges();
                //=========================
                //自傳附件
                //=========================
                MessageBox.Show("新增成功");
                this.tabControl2.SelectedIndex = 0;
                LoadMyResume();
                LoadCreatePage();

            }
            else
            {
                MessageBox.Show("請選擇大頭照");
            }
        }



        private void listBox2_DoubleClick(object sender, EventArgs e)
        {
            //=================================
            //treeview
            //todo 點listbox在treeview加入當下點選的大類及小類，點多少加多少

            //this.treeView1.Nodes.Clear();

            //var q = from p in this.db.Skills
            //        where p.Name.Equals(this.listBox2.Text)
            //        group p by p.SkillClass into g
            //        select new { skillclass = g.Key.Name, mygroup = g };

            //foreach(var s in q)
            //{
            //    //saving = this.treeView1.Nodes.Add(s.skillclass.ToString());
            //    saving = this.treeView1.Nodes.Add(s.skillclass);
            //    foreach (var items in s.mygroup)
            //    {
            //        saving.Nodes.Add(items.Name);
            //    }
            //    saving.Toggle();
            //}

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

            this.listBox2.Items.Remove(this.listBox2.SelectedItem);

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

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            DialogResult result= MessageBox.Show("確定要刪除嗎?","刪除履歷",MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                //JobResumes
                var jr = from p in this.db.JobResumes.AsEnumerable()
                         where p.ResumeID == int.Parse(this.dataGridView2.CurrentRow.Cells[0].Value.ToString())
                         select p;
                if (jr == null) { return; }
                foreach (var g in jr)
                {
                    this.db.JobResumes.Remove(g);
                }

                //ResumeCertificates
                var rc = from p in this.db.ResumeCertificates.AsEnumerable()
                         where p.ResumeID == int.Parse(this.dataGridView2.CurrentRow.Cells[0].Value.ToString())
                         select p;
                if (rc == null) { return; }
                foreach (var c in rc)
                {
                    this.db.ResumeCertificates.Remove(c);
                }

                //ResumeSkills
                var s = from p in this.db.ResumeSkills.AsEnumerable()
                        where p.ResumeID == int.Parse(this.dataGridView2.CurrentRow.Cells[0].Value.ToString())
                        select p;

                if (s == null) { return; }
                foreach (var x in s)
                {
                    this.db.ResumeSkills.Remove(x);
                }

                this.db.SaveChanges();

                //resumes
                var q = (from p in this.db.Resumes.AsEnumerable()
                         where p.ResumeID == int.Parse(this.dataGridView2.CurrentRow.Cells[0].Value.ToString())
                         select p).FirstOrDefault();

                if (q == null) { return; }
                this.db.Resumes.Remove(q);
                this.db.SaveChanges();

                LoadMyResume();
                LoadMySendResumes();
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {

            DialogResult result = MessageBox.Show("此操作會同時將所有已投遞履歷撤回\r要繼續請按確定嗎?", "刪除履歷", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                //從所有公司接收的所有履歷中找出符合所選項目的履歷ID
                var q = from p in this.db.JobResumes.AsEnumerable()
                        where p.ResumeID == int.Parse(this.dataGridView1.CurrentRow.Cells[0].Value.ToString())
                        select p;
                if (q == null) { return; }
                foreach (var x in q)
                {
                    this.db.JobResumes.Remove(x);
                }

                //從證照中找出符合所選項目的履歷ID
                var qq = from p in this.db.ResumeCertificates.AsEnumerable()
                         where p.ResumeID == int.Parse(this.dataGridView1.CurrentRow.Cells[0].Value.ToString())
                         select p;
                foreach (var x in qq)
                {
                    this.db.ResumeCertificates.Remove(x);
                }

                //從技能中找出符合所選項目的履歷ID
                var qqq = from p in this.db.ResumeSkills.AsEnumerable()
                          where p.ResumeID == int.Parse(this.dataGridView1.CurrentRow.Cells[0].Value.ToString())
                          select p;
                foreach (var x in qqq)
                {
                    this.db.ResumeSkills.Remove(x);
                }

                //儲存
                this.db.SaveChanges();

                //從我的履歷中刪除所選項目的履歷ID
                var r = (from p in this.db.Resumes.AsEnumerable()
                         where p.ResumeID == int.Parse(this.dataGridView1.CurrentRow.Cells[0].Value.ToString())
                         select p).FirstOrDefault();
                if (r == null) { return; }


                this.db.Resumes.Remove(r);
                this.db.SaveChanges();


                LoadMySendResumes();
            }
            

        }

        private void button6_Click(object sender, EventArgs e)
        {
            var q = (from p in this.db.JobResumes.AsEnumerable()
                     where p.ResumeID == int.Parse(this.dataGridView1.CurrentRow.Cells[0].Value.ToString())
                     select p).FirstOrDefault();
            if (q == null) { return; }

            this.db.JobResumes.Remove(q);
            this.db.SaveChanges();
            LoadMySendResumes();

        }

        private void button2_Click_1(object sender, EventArgs e)
        {

        }

        private void button13_Click(object sender, EventArgs e)
        {
            var q = (from p in this.db.Resumes.AsEnumerable()
                     where p.ResumeID == int.Parse(this.dataGridView2.CurrentRow.Cells[0].Value.ToString())
                     select p).FirstOrDefault();

            if (q == null) { return; }

            if (q.ResumeStatusID == 2)
            {
                q.ResumeStatusID = 1;
            }
            else if (q.ResumeStatusID == 1)
            {
                q.ResumeStatusID = 2;
            }

            this.db.SaveChanges();
            LoadMyResume();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var q = (from p in this.db.JobResumes.AsEnumerable()
                    where p.ResumeID == int.Parse(this.dataGridView1.CurrentRow.Cells[0].Value.ToString()) && p.JobID == int.Parse(this.dataGridView1.CurrentRow.Cells[2].Value.ToString())
                    select new
                    {
                        履歷編號=p.ResumeID,
                        會員編號=p.Resume.MemberID,
                        工作編號=p.JobID,
                        公司名稱=p.Job_Opportunities.Firm.FirmName,
                        狀態=p.Status.Name,
                        身份證字號=p.Resume.IdentityID,
                        手機號碼=p.Resume.PhoneNumber,
                        教育程度=p.Resume.Education.Name,
                        工作經驗=p.Resume.WorkExp,
                        //通訊地址=
                        自我介紹=p.Resume.ResumeContent
                    }).FirstOrDefault();
            
            this.textBox4.Text = q.履歷編號.ToString();
            this.textBox9.Text=q.會員編號.ToString();
            this.textBox10.Text=q.工作編號.ToString() ;
            this.textBox11.Text = q.公司名稱.ToString();
            this.textBox12.Text = q.狀態;
            this.textBox13.Text = q.身份證字號;
            this.textBox14.Text = q.手機號碼;
            this.textBox16.Text = q.教育程度;
            this.textBox17.Text = q.工作經驗 + "年";
            this.richTextBox2.Text = q.自我介紹;
        }

        

        private void button18_Click(object sender, EventArgs e)
        {
            //var j = from p in this.db.Job_Opportunities
            //        select p;
            //var q = (from p in this.db.Job_Opportunities.AsEnumerable()
            //         where p.JobID == j.ToList()[this.listBox5.SelectedIndex].JobID
            //         select p).FirstOrDefault();
            //var r = (from p in this.db.Resumes.AsEnumerable()
            //         where p.ResumeID == int.Parse(this.dataGridView3.CurrentRow.Cells[0].Value.ToString())
            //         select p).FirstOrDefault();

            //JobResume jr = new JobResume
            //{
            //    JobID = q.JobID,
            //    ResumeID=r.ResumeID,
            //    ApplyStatusID=5
            //};
            //this.db.JobResumes.Add(jr);
            //this.db.SaveChanges();
            //LoadMySendResumes();
            //MessageBox.Show("應徵成功!");
        }

        private void dataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
            if (dataGridView4.Columns[e.ColumnIndex].Name == "我要應徵" && e.RowIndex >= 0)
            {
                
                int selectIndex = e.RowIndex;

                var j = from p in this.db.Job_Opportunities
                        select p;

                var o = (from p in this.db.JobResumes.AsEnumerable()
                        where p.ResumeID == int.Parse(this.dataGridView3.CurrentRow.Cells[0].Value.ToString()) 
                        && p.JobID == j.ToList()[selectIndex].JobID
                        select p).FirstOrDefault();

                if (o==null)
                {
                    
                    
                    var q = (from p in this.db.Job_Opportunities.AsEnumerable()
                             where p.JobID == j.ToList()[selectIndex].JobID
                             select p).FirstOrDefault();
                    var r = (from p in this.db.Resumes.AsEnumerable()
                             where p.ResumeID == int.Parse(this.dataGridView3.CurrentRow.Cells[0].Value.ToString())
                             select p).FirstOrDefault();

                    JobResume jr = new JobResume
                    {
                        JobID = q.JobID,
                        ResumeID = r.ResumeID,
                        ApplyStatusID = 5
                    };
                    this.db.JobResumes.Add(jr);
                    this.db.SaveChanges();

                    LoadMySendResumes();
                    
                    MessageBox.Show("應徵成功!");
                }
                else
                {
                    MessageBox.Show("此工作機會已有您的應徵紀錄，請耐心等候");
                }
            }
        }

        private void dataGridView4_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
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



        private void button14_Click(object sender, EventArgs e)
        {
            this.button16.Enabled = true;
            this.button17.Enabled = true;
            checkBox4.Enabled = true;
            checkBox5.Enabled = true;
            this.textBox20.Enabled = true;
            this.comboBox2.Enabled = true;
            this.textBox15.Enabled = true;
            this.richTextBox3.Enabled = true;
            this.button19.Enabled = true;
        }

        private void button15_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer4_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }


        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            LoadMyResumeDetials();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            enabledFalse();
            LoadMyResumeDetials();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox4.Checked)
            {this.checkBox5.Checked = false;}
            else if (checkBox4.Checked == false && checkBox5.Checked == false)
            {this.checkBox4.Checked = true;}
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox5.Checked)
            {this.checkBox4.Checked = false;}
            else if (checkBox4.Checked == false && checkBox5.Checked == false)
            {this.checkBox5.Checked = true;}
        }

        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(this.tabControl2.SelectedIndex == 0)
            {
                LoadMyResume();
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            var qq = (from p in this.db.Resumes.AsEnumerable()
                      where p.ResumeID == int.Parse(textBox26.Text)
                      select p).FirstOrDefault();
            if (qq == null) { return; }


            //判斷狀態為何，並根據狀態給ID數值
            int a;
            if (checkBox4.Checked) { a = 1; }
            else { a = 2; }


            //圖片
            byte[] bytes;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            this.pictureBox3.Image.Save(ms, ImageFormat.Jpeg);
            bytes = ms.GetBuffer();


            //update
            qq.ResumeStatusID = a;
            qq.IdentityID = this.textBox21.Text;
            qq.PhoneNumber = this.textBox20.Text;
            qq.EDID = this.comboBox2.SelectedIndex + 1;
            qq.WorkExp = this.textBox15.Text;
            qq.ResumeContent = this.richTextBox3.Text;
            qq.Image.Image1 = bytes;


            this.db.SaveChanges();

            LoadMyResume();
            enabledFalse();
            MessageBox.Show("修改成功");


        }

        

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void button19_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.pictureBox3.Image = System.Drawing.Image.FromFile(this.openFileDialog1.FileName);
            }
        }


        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
    }
}
