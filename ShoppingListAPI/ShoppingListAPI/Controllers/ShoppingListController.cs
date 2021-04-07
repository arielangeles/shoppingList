using Microsoft.AspNetCore.Mvc;
using ShoppingListAPI.DTO;
using ShoppingListAPI.Models;
using System;
using System.Collections.Generic;
using ShoppingListAPI.Controllers;
using System.Linq;
using System.Threading.Tasks;
using System.Net;

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

        //POST api/<ShoppingListController>
		[HttpPost]
        public ActionResult Post([FromBody] PostShoppingList list)
        {
            List<Item> ItemsList = new List<Item>();

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

		// GET api/<ShoppingListController>/1/Items
		[HttpGet("{shoppingdId}/Items")]
		public ActionResult<IEnumerable<Item>> ItemsGet(int shoppingdId,
			[FromQuery(Name = "status")] string status = null, [FromQuery(Name = "name")] string name = null)
		{
			ShoppingList list = ShoppingLists.Find(e => e.Id == shoppingdId);

			if (list == null) return NotFound("ShoppingList not found");

			List<Item> items = list.Items;


			if (items.Count == 0) return NotFound("List of items is empty");

			if (status != null)
			{
				return items.Where(item => item.Status == status).ToList();
			}
			else if (name != null)
			{
				return items.Where(item => item.Name == name).ToList();
			}

			return items;
		}

		// GET api/<ShoppingListController>/1/Items/pan
		[HttpGet("{shoppingdId}/Items/{name}")]
		public ActionResult<Item> ItemGet(int shoppingdId, string name)
		{
			ShoppingList list = ShoppingLists.Find(e => e.Id == shoppingdId);

			if (list == null) return NotFound("ShoppingList not found");

			List<Item> items = list.Items;

			Item item = items.Where(item => item.Name == name).FirstOrDefault();
			return item;
		}

		// POST api/<ShoppingListController>/1/Items
		[HttpPost("{shoppingdId}/Items")]
		public ActionResult ItemPost(int shoppingdId, [FromBody] PostItem item)
		{
			ShoppingList list = ShoppingLists.Find(e => e.Id == shoppingdId);

			if (list == null) return NotFound("ShoppingList not found");

			List<Item> items = list.Items;

			// Check if name already exists
			if (items.Exists(e => e.Name == item.Name))
				return StatusCode((int)HttpStatusCode.Conflict, value: "Item with this name already exists.");



			list.Items.Add(new Item(item.Name, item.Quantity, item.Status));

			return Ok();
		}

		public class PutStatus
		{
			public string Status { get; set; }
		}

        // PUT api/<ShoppingListController>/1/Items/pan
        [HttpPut("{shoppingdId}/Items/{name}")]
        public ActionResult ItemPut(int shoppingdId, string name, [FromBody] PutStatus status)
        {
            ShoppingList list = ShoppingLists.Find(e => e.Id == shoppingdId);

            if (list == null) return NotFound("ShoppingList not found");

            List<Item> items = list.Items;
            //var etag = ETagGenerator.GetETag(Request.Path.ToString(), Encoding.UTF8.GetBytes(name));

            //if (Request.Headers.ContainsKey("If-None-Match") && Request.Headers["If-None-Match"].ToString() == etag)
            //{
            //    return StatusCode((int)HttpStatusCode.NotModified);
            //}

            Item item = items.Where(item => item.Name == name).FirstOrDefault();
            item.Status = status.Status;

            return Ok();

        }

		// DELETE api/<ShoppingListController>/1/Items/pan
		[HttpDelete("{shoppingdId}/Items/{name}")]
        public ActionResult ItemDelete(int shoppingdId, string name)
        {
            ShoppingList list = ShoppingLists.Find(e => e.Id == shoppingdId);

            if (list == null) return NotFound("ShoppingList not found");

            List<Item> items = list.Items;

            Item item = items.Where(item => item.Name == name).FirstOrDefault();
            if (item == null)
                return NotFound("Item with this name was not found.");

            items.Remove(item);

            return Ok();

        }
    }
}
