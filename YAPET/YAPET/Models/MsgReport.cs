//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace YAPET.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    [MetadataType(typeof(MetaMsgReport))]
    public partial class MsgReport
    {
        public int MsgReportNo { get; set; }
        public int MessageNo { get; set; }
        public int UserNo { get; set; }
        public System.DateTime Time { get; set; }
        public string Reason { get; set; }
        public bool State { get; set; }
    
        public virtual Message Message { get; set; }
        public virtual User User { get; set; }
    }
}
