using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YAPET.Models
{
    public class VMBlog
    {
        public VMBlog()
        {
            this.posts = new List<Post>();
            this.posttypes = new List<PostType>();
            this.likes = new List<Like>();
            this.messages = new List<VMMessage>();
            this.photos = new List<Photo>();
            this.follows = new List<Follow>();
            this.blacks = new List<Black>();

            //this.reposts = new List<Report>();
            //this.msgereposts = new List<MsgReport>();
        }

        public List<Post> posts { get; set; }
        public List<VMMessage> messages { get; set; }
        public List<Photo> photos { get; set; }
        public List<PostType> posttypes { get; set; }
        public List<Like> likes { get; set; }
        public List<Follow> follows { get; set; }
        public List<Black> blacks { get; set; }

        //public List<Report> reposts { get; set; }
        //public List<MsgReport> msgereposts { get; set; }

        public int PageCount { set; get; }

    }

    public class VMMessage
    {
        public int MessageNo { set; get; }
        public int UserNo { set; get; }
        public DateTime Date { set; get; }
        public string Content { set; get; }
        public string uId { set; get; }
        public string uNickname { set; get; }
    }

    //Edit還沒寫
    public class VMPost
    {       
        //post
        public string Title { get; set; }
        public string Content { get; set; }
        public System.DateTime Date { get; set; }
        public string State { get; set; }
        public string PostTypeNo { get; set; }
        //photo
        public int PostNo { get; set; }
        public byte[] Photo1 { get; set; }
        public string ImageMimeType { get; set; }
        public int PhotoNo { get; set; }
    }

}