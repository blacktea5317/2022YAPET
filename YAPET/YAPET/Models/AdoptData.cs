using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace YAPET.Models
{
    public class AdoptData
    {
        [DisplayName("動物的流水編號")]
        public int animal_id { get; set; }
        [DisplayName("動物的區域編號")]
        public string animal_subid { get; set; }
        [DisplayName("動物所屬縣市代碼")]
        public int animal_area_pkid { get; set; }
        [DisplayName("動物所屬收容所代碼")]
        public int animal_shelter_pkid { get; set; }
        [DisplayName("動物的實際所在地")]
        public string animal_place { get; set; }
        [DisplayName("動物的類型")]
        public string animal_kind { get; set; }
        [DisplayName("動物性別")]
        public string animal_sex { get; set; }
        [DisplayName("動物體型")]
        public string animal_bodytype { get; set; }
        [DisplayName("動物毛色")]
        public string animal_colour { get; set; }
        [DisplayName("動物年紀")]
        public string animal_age { get; set; }
        [DisplayName("是否絕育")]
        public string animal_sterilization { get; set; }
        [DisplayName("是否施打狂犬病疫苗")]
        public string animal_bacterin { get; set; }
        [DisplayName("動物尋獲地")]
        public string animal_foundplace { get; set; }
        [DisplayName("動物網頁標題")]
        public string animal_title { get; set; }
        [DisplayName("動物狀態")]
        public string animal_status { get; set; }
        [DisplayName("資料備註")]
        public string animal_remark { get; set; }
        [DisplayName("其他說明")]
        public string animal_caption { get; set; }
        [DisplayName("開放認養時間(起)")]
        public string animal_opendate { get; set; }
        [DisplayName("開放認養時間(迄)")]
        public string animal_closeddate { get; set; }
        [DisplayName("動物資料異動時間")]
        public string animal_update { get; set; }
        [DisplayName("動物資料建立時間")]
        public string animal_createtime { get; set; }
        [DisplayName("動物所屬收容所名稱")]
        public string shelter_name { get; set; }
        [DisplayName("圖片名稱")]
        public string album_file { get; set; }
        [DisplayName("異動時間")]
        public string album_update { get; set; }
        [DisplayName("資料更新時間")]
        public string cDate { get; set; }
        [DisplayName("地址")]
        public string shelter_address { get; set; }
        [DisplayName("連絡電話")]
        public string shelter_tel { get; set; }

    }
}