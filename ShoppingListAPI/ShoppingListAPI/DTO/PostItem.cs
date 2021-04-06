using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingListAPI.DTO
{
    public class PostItem
    {
        public PostItem() { }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
    }
}
