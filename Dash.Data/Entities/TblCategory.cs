using System;
using System.Collections.Generic;

#nullable disable

namespace anyhelp.Data.Entities
{
    public partial class TblCategory
    {
        public long CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool? CategoryIsdelete { get; set; }
    }
}
