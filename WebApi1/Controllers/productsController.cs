using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi1.Models;

namespace WebApi1.Controllers
{
    public class productsController : ApiController
    {
        private Dbmodels db = new Dbmodels();

        // GET: api/products45
        public IQueryable<product> Getproducts()
        {
            return db.products;
        }

        // GET: api/products/5
        [ResponseType(typeof(product))]
        public IHttpActionResult Getproduct(long id)
        {
            product product = db.products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // PUT: api/products/5
        /*[ResponseType(typeof(void))]
        public IHttpActionResult Putproduct(long id, product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.Id)
            {
                return BadRequest();
            }

            db.Entry(product).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!productExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }*/

        // POST: api/products
        /*[ResponseType(typeof(product))]
        public IHttpActionResult Postproduct(product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.products.Add(product);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = product.Id }, product);
        }*/

        //Image Upload 
        [HttpPost]
        [Route("api/UploadImage")]
        public HttpResponseMessage UploadImage()
        {
            string imageName = null;
            var httpRequest = HttpContext.Current.Request;
            var postedFile = httpRequest.Files["Image"];
            string postid = httpRequest["Id"];

            if(postedFile != null)
            {
                imageName = new String(Path.GetFileNameWithoutExtension(postedFile.FileName).Take(10).ToArray()).Replace(" ", "-");
                imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(postedFile.FileName);
                var filePath = HttpContext.Current.Server.MapPath("~/Image/" + imageName);
                postedFile.SaveAs(filePath);
            }
            else if (postedFile == null && postid != null)
            {
                imageName = db.products.Find(Convert.ToInt64(httpRequest["Id"])).Image;
            }

            //save to db
            using (Dbmodels db = new Dbmodels())
            {
                product Product = new product()
                {
                    Id = Convert.ToInt64(httpRequest["Id"]),
                    Title = httpRequest["Title"],
                    Image = imageName,
                    Features = httpRequest["Features"],
                    Price = httpRequest["Price"],
                    EnteredDate = DateTime.Now,
                    IsActive = 1,
                };

                if (postid == null || postid == "0")
                {
                    db.products.Add(Product);
                }
                else
                {
                    db.Entry(Product).State = EntityState.Modified;
                }
                db.SaveChanges();
            }
            return Request.CreateResponse(HttpStatusCode.Created);
        }

        // DELETE: api/products/5
        [ResponseType(typeof(product))]
        public IHttpActionResult Deleteproduct(long id)
        {
            product product = db.products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            db.products.Remove(product);
            db.SaveChanges();

            return Ok(product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool productExists(long id)
        {
            return db.products.Count(e => e.Id == id) > 0;
        }
    }
}