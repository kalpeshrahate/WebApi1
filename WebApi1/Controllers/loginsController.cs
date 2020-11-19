using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi1.Models;

namespace WebApi1.Controllers
{
    public class loginsController : ApiController
    {
        private Dbmodels db = new Dbmodels();

        // POST: api/logins
        [ResponseType(typeof(registration))]
        public IHttpActionResult Postregistration(login user)
        {
            registration Registration = db.registrations.Where(m => m.FirstName == user.Username && m.Password == user.Password).FirstOrDefault();
            return Ok(Registration);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}