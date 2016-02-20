using EducationSalvation.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EducationSalvation.Controllers
{
    public class HomeController : Controller
    {

        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public JsonResult GetPublicationThumbnails()
        {
            var collection = new List<PublicationThumbnailModel>();
            using (var db = new PublicationModelContext())
            {
                collection = db.PublicationModels.Select(p => new 
                {
                    Date = p.Date,
                    Description = p.Description,
                    Id = p.Id,
                    Stars = p.Stars,
                    Tags = p.TagModels.Select(t => t.Content),
                    Title = p.Title,
                    UserId = p.AdditionalUserInfoId
                }
                ).ToList().Select(obj => new PublicationThumbnailModel()
                {
                    Date = obj.Date,
                    Description = obj.Description,
                    Id = obj.Id,
                    Stars = obj.Stars,
                    Tags = obj.Tags.ToArray(),
                    Title = obj.Title,
                    UserId = obj.UserId
                }).ToList();
                return Json(collection, JsonRequestBehavior.AllowGet);
            }
        }
    }
}