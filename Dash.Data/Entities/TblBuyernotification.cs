using System;
using System.Collections.Generic;

#nullable disable

namespace anyhelp.Data.Entities
{
    public partial class TblBuyernotification
    {
        public long BuyernotificationId { get; set; }
        public long? BuyernotificationParentid { get; set; }
        public long? BuyernotificationCategoryid { get; set; }
        public DateTime? BuyernotificationDatetime { get; set; }
        public bool? BuyernotificationIsselleraccepted { get; set; }
        public long? BuyernotificationBuyersellerid { get; set; }
        public bool? BuyernotificationIsdelete { get; set; }
        public bool? BuyernotificationIsread { get; set; }
        public string BuyernotificationMessage { get; set; }
    }
}
