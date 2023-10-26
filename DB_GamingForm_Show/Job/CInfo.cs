using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DB_GamingForm_Show.Job
{

    public class CInfo
    {

        DB_GamingFormEntities db = new DB_GamingFormEntities();


        public void ConfirmInvite()
        {
            var q = from p in db.JobResumes.AsEnumerable()
                    where p.Resume.MemberID == int.Parse(CMyInfo.currentID) && p.ApplyStatusID == 9
                    select p;
            if (q.Any())
            {
                MessageBox.Show($"您有{q.Count()}份面試邀請，祝您面試順利");
            }
        }

        public void LoadMyInfo(int s)
        {
            var q = (from p in this.db.Members.AsEnumerable()
                     where p.MemberID == s
                     select p).FirstOrDefault();

            CMyInfo.Name = q.Name;
            CMyInfo.IdentityID = q.Resumes.FirstOrDefault().IdentityID;
            CMyInfo.Email = q.Email;
            CMyInfo.MyContend = q.Mycomment;
            CMyInfo.PhoneNumber = q.Phone;
            CMyInfo.WorkExp = q.Resumes.FirstOrDefault().WorkExp;
            CMyInfo.EDID = q.Resumes.FirstOrDefault().EDID;
        }

        //List<CMyResume> _list = null;
        public void LoadMyresume(int s)
        {
            //_list = new List<CMyResume>();
            //CMyResume x= new CMyResume();

            var q = (from p in db.Resumes.AsEnumerable()
                      where p.ResumeID == s
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

            CMyResumeDetial.resumeID = q.履歷編號;
            CMyResumeDetial.memberID = q.會員編號;
            CMyResumeDetial.resumestateID = (int)q.狀態;
            CMyResumeDetial.identityID = q.身份證字號;
            CMyResumeDetial.phoneNumber = q.手機號碼;
            CMyResumeDetial.edID = (int)q.教育程度編號;
            CMyResumeDetial.workExp = q.工作經驗;
            CMyResumeDetial.resumeContend = q.自我介紹;
            CMyResumeDetial.email = q.電子信箱;
            CMyResumeDetial.image = q.大頭照;
        }
        
    }

    
}
