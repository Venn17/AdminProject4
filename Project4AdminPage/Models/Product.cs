using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project4AdminPage.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int SalePrice { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public int Sold { get; set; }
        public int RestaurantID { get; set; }
        public int CategoryID { get; set; }
    }
}
