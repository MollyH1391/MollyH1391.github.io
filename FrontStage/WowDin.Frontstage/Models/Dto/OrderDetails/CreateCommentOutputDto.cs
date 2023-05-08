using System;
using System.Collections.Generic;
using static WowDin.Frontstage.Common.ModelEnum.OrderEnum;

namespace WowDin.Frontstage.Models.Dto.OrderDetails
{
    public class CreateCommentOutputDto
    {

        public int CommentId { get; set; }
        public int UserAccountId { get; set; }
        public string Comment1 { get; set; }
        public int Star { get; set; }
        public DateTime Date { get; set; }
        public int BrandId { get; set; }
        public int ShopId { get; set; }
        public int OrderId { get; set; }
        //public CreateCommentOutputDto()
        //{
        //    Comment = new CommentData();
        //}
        //public bool IsSuccess { get; set; }
        //public string Message { get; set; } //?
        ////public int CommentId { get; set; }
        //public CommentData Comment { get; set; }



        //public class CommentData
        //{
        //    public int CommentId { get; set; }
        //    public int UserAccountId { get; set; }
        //    public string Comment1 { get; set; }
        //    public int Star { get; set; }
        //    public DateTime Date { get; set; }
        //    public int BrandId { get; set; }
        //    public int ShopId { get; set; }
        //    public int OrderId { get; set; }
        //}
    }
}
