using Microsoft.AspNetCore.Mvc;
using ShoppingListAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShoppingListAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		public static List<string> Emails = new List<string>() {
			"example@gmail.com",
			"Secondexample@gmail.com"
		};

		public static List<User> Users = new List<User>() { 
			new User("example@gmail.com", "1234"),
			new User("Secondexample@gmail.com", "1234")
		};

		// GET: api/<ShoppingListController>
		[HttpGet]
		public ActionResult<IEnumerable<User>> Get()
		{
			return Users;
		}

		// POST api/<UsersController>
		[HttpPost("register/")]
		public ActionResult Register([FromBody] User user)
		{
			if (Emails.Contains(user.Email))
				return StatusCode((int)HttpStatusCode.Conflict, value: "User with this Email already exists.");
			else
				Emails.Add(user.Email);

			Users.Add(new User(user.Email, user.Pass));
			return Ok();
		}

		[HttpPost("login/")]
		public ActionResult Login([FromBody] User user)
		{
			User us = Users.Where(thisuser => thisuser.Email == user.Email && thisuser.Pass == user.Pass).FirstOrDefault();
			if (us != null)
			{
				if (Emails.Contains(user.Email))
					return Ok("Succesful");
				else
				{
					return StatusCode((int)HttpStatusCode.Conflict, value: "Invalid Email");
				}
			}

			return NotFound();
		}

		// PUT api/<UsersController>/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] string value)
		{
		}

		// DELETE api/<UsersController>/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}
	}
}
