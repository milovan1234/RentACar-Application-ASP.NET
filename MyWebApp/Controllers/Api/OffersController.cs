using MyWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MyWebApp.Controllers.Api
{
    public class OffersController : ApiController
    {
        private ModelContext _context;
        public OffersController()
        {
            _context = new ModelContext();
        }
        public IEnumerable<Offer> GetOffers()
        {
            return _context.Offers.ToList();
        }
        [HttpDelete]
        public void DeleteOffer(int id)
        {
            var offerInDb = _context.Offers.FirstOrDefault(o => o.Id == id);
            if (offerInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            _context.Offers.Remove(offerInDb);
            _context.SaveChanges();
        }
    }
}
