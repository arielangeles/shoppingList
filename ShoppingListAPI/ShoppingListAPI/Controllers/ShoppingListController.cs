using Microsoft.AspNetCore.Mvc;
using ShoppingListAPI.DTO;
using ShoppingListAPI.Models;
using System;
using System.Collections.Generic;
using ShoppingListAPI.Controllers;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShoppingListAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ShoppingListController : ControllerBase
	{
		public static List<ShoppingList> ShoppingLists = new List<ShoppingList>(); 

		// GET: api/<ShoppingListController>
		[HttpGet]
		public ActionResult<IEnumerable<ShoppingList>> Get()
		{
			return ShoppingLists;
		}

		// GET api/<ShoppingListController>/5
		[HttpGet("{id}")]
		public string Get(int id)
		{
			return "value";
		}

		// POST api/<ShoppingListController>
		[HttpPost]
		public ActionResult Post([FromBody] PostShoppingList list)
		{
			Item NewItem;
			List<Item> ItemsList = new List<Item>();
			foreach (var id in list.Items)
			{
				NewItem = ItemsController.Items.Where(item => item.Id == id).FirstOrDefault();
				ItemsList.Add(NewItem);
			}

			ShoppingList Shop = new ShoppingList(
				ItemsList,
				list.Description
				);
			ShoppingLists.Add(Shop);
			return Ok();
		}

		// PUT api/<ShoppingListController>/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] string value)
		{
		}

		// DELETE api/<ShoppingListController>/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}
	}
}
