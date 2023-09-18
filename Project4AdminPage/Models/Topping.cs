using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project4AdminPage.Models
{
    public class Topping
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProductID { get; set; }
        public int Price { get; set; }
        public bool Status { get; set; }
    }
}
