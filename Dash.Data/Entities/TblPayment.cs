using System;
using System.Collections.Generic;

#nullable disable

namespace anyhelp.Data.Entities
{
    public partial class TblPayment
    {
        public long PaymentId { get; set; }
        public long? PaymentSellerid { get; set; }
        public string PaymentTransactiondescription { get; set; }
        public string PaymentTransactionid { get; set; }
        public DateTime? PaymentDatetime { get; set; }
        public double? PaymentAmount { get; set; }
        public bool? PaymentIspaid { get; set; }
        public bool? PaymentIsdelete { get; set; }
    }
}
