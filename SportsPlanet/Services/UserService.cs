using Microsoft.EntityFrameworkCore;
using SportsPlanet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsPlanet.Services
{
    public class UserService
    {
        private readonly DbService dbService;

        public UserService()
        {
            var context = new EadMidsContext();
            dbService = new DbService(context);
        }
        public int GetTotalUsers()
        {
            return dbService.GetTotalUsers();
        }
    }
}
