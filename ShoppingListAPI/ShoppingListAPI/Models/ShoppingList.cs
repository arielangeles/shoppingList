using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingListAPI.Models
{
	public class ShoppingList
	{
		static int id = 0;

		public ShoppingList()
		{
			id++;
		}		

		public ShoppingList(List<Item> items, string description)
		{
			id++;
			Id = id;
			Items = items;
			Date = DateTime.Now;
			Description = description;
		}

		public int Id { get; set; }
		public List<Item> Items { get; set; }
		public DateTime Date { get; set; }
		public string Description { get; set; }
	}
}
