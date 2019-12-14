using MyWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MyWebApp.Controllers.Api
{
    public class ReservationController : ApiController
    {
        [HttpPut]
        public IHttpActionResult DeleteResCheck(int id)
        {
            Lists.CurrentlyRes.RemoveAt(id);
            return Ok();
        }
    }
}
