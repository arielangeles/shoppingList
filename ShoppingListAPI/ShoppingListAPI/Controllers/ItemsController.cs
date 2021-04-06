using Microsoft.AspNetCore.Mvc;
using ShoppingListAPI.DTO;
using ShoppingListAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShoppingListAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private static List<Item> Items = new List<Item>()
        { 
            new Item("Leche", 5, "Completado"),
            new Item("Pan", 10, "Pendiente Compra"),
        };

        // GET: api/<ItemsController>
        [HttpGet]
        public IEnumerable<Item> Get(
            [FromQuery(Name = "status")] string status = null, [FromQuery(Name = "name")] string name = null)
        {
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
        public OkResult Post([FromBody] PostItem item)
        {
            Items.Add(new Item(item.Name, item.Quantity, item.Status));

            return Ok();
        }

        // PUT api/<ItemsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ItemsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
