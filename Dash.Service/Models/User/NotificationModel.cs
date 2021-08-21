using anyhelp.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;
using System.Text;

namespace anyhelp.Service.Models
{
    
        public  class BuyernotificationModel
        {
        public long Index { get; set; }
        public long BuyernotificationId { get; set; }
            public long? BuyernotificationParentid { get; set; }
            public long? BuyernotificationCategoryid { get; set; }
            public DateTime? BuyernotificationDatetime { get; set; }
            public bool? BuyernotificationIsselleraccepted { get; set; }
            public long? BuyernotificationBuyersellerid { get; set; }
            public bool? BuyernotificationIsdelete { get; set; }
            public bool? BuyernotificationIsread { get; set; }
            public string BuyernotificationMessage { get; set; }
            public string BuyernotificationCategoryName { get; set; }
            public string SellerName { get; set; }
            public string SellerPhone { get; set; }
      }



    public  class SellernotificationModel
    {
        public long SellernotificationId { get; set; }
        public long? SellernotificationInqueryid { get; set; }
        public long? SellernotificationCategoryid { get; set; }
        public long? SellernotificationBuyernotificationid { get; set; }
        public long? SellernotificationSellerid { get; set; }
        public bool? SellernotificationIsdelete { get; set; }
        public bool? SellernotificationIsread { get; set; }
        public string SellernotificationMessage { get; set; }
        public bool? SellernotificationIsaccept { get; set; }
        public string SellernotificationCategoryName { get; set; }
        [NotMapped]
        public TblBuyerinquiry Buyerinquiry { get; set; }

    }




    public class NotificationModel
    {
        public bool IsSeller { get; set; }
        public List<BuyernotificationModel> BuyernotificationModelList { get; set; }
        public List<SellernotificationModel> SellernotificationModelList { get; set; }

        public TblSellerregister tblSellerregister { get; set; }
    }

}
