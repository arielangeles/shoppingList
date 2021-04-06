using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingListAPI.Models
{
    public class Item
    {
        static int id = 0;

		public Item()
        {
            id++;
        }

		public Item(string name, int quantity, string status)
        {
            id++;

            Id = id;
            Name = name;
            Quantity = quantity;
            Status = status;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }

    }
}
