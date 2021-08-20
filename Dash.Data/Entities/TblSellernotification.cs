using System;
using System.Collections.Generic;

#nullable disable

namespace anyhelp.Data.Entities
{
    public partial class TblSellernotification
    {
        public long SellernotificationId { get; set; }
        public long? SellernotificationInqueryid { get; set; }
        public long? SellernotificationCategoryid { get; set; }
        public long? SellernotificationBuyernotificationid { get; set; }
        public long? SellernotificationSellerid { get; set; }
        public bool? SellernotificationIsdelete { get; set; }
        public bool? SellernotificationIsread { get; set; }
        public string SellernotificationMessage { get; set; }
    }
}
