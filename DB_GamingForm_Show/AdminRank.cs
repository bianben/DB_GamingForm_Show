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
    
    public partial class AdminRank
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AdminRank()
        {
            this.Admins = new HashSet<Admin>();
        }
    
        public int RankID { get; set; }
        public bool RK_Product { get; set; }
        public bool RK_Member { get; set; }
        public bool RK_Blog { get; set; }
        public bool RK_Firm { get; set; }
        public bool RK_Order { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Admin> Admins { get; set; }
    }
}
