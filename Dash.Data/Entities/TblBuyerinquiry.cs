using System;
using System.Collections.Generic;

#nullable disable

namespace anyhelp.Data.Entities
{
    public partial class TblBuyerinquiry
    {
        public long BuyerinquiryId { get; set; }
        public string BuyerinquiryFullname { get; set; }
        public string BuyerinquiryPhoneno { get; set; }
        public long? BuyerinquiryCategoryid { get; set; }
        public string BuyerinquiryLatitude { get; set; }
        public string BuyerinquiryLongitude { get; set; }
        public bool? BuyerinquiryIsdelete { get; set; }
    }
}
