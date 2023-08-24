using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project4AdminPage.Models
{
    public class Coupon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Percent { get; set; }
        public bool Status { get; set; }
    }
}
