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
    public class registrationsController : ApiController
    {
        private Dbmodels db = new Dbmodels();

        // GET: api/registrations
        public IQueryable<registration> Getregistrations()
        {
            return db.registrations;
        }

        // GET: api/registrations/5
        [ResponseType(typeof(registration))]
        public IHttpActionResult Getregistration(long id)
        {
            registration registration = db.registrations.Find(id);
            if (registration == null)
            {
                return NotFound();
            }

            return Ok(registration);
        }

        // PUT: api/registrations/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putregistration(long id, registration registration)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != registration.Id)
            {
                return BadRequest();
            }

            db.Entry(registration).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!registrationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/registrations
        [ResponseType(typeof(registration))]
        public IHttpActionResult Postregistration(registration registration)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.registrations.Add(registration);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = registration.Id }, registration);
        }

        // DELETE: api/registrations/5
        [ResponseType(typeof(registration))]
        public IHttpActionResult Deleteregistration(long id)
        {
            registration registration = db.registrations.Find(id);
            if (registration == null)
            {
                return NotFound();
            }

            db.registrations.Remove(registration);
            db.SaveChanges();

            return Ok(registration);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool registrationExists(long id)
        {
            return db.registrations.Count(e => e.Id == id) > 0;
        }
    }
}