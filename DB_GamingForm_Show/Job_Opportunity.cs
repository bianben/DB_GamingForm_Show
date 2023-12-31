//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace DB_GamingForm_Show
{
    using System;
    using System.Collections.Generic;
    
    public partial class Job_Opportunity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Job_Opportunity()
        {
            this.JobAdvertises = new HashSet<JobAdvertise>();
            this.JobCertificates = new HashSet<JobCertificate>();
            this.JobResumes = new HashSet<JobResume>();
            this.JobSkills = new HashSet<JobSkill>();
        }
    
        public int JobID { get; set; }
        public int FirmID { get; set; }
        public int RegionID { get; set; }
        public int RequiredNum { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public string Salary { get; set; }
        public string JobExp { get; set; }
        public string JobContent { get; set; }
        public int JobStatusID { get; set; }
        public int EDID { get; set; }
    
        public virtual Education Education { get; set; }
        public virtual Firm Firm { get; set; }
        public virtual Region Region { get; set; }
        public virtual Status Status { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JobAdvertise> JobAdvertises { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JobCertificate> JobCertificates { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JobResume> JobResumes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JobSkill> JobSkills { get; set; }
    }
}
