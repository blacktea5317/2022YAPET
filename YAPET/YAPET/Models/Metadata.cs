using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace YAPET.Models
{
    //複製用
    //using System.ComponentModel.DataAnnotations;

    //[MetadataType(typeof(MetaActivity))]
    public class MetaActivity
    {
        [Key]
        [DisplayName("活動資訊編號")]
        public string ActivityNo { get; set; }
        [DisplayName("管理員編號")]
        public string AdmNo { get; set; }
        [DisplayName("主旨")]
        [Required(ErrorMessage = "此欄位為必填")]
        [StringLength(30, ErrorMessage = "主旨最多30個字")]
        public string Title { get; set; }
        [DisplayName("內容")]
        [Required(ErrorMessage = "此欄位為必填")]
        public string Content { get; set; }
        [DisplayName("圖片")]
        public byte[] Photo { get; set; }
        [DisplayName("發布日")]
        [Required(ErrorMessage = "此欄位為必填")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public System.DateTime Date { get; set; }
        [DisplayName("截止日")]
        [Required(ErrorMessage = "此欄位為必填")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public System.DateTime Deadline { get; set; }
        public string ImageMimeType { get; set; }
    }

    public class MetaAdm
    {
        [Key]
        [DisplayName("管理員編號")]
        public string AdmNo { get; set; }
        [DisplayName("帳號")]
        [Required(ErrorMessage = "此欄位為必填")]
        [RegularExpression("[a-zA-Z0-9_]{4,30}", ErrorMessage = "請填寫4~30個英文或數字")]
        public string AdmId { get; set; }
        [DisplayName("密碼")]
        [Required(ErrorMessage = "此欄位為必填")]
        [RegularExpression("[a-zA-Z0-9_]{4,30}", ErrorMessage = "請填寫4~30個英文或數字")]
        public string AdmPwd { get; set; }
    }

    public class MetaAdopt
    {
        [Key]
        [DisplayName("領養編號")]
        public string AdoptNo { get; set; }
        [DisplayName("照片")]
        public byte[] Photo { get; set; }
        [DisplayName("寵物名")]
        [Required(ErrorMessage = "此欄位為必填")]
        [StringLength(20, ErrorMessage = "最多20個字")]
        public string Name { get; set; }
        [DisplayName("種類")]
        [Required(ErrorMessage = "此欄位為必填")]
        public string SpeciesNo { get; set; }
        [DisplayName("性別")]
        [Required(ErrorMessage = "此欄位為必填")]
        public string GenderNo { get; set; }
        [DisplayName("體型")]
        [Required(ErrorMessage = "此欄位為必填")]
        public string SizeNo { get; set; }
        [DisplayName("顏色")]
        [Required(ErrorMessage = "此欄位為必填")]
        [StringLength(10, ErrorMessage = "最多10個字")]
        public string Color { get; set; }
        [DisplayName("年齡")]
        [Required(ErrorMessage = "此欄位為必填")]
        [StringLength(10, ErrorMessage = "最多10個字")]
        public string Age { get; set; }
        [DisplayName("狀態")]
        public Nullable<bool> State { get; set; }
        [DisplayName("是否結紮")]
        public Nullable<bool> LigationState { get; set; }
        [DisplayName("聯絡人")]
        [Required(ErrorMessage = "此欄位為必填")]
        [StringLength(20, ErrorMessage = "最多20個字")]
        public string ContactPerson { get; set; }
        [DisplayName("聯絡方式")]
        [Required(ErrorMessage = "此欄位為必填")]
        [StringLength(100, ErrorMessage = "最多100個字")]
        public string Contact { get; set; }
        [DisplayName("詳細資訊")]
        public string Details { get; set; }
        [DisplayName("晶片編號")]
        [StringLength(20, ErrorMessage = "最多20個字")]
        public string ChipNumber { get; set; }
        [DisplayName("張貼會員")]
        public int UserNo { get; set; }
        [DisplayName("所在城市")]
        [Required(ErrorMessage = "此欄位為必填")]
        public string CityNo { get; set; }
        [DisplayName("張貼時間")]
        public System.DateTime Date { get; set; }
        public string ImageMimeType { get; set; }
    }

    public class MetaCity
    {
        [DisplayName("縣市編號")]
        public string CityNo { get; set; }
        [DisplayName("縣市")]
        public string CityName { get; set; }
    }

    public class MetaFreeze
    {
        [Key]
        [DisplayName("凍結編號")]
        public string FreezeNo { get; set; }
        [DisplayName("會員編號")]
        public int UserNo { get; set; }
        [DisplayName("起始時間")]
        public System.DateTime StartDays { get; set; }
        [DisplayName("結束時間")]
        public System.DateTime EndDays { get; set; }
        [DisplayName("原因")]
        public string Reason { get; set; }
        [DisplayName("管理員編號")]
        public string AdmNo { get; set; }
        [DisplayName("是否凍結")]
        public bool State { get; set; }
    }

    public class MetaFrom
    {
        [Key]
        [DisplayName("表單編號")]
        public string FromNo { get; set; }
        [DisplayName("問題編號")]
        //[Required(ErrorMessage = "此欄位為必填")]
        public string ProblemNo { get; set; }
        [DisplayName("聯絡信箱")]
        //[Required(ErrorMessage = "此欄位為必填")]
        //[RegularExpression("^[0-9a-zA-Z]+([0-9a-zA-Z]*[-._+])*[0-9a-zA-Z]+@[0-9a-zA-Z]+([-.][0-9a-zA-Z]+)*([0-9a-zA-Z]*[.])[a-zA-Z]{2,6}$", ErrorMessage = "信箱格式錯誤")]
        public string Mail { get; set; }
        [DisplayName("內容")]
        //[Required(ErrorMessage = "此欄位為必填")]
        public string Content { get; set; }
        [DisplayName("狀態")]
        public bool State { get; set; }
    }

    public class MetaIntroduction
    {
        [Key]
        [DisplayName("平台消息編號")]
        public string IntroductionNo { get; set; }
        [DisplayName("管理員編號")]
        public string AdmNo { get; set; }
        [DisplayName("主旨")]
        [Required(ErrorMessage = "此欄位為必填")]
        [StringLength(30, ErrorMessage = "最多30個字")]
        public string Title { get; set; }
        [DisplayName("內容")]
        [Required(ErrorMessage = "此欄位為必填")]
        public string Content { get; set; }
        [DisplayName("照片")]
        public byte[] Photo { get; set; }
        [DisplayName("發布日")]
        public System.DateTime Date { get; set; }
        public string ImageMimeType { get; set; }
    }

    public class MetaKnowledge
    {
        [Key]
        [DisplayName("知識小學堂編號")]
        public string KnowledgeNo { get; set; }
        [DisplayName("管理員編號")]
        public string AdmNo { get; set; }
        [DisplayName("主旨")]
        [Required(ErrorMessage = "此欄位為必填")]
        [StringLength(30, ErrorMessage = "最多30個字")]
        public string Title { get; set; }
        [DisplayName("內容")]
        [Required(ErrorMessage = "此欄位為必填")]
        public string Content { get; set; }
        [DisplayName("照片")]
        public byte[] Photo { get; set; }
        [DisplayName("發布日")]
        public System.DateTime Date { get; set; }
    }

    public class MetaLost
    {
        [Key]
        [DisplayName("走失編號")]
        public string LostNo { get; set; }
        [DisplayName("照片")]
        public byte[] Photo { get; set; }
        [DisplayName("寵物名")]
        [Required(ErrorMessage = "此欄位為必填")]
        [StringLength(20, ErrorMessage = "最多20個字")]
        public string Name { get; set; }
        [DisplayName("種類")]
        [Required(ErrorMessage = "此欄位為必填")]
        public string SpeciesNo { get; set; }
        [DisplayName("性別")]
        [Required(ErrorMessage = "此欄位為必填")]
        public string GenderNo { get; set; }
        [DisplayName("體型")]
        [Required(ErrorMessage = "此欄位為必填")]
        public string SizeNo { get; set; }
        [DisplayName("顏色")]
        [Required(ErrorMessage = "此欄位為必填")]
        [StringLength(10, ErrorMessage = "最多10個字")]
        public string Color { get; set; }
        [DisplayName("年齡")]
        [Required(ErrorMessage = "此欄位為必填")]
        [StringLength(10, ErrorMessage = "最多10個字")]
        public string Age { get; set; }
        [DisplayName("走失時間")]
        [Required(ErrorMessage = "此欄位為必填")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public System.DateTime Time { get; set; }
        [DisplayName("走失地點")]
        [Required(ErrorMessage = "此欄位為必填")]
        [StringLength(100, ErrorMessage = "最多100個字")]
        public string Place { get; set; }
        [DisplayName("聯絡人")]
        [Required(ErrorMessage = "此欄位為必填")]
        [StringLength(20, ErrorMessage = "最多20個字")]
        public string ContactPerson { get; set; }
        [DisplayName("連絡方式")]
        [Required(ErrorMessage = "此欄位為必填")]
        [StringLength(100, ErrorMessage = "最多100個字")]
        public string Contact { get; set; }
        [DisplayName("詳細資料")]
        public string Details { get; set; }
        [DisplayName("晶片號碼")]
        [StringLength(20, ErrorMessage = "最多20個字")]
        public string ChipNumber { get; set; }
        [DisplayName("刊登會員")]
        public int UserNo { get; set; }
        [DisplayName("走失城市")]
        [Required(ErrorMessage = "此欄位為必填")]
        public string CityNo { get; set; }
        [DisplayName("刊登日期")]
        public System.DateTime Date { get; set; }
    }

    public class MetaMsgReport
    {
        [Key]
        [DisplayName("留言檢舉編號")]
        public int MsgReportNo { get; set; }
        [DisplayName("留言編號")]
        public int MessageNo { get; set; }
        [DisplayName("使用者編號")]
        public int UserNo { get; set; }
        [DisplayName("時間")]
        public System.DateTime Time { get; set; }
        [DisplayName("原因")]
        [Required(ErrorMessage = "此欄位為必填")]      
        public string Reason { get; set; }
        [DisplayName("是否移除")]
        public bool State { get; set; }
    }
    public  class MetaBlack
    {
        [Key]
        [DisplayName("黑單編號")]
        public int BlackNo { get; set; }
        [DisplayName("會員編號")]
        public int UserNo { get; set; }
        [DisplayName("被黑單會員編號")]
        public int BlackedNo { get; set; }
        [DisplayName("是否封鎖")]
        public bool State { get; set; }

      
    }

    public class MetaPetGender
    {
        [Key]
        [DisplayName("性別編號")]
        public string GenderNo { get; set; }
        [DisplayName("性別")]
        public string Gender { get; set; }
    }

    public class MetaPetSize
    {
        [Key]
        [DisplayName("體型編號")]
        public string SizeNo { get; set; }
        [DisplayName("體型")]
        public string Size { get; set; }
    }

    public class MetaPhoto
    {
        [Key]
        [DisplayName("照片編號")]
        public int PhotoNo { get; set; }
        [DisplayName("貼文編號")]
        public int PostNo { get; set; }
        [DisplayName("圖片")]
        public byte[] Photo1 { get; set; }
    }

    public class MetaPost
    {
        [DisplayName("貼文編號")]
        public int PostNo { get; set; }
        [DisplayName("貼文會員")]
        [Required(ErrorMessage = "此欄位為必填")]
        public int UserNo { get; set; }
        [DisplayName("貼文標題")]
        [Required(ErrorMessage = "此欄位為必填")]
        [StringLength(30, ErrorMessage = "最多30個字")]
        public string Title { get; set; }
        [DisplayName("貼文內容")]
        [Required(ErrorMessage = "此欄位為必填")]
        public string Content { get; set; }
        [DisplayName("發布日期")]
        public System.DateTime Date { get; set; }
        [DisplayName("貼文狀態")]
        [Required(ErrorMessage = "此欄位為必填")]
        public string State { get; set; }
        [DisplayName("貼文分類")]
        [Required(ErrorMessage = "此欄位為必填")]
        public string PostTypeNo { get; set; }
    }

    public class MetaPostState
    {
        [Key]
        [DisplayName("貼文狀態編號")]
        [Required(ErrorMessage = "此欄位為必填")]
        public string StateNo { get; set; }
        [DisplayName("貼文狀態")]
        [Required(ErrorMessage = "此欄位為必填")]
        [StringLength(10, ErrorMessage = "最多10個字")]
        public string StateName { get; set; }
    }


    public class MetaPostType
    {
        [Key]
        [DisplayName("貼文類型編號")]
        [Required(ErrorMessage = "此欄位為必填")]
        public string PostTypeNo { get; set; }
        [DisplayName("貼文類型")]
        [Required(ErrorMessage = "此欄位為必填")]
        [StringLength(10, ErrorMessage = "最多10個字")]
        public string PostType1 { get; set; }
    }

    public class MetaProblem
    {
        [Key]
        [DisplayName("問題類型編號")]
        [Required(ErrorMessage = "此欄位為必填")]
        public string ProblemNo { get; set; }
        [DisplayName("問題類型")]
        [Required(ErrorMessage = "此欄位為必填")]
        [StringLength(50, ErrorMessage = "最多50個字")]
        public string ProblemName { get; set; }
    }

    public class MetaReport
    {
        [Key]
        [DisplayName("貼文檢舉編號")]
        public int ReportNo { get; set; }
        [Key]
        [DisplayName("貼文編號")]
        public int PostNo { get; set; }
        [Key]
        [DisplayName("會員編號")]
        public int UserNo { get; set; }
        [Key]
        [DisplayName("時間")]
        public System.DateTime Time { get; set; }
        [Key]
        [DisplayName("原因")]
        public string Reason { get; set; }
        [Key]
        [DisplayName("是否移除")]
        public bool State { get; set; }
    }

    public class MetaSpecies
    {
        [Key]
        [DisplayName("種類編號")]
        [Required(ErrorMessage = "此欄位為必填")]
        public string SpeciesNo { get; set; }
        [DisplayName("種類")]
        [Required(ErrorMessage = "此欄位為必填")]
        [StringLength(10, ErrorMessage = "最多10個字")]
        public string SpeciesName { get; set; }
    }

    public class MetaUser
    {
        [DisplayName("會員編號")]
        public int UserNo { get; set; }
        [DisplayName("帳號")]
        [Required(ErrorMessage = "此欄位為必填")]
        [RegularExpression("[a-zA-Z0-9_]{4,30}", ErrorMessage = "請填寫4~30個英文或數字")]
        public string UserId { get; set; }
        [DisplayName("密碼")]
        [Required(ErrorMessage = "此欄位為必填")]
        public string UserPwd { get; set; }
        [DisplayName("姓名")]
        [Required(ErrorMessage = "此欄位為必填")]
        [StringLength(20, ErrorMessage = "最多20個字")]
        public string Name { get; set; }
        [DisplayName("生日")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> Birthday { get; set; }
        [DisplayName("性別")]
        public Nullable<bool> Gender { get; set; }
        [DisplayName("信箱")]
        [Required(ErrorMessage = "此欄位為必填")]
        [StringLength(100, ErrorMessage = "最多100個字")]
        [RegularExpression("^[0-9a-zA-Z]+([0-9a-zA-Z]*[-._+])*[0-9a-zA-Z]+@[0-9a-zA-Z]+([-.][0-9a-zA-Z]+)*([0-9a-zA-Z]*[.])[a-zA-Z]{2,6}$", ErrorMessage = "信箱格式錯誤")]
        public string EMail { get; set; }
        [DisplayName("頭貼")]
        public byte[] Herder { get; set; }
        [DisplayName("電話")]
        [StringLength(20, ErrorMessage = "最多20個字")]
        public string Telephone { get; set; }
        [DisplayName("地址")]
        [StringLength(100, ErrorMessage = "最多100個字")]
        public string Address { get; set; }
        [DisplayName("狀態")]
        public Nullable<bool> State { get; set; }
        [DisplayName("暱稱")]
        [StringLength(20, ErrorMessage = "最多20個字")]
        public string Nickname { get; set; }
        [DisplayName("圖片類型")]
        public string ImageMimeType { get; set; }
        [DisplayName("關於我")]
        public string AboutMe { get; set; }
    }
}