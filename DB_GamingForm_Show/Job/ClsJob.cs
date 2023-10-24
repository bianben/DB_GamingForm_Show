using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_GamingForm_Show.Job
{
    internal class ClsJob
    {
        #region 待整合
        //Todo:整合code到class
        DB_GamingFormEntities entities = new DB_GamingFormEntities();
        public class JobResult
        {
            public string Firm { get; set; }
            public string Region { get; set; }
            public int RequireNum { get; set; }

            public string ModerfiedDate { get; set; }

            public string Salary { get; set; }
            public string JobExp { get; set; }

            public string JobContent { get; set; }

            public string Status { get; set; }

            public string EducationRequirements { get; set; }

        }
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

        public void DataInsert()
        {   
            Random rnd = new Random();
            var data = from n in this.entities.Job_Opportunities.AsEnumerable()
                       select new
                       {    
                           
                           n.Firm.FirmName,
                           n.Region.City,
                           n.RequiredNum,
                           ModifiedDate = n.ModifiedDate.ToString("d"),
                           n.Salary,
                           n.JobExp,
                           n.JobContent,
                           Status = n.Status.Name,
                           EducationRequirements = n.Education.Name
                       };

            //this.entities.Job_Opportunities.Add(
            //    new Job_Opportunity 
            //    {
            //       FirmID = 

            //    }
            //    );
            
            

        }
        

        internal void HotSearch(string source,string source1,string source2)
        {
            var value = (from n in this.entities.SerachRecords.AsEnumerable()
                         where n.IsMember == false
                         group n by n.Name into q
                         orderby q.Count() descending
                         select q.Key).ToList();
            source = value.ToList()[0].ToString();
            source1 = value.ToList()[1].ToString();
            source2 = value.ToList()[2].ToString();


        }
        #endregion
    }
}
