using Microsoft.AspNetCore.Mvc;
using ShoppingListAPI.DTO;
using ShoppingListAPI.Models;
using System;
using System.Collections.Generic;
using ShoppingListAPI.Controllers;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using ShoppingListAPI.Repository;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShoppingListAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ShoppingListController : ControllerBase
	{
		private List<ShoppingList> GetUserShoppingLists()
        {
			string currentUserName = User.FindFirst(ClaimTypes.Name).Value;
			return UserRepository.LoginUsers.FirstOrDefault(item => item.Username == currentUserName)?.ShoppingList;
		}

		[HttpPost("Login")]
		public ActionResult Login ([FromBody] UserAuthenticate user)
        {
			User authorizedUser = UserRepository.LoginUsers.FirstOrDefault(item => item.Username == user.Username);
			if (authorizedUser != null)
				 return NotFound("User already logged");

			authorizedUser = UserRepository.RegisterUsers.FirstOrDefault(item => item.Username == user.Username);
			if (authorizedUser == null)
				return NotFound("Invalid Username or Password");

			UserRepository.LoginUsers.Add(authorizedUser);

			return Ok();
		}

		[HttpPost("Register")]
		public ActionResult Register([FromBody] UserAuthenticate user)
		{
			bool userExist = UserRepository.RegisterUsers.FirstOrDefault(item => item.Username == user.Username) != null;
			if (userExist)
				return NotFound("User already exist");

			User newUser = new User(user.Username, user.Password);
			UserRepository.RegisterUsers.Add(newUser);

			return Ok();
		}

		[BasicAuthentication]
		[HttpPost("Logout")]
		public ActionResult Logout([FromBody] User logoutUser)
		{
			string currentUserName = User.FindFirst(ClaimTypes.Name).Value;
			User user = UserRepository.LoginUsers.FirstOrDefault(item => item.Username == currentUserName);
			if (user == null)
				return NotFound("error");
		
			UserRepository.LoginUsers.Remove(user);
			return Ok();
		}

		[HttpGet("Login")]
		public ActionResult login()
		{
			return Ok(UserRepository.LoginUsers);
		}

		[HttpGet("Register")]
		public ActionResult Register()
		{
			return Ok(UserRepository.RegisterUsers);
		}


		// GET: api/<ShoppingListController>
		[BasicAuthentication]
		[HttpGet]
		public ActionResult<IEnumerable<ShoppingList>> Get()
		{
			return GetUserShoppingLists();
		}

		// GET api/<ShoppingListController>/5
		[BasicAuthentication]
		[HttpGet("{id}")]
		public string Get(int id)
		{
			return "value";
		}

		//POST api/<ShoppingListController>
		[BasicAuthentication]
		[HttpPost]
        public ActionResult Post([FromBody] PostShoppingList list)
        {
			List<ShoppingList> ShoppingLists = GetUserShoppingLists();

			List<Item> ItemsList = new List<Item>();

            ShoppingList Shop = new ShoppingList(
                ItemsList,
                list.Description
                );
            ShoppingLists.Add(Shop);

            return Ok();
        }

		// PUT api/<ShoppingListController>/5
		[BasicAuthentication]
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] string value)
		{
		}

		// DELETE api/<ShoppingListController>/5
		[BasicAuthentication]
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}

		// GET api/<ShoppingListController>/1/Items
		[BasicAuthentication]
		[HttpGet("{shoppingdId}/Items")]
		public ActionResult<IEnumerable<Item>> ItemsGet(int shoppingdId,
			[FromQuery(Name = "status")] string status = null, [FromQuery(Name = "name")] string name = null)
		{
			List<ShoppingList> ShoppingLists = GetUserShoppingLists();

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
		[BasicAuthentication]
		[HttpGet("{shoppingdId}/Items/{name}")]
		public ActionResult<Item> ItemGet(int shoppingdId, string name)
		{
			List<ShoppingList> ShoppingLists = GetUserShoppingLists();

			ShoppingList list = ShoppingLists.Find(e => e.Id == shoppingdId);

			if (list == null) return NotFound("ShoppingList not found");

			List<Item> items = list.Items;

			Item item = items.Where(item => item.Name == name).FirstOrDefault();
			return item;
		}

		// POST api/<ShoppingListController>/1/Items
		[BasicAuthentication]
		[HttpPost("{shoppingdId}/Items")]
		public ActionResult ItemPost(int shoppingdId, [FromBody] PostItem item)
		{
			List<ShoppingList> ShoppingLists = GetUserShoppingLists();

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
		[BasicAuthentication]
		[HttpPut("{shoppingdId}/Items/{name}")]
        public ActionResult ItemPut(int shoppingdId, string name, [FromBody] PutStatus status)
        {
			List<ShoppingList> ShoppingLists = GetUserShoppingLists();

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
		[BasicAuthentication]
		[HttpDelete("{shoppingdId}/Items/{name}")]
        public ActionResult ItemDelete(int shoppingdId, string name)
        {
			List<ShoppingList> ShoppingLists = GetUserShoppingLists();

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
