using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using ShoppingListAPI.DTO;
using ShoppingListAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShoppingListAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        public static List<string> Names = new List<string>()
        {
            "Leche", "Pan"
        };

        public static List<Item> Items = new List<Item>()
        { 
            new Item("Leche", 5, "Completado"),
            new Item("Pan", 10, "Pendiente Compra"),
        };

        // GET: api/<ItemsController>
        [HttpGet]
        public ActionResult<IEnumerable<Item>> Get(
            [FromQuery(Name = "status")] string status = null, [FromQuery(Name = "name")] string name = null)
        {
            if (Items.Count == 0) return NotFound("List of items is empty");

            if (status != null)
            {
                return Items.Where(item => item.Status == status).ToList();
            }
            else if (name != null)
            {
                return Items.Where(item => item.Name == name).ToList();
            }
            
            return Items;
        }

        // GET api/<ItemsController>/5
        [HttpGet("{id}")]
        public Item Get(int id)
        {
            Item item = Items.Where(item => item.Id == id).FirstOrDefault();
            return item;
        }


        // POST api/<ItemsController>
        [HttpPost]
        public ActionResult Post([FromBody] PostItem item)
        {
            // Check if name already exists
            if (Names.Contains(item.Name)) 
                return StatusCode((int)HttpStatusCode.Conflict, value: "Item with this name already exists.");
            else
                Names.Add(item.Name);
            

            Items.Add(new Item(item.Name, item.Quantity, item.Status));

            return Ok();
        }

        public class PutStatus
        {
            public string Status { get; set; }
        }

        // PUT api/<ItemsController>/5
        [HttpPut("{name}")]
        public ActionResult Put(string name, [FromBody] PutStatus status)
        {
            
            //var etag = ETagGenerator.GetETag(Request.Path.ToString(), Encoding.UTF8.GetBytes(name));

            //if (Request.Headers.ContainsKey("If-None-Match") && Request.Headers["If-None-Match"].ToString() == etag)
            //{
            //    return StatusCode((int)HttpStatusCode.NotModified);
            //}

            Item item = Items.Where(item => item.Name == name).FirstOrDefault();
            item.Status = status.Status;

            return Ok();
 
        }
   
        // DELETE api/<ItemsController>/5
        [HttpDelete("{name}")]
        public ActionResult Delete(string name)
        {
            Item item = Items.Where(item => item.Name == name).FirstOrDefault();
            if (item == null)
                return NotFound("Item with this name was not found.");

            Names.Remove(item.Name);
            Items.Remove(item);

            return Ok();
            
        }
    }
}
