using System;
using System.Collections.Generic;

#nullable disable

namespace anyhelp.Data.Entities
{
    public partial class TblSellercategory
    {
        public long SellercategoryId { get; set; }
        public long? SellercategorySellerid { get; set; }
        public long? SellercategoryCategoryid { get; set; }
        public bool? SellercategoryIsactive { get; set; }
    }
}
