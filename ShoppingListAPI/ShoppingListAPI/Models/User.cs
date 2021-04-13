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
		public string Username { get; set; }
		public string Password { get; set; }
		public List<ShoppingList> ShoppingList { get; set; }

		public User(string username, string password)
		{
			id++;

			Id = id;
			Username = username;
			Password = password;
			ShoppingList = new List<ShoppingList>();
		}
	}
}
