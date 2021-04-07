using ShoppingListAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingListAPI.DTO
{
	public class PostShoppingList
	{
		public string Description { get; set; }
		public List<Item> Items { get; set; }
	}
}
