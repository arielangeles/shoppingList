using Microsoft.AspNetCore.Mvc;
using ShoppingListAPI.DTO;
using ShoppingListAPI.Models;
using ShoppingListAPI.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShoppingListAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{

		[HttpPost("Login")]
		public ActionResult Login([FromBody] UserAuthenticate user)
		{
			User authorizedUser = UserRepository.LoginUsers.FirstOrDefault(item => item.Username == user.Username);
			if (authorizedUser != null)
				return Ok("User already logged in");


			bool isPassword = SecurePasswordHasher.Verify(user.Password, user.Username);

			authorizedUser = UserRepository.RegisterUsers.FirstOrDefault(item => item.Username == user.Username);
			if (authorizedUser == null || !isPassword)
				return Unauthorized("Invalid Username or Password");

			UserRepository.LoginUsers.Add(authorizedUser);

			return Ok();
		}

		[HttpPost("Register")]
		public ActionResult Register([FromBody] UserAuthenticate user)
		{
			bool userExist = UserRepository.RegisterUsers.FirstOrDefault(item => item.Username == user.Username) != null;
			if (userExist)
				return	BadRequest("User already exist");

			string hashedPassword = SecurePasswordHasher.Hash(user.Password);

			User newUser = new User(user.Username, hashedPassword);
			UserRepository.RegisterUsers.Add(newUser);

			return Ok();
		}

		[BasicAuthentication]
		[HttpPost("Logout")]
		public ActionResult Logout()
		{
			string currentUserName = User.FindFirst(ClaimTypes.Name).Value;
			User user = UserRepository.LoginUsers.FirstOrDefault(item => item.Username == currentUserName);
			if (user == null)
				return Unauthorized("Invalid user");

			UserRepository.LoginUsers.Remove(user);

			return Ok();
		}

		//[HttpGet("Login")]
		//public ActionResult login()
		//{
		//	return Ok(UserRepository.LoginUsers);
		//}

		//[HttpGet("Register")]
		//public ActionResult Register()
		//{
		//	return Ok(UserRepository.RegisterUsers);
		//}
	}
}
