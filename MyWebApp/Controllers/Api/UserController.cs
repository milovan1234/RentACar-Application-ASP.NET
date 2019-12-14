using MyWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MyWebApp.Controllers.Api
{
    public class UserController : ApiController
    {
        private ModelContext _context;
        public UserController()
        {
            _context = new ModelContext();
        }
        [HttpDelete]
        public IHttpActionResult DeleteUser(int id)
        {
            var userInDb = _context.Users.FirstOrDefault(u => u.Id == id);
            if (userInDb == null)
            {
                return NotFound();
            }
            else
            {
                _context.Users.Remove(userInDb);
                _context.SaveChanges();
                return Ok();
            }

        }        
    }
}
