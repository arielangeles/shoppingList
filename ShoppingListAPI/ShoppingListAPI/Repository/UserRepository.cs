using ShoppingListAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingListAPI.Repository
{
    public static class UserRepository
    {
        public static List<User> RegisterUsers = new List<User>();

        public static List<User> LoginUsers = new List<User>();
    }
}
