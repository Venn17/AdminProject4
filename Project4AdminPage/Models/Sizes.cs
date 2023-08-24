using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project4AdminPage.Models
{
    public class Sizes
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProductID { get; set; }
        public Product Product { get; set; }
    }
}
