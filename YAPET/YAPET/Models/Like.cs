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
    
    public partial class Like
    {
        public int LikeNo { get; set; }
        public Nullable<int> UserNo { get; set; }
        public int PostNo { get; set; }
        public System.DateTime Time { get; set; }
        public bool State { get; set; }
    
        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
    }
}
