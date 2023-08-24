using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project4AdminPage.Models
{
    public class History
    {
        public int Id { get; set; }
        public int UserID { get; set; }
        public int CartID { get; set; }
        public int Payment { get; set; }
        public int CouponID { get; set; }
        public int Status { get; set; }
    }
}
