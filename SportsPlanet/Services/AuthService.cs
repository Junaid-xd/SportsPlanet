using SportsPlanet.Helpers;
using SportsPlanet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsPlanet.Services
{
    internal class AuthService
    {
        private readonly DbService dbService;
        public static bool isLoggedIn = false;
        public static User ?loggedInUser = null;

        public AuthService()
        {
            var context = new EadMidsContext();   
            dbService = new DbService(context);  
        }

        public bool AddUser(User user)
        {
            return dbService.AddNewUser(user);
        }

        public bool login(string email, string password)
        {
            User? user = dbService.FindByEmail(email);

            if (user == null)
            {
                return false;
            }

            bool isValid = Security.VerifyPassword(password, user.Password);

            if (!isValid)
            {
                return false;
            }
            isLoggedIn = true;
            loggedInUser = user;
            return true; 
        }

        public static void logout() 
        {
            isLoggedIn = false;
            loggedInUser=null;
        }


    }
}
