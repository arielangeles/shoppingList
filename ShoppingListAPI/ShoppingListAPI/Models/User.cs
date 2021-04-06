using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingListAPI.Models
{
	public class User
	{
		static int id = 0;

		public User()
		{
			id++;
		}

		public int Id { get; set; }
		public string Email { get; set; }
		public string Pass { get; set; }
		public List<ShoppingList> List { get; set; }

		public User(string email, string pass)
		{
			id++;

			Id = id;
			Email = email;
			Pass = pass;
			List = new List<ShoppingList>();
		}
	}
}
