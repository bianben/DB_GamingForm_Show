using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shopping;
using WindowsFormsApp1;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using DB_GamingForm_Show;
using Gaming_Forum;

namespace WindowsFormsApp1
{
    public partial class FrmProductEvaluate : Form
    {
        DB_GamingFormEntities db = new DB_GamingFormEntities();
        public int ID { get; set; }
        public bool IsFirm { get; set; }

        private void MemberFirm()
        {
            if (ClassUtility.FirmID != 0)
            {
                ID = ClassUtility.FirmID;
                IsFirm = true;
            }
            else
            {
                ID = ClassUtility.MemberID;
                IsFirm = false;
            }
        }
        public FrmProductEvaluate(int SelectProID,int MemberID)
        {
            InitializeComponent();
            
            //傳進來的產品ID值
            InSelectID = SelectProID;
            InMemberID=MemberID;
            MemberFirm();
            button2.Enabled = false;
        }
        //存傳進來的值
        int InSelectID;
        int InMemberID;

        private void button1_Click(object sender, EventArgs e)
        {
            ProductEvaluate eVAValue = new ProductEvaluate { ProductID = InSelectID, MemberID = InMemberID, EvaLevel = "", EvaContent = textBox1.Text };
            this.db.ProductEvaluates.Add(eVAValue);
            this.db.SaveChanges();
            textBox1.Text= "";

            //問老師
            //dataGridView1.DataSource = null;
            //var DataLoad = from aa in db.ProductEvaluates
            //               where aa.ProductID == InSelectID
            //               select new { 會員 = aa.Product.Member.Name, 評論 = aa.EvaContent };
            //this.dataGridView1.DataSource = DataLoad.ToList();
            RefreshData();


        }

        private void RefreshData()
        {
            dataGridView1.DataSource = null;
            var DataLoad = from aa in db.ProductEvaluates
                           where aa.ProductID == InSelectID
                           select new {發文編號=aa.ID,會員ID=aa.MemberID ,會員 = aa.Member.Name, 評論 = aa.EvaContent };
                             this.dataGridView1.DataSource = DataLoad.ToList();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void FrmProductEvaluate_Load(object sender, EventArgs e)
        {
            RefreshData();  
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {   //如果點擊右鍵&&是有行跟列
            if(e.Button==MouseButtons.Right&&   e.ColumnIndex==0 && e.RowIndex >= 0 && (int)dataGridView1.CurrentRow.Cells["會員ID"].Value== InMemberID) 
            {
                DataGridViewCell clickedCell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                Point clickLocation = dataGridView1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false).Location;
                contextMenuStrip1.Show(dataGridView1,clickLocation);
            }
        }

        private void 刪除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var DelContent = (from p in this.db.ProductEvaluates.AsEnumerable()
                              where p.ID == (int)dataGridView1.CurrentRow.Cells["發文編號"].Value
                              select p).FirstOrDefault();

                             if (DelContent == null) return;

            this.db.ProductEvaluates.Remove(DelContent);
            this.db.SaveChanges();

            this.RefreshData();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void 修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.Enabled = false;
            button2.Enabled = true;
            textBox1.Text = "請輸入欲修改內容";
            var DelContent = (from p in this.db.ProductEvaluates.AsEnumerable()
                              where p.ID == (int)dataGridView1.CurrentRow.Cells["發文編號"].Value
                              select p).FirstOrDefault();

            if (DelContent == null) return;

            this.button2.Click += (object sender1, EventArgs e1) =>
            {

                DelContent.EvaContent = textBox1.Text;
                this.db.SaveChanges();
                this.RefreshData();
                button2.Enabled = false;
                textBox1.Text = "";
                dataGridView1.Enabled = true;
            };


        }
    }
}
