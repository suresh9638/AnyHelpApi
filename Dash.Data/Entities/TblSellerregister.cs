using System;
using System.Collections.Generic;

#nullable disable

namespace anyhelp.Data.Entities
{
    public partial class TblSellerregister
    {
        public long SellerregisterId { get; set; }
        public string SellerregisterFullname { get; set; }
        public string SellerregisterPhoneno { get; set; }
        public string SellerregisterPassword { get; set; }
        public double? SellerregisterCreditamount { get; set; }
        public string SellerregisterLatitude { get; set; }
        public string SellerregisterLongitude { get; set; }
        public bool? SellerregisterIsdelete { get; set; }
    }
}
