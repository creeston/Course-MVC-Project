using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EducationSalvation.Models;
using System.IO;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNet.Identity;
using MarkdownDeep;
using Microsoft.AspNet.SignalR;

namespace EducationSalvation.Controllers
{
    public class PublicationController : Controller
    {
        const string TemplatesPath = @"~/StaticResources/Templates";

        // GET: Publication
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Post(int id)
        {
            //PublicationModel model;
            //using (var db = new PublicationModelContext())
            //{
            //    model = db.PublicationModels.FirstOrDefault(m => m.Id == id.ToString());
            //}
            //return View(model.Template, new PubclicationShowingModel(model));
            ViewBag.PublicationIndex = id;
            return View();
        }

        public void LikeComment(int CommentId)
        {
            var currentUserId = User.Identity.GetUserId();
            var commentUser = "";
            using (var db = new PublicationModelContext())
            {
                db.LikeModels.Add(new LikeModel()
                {
                    AdditionalUserInfoId = currentUserId,
                    CommentModelId = CommentId
                });
                commentUser = db.CommentModels.FirstOrDefault(c => c.Id == CommentId).AdditionalUserInfoId;
                db.SaveChanges();
            }
            Helper.RecountUserMedals(commentUser);

        }


        [HttpPost]
        public void SendPublicationGrade(RatingSendingModel model)
        {
            var currentUserId = User.Identity.GetUserId();
            AdditionalUserInfo clientUser;
            using (var db = new PublicationModelContext())
            {
                db.RatingModels.Add(new RatingModel()
                {
                    PublicationModelId = model.PublicationId,
                    Value = model.Value,
                    AdditionalUserInfoId = currentUserId
                });
                clientUser = db.PublicationModels.FirstOrDefault(p => p.Id == model.PublicationId).User;
                db.SaveChanges();
            }

            //////////////////////////!!!!!!!!!!!!!
            //var hubContext = GlobalHost.ConnectionManager.GetHubContext<Hubs.CommentsHub>();
            //hubContext.Clients.User(clientUser.Id).display(string.Format("User {0} graduate you at {1} stars!", clientUser.Nickname, model.Value));
        }

        [HttpPost]
        public JsonResult GetPublicationGrade(int Index)
        {
            int grade = 0;
            bool result;
            var currentUserId = User.Identity.GetUserId();
            using (var db = new PublicationModelContext())
            {
                var collection = db.PublicationModels.First(p => p.Id == Index).RatingModels;
                grade = (int)collection.Average(r => r.Value);
                if (collection.FirstOrDefault(r => r.AdditionalUserInfoId == currentUserId) == null)
                    result = false;
                else result = true;
            }
            return Json(new { Result = result, Grade = grade});
        }

        [HttpPost]
        public JsonResult GetPublication(int Index)
        {
            PublicationModel model;
            PublicationShowingModel Model;
            var currentUserId = User.Identity.GetUserId();
            using (var db = new PublicationModelContext())
            {
                model = db.PublicationModels.FirstOrDefault(m => m.Id == Index);
                Model = new PublicationShowingModel()
                {
                    Id = model.Id,
                    Title = model.Title,
                    Description = model.Description,
                    Tags = model.TagModels.Select(m => m.Content).ToArray(),
                    Images = model.Images.Split(' '),
                    Videos = model.Videos.Split(' '),
                    Markdown = model.Markdown,
                    Template = model.Template,
                    Date = model.Date,
                    Stars = model.RatingModels.Count() > 0 ? (int)model.RatingModels.Average(r => r.Value) : 5,
                    UserId = model.AdditionalUserInfoId,
                    IsUserAlreadyGraduateIt = 
                        model.RatingModels.FirstOrDefault(r => r.AdditionalUserInfoId == currentUserId) == null ? false : true,
                    Comments = model.CommentModels.Select(c => new CommentShowingModel()
                    {
                        Id = c.Id,
                        Author = c.User.Nickname,
                        Content = c.Content,
                        Date = c.Date,
                        Rating = c.LikeModels.Count(),
                        IsUserAlreadyLikedIt =
                            c.LikeModels.FirstOrDefault(l => l.AdditionalUserInfoId == currentUserId) == null ? false : true
                    }).ToArray()
                };
            }

            return Json(Model);
        }



        [HttpGet]
        [System.Web.Mvc.Authorize]
        public ActionResult Create()
        {
            var model = new PublicationCreatingModel();
            return View(model);
        }

        public JsonResult GetTemplates()
        {
            DirectoryInfo d = new DirectoryInfo(Server.MapPath(TemplatesPath));
            List<Template> templates = new List<Template>();
            foreach (var file in d.GetFiles("*.html"))
            {
                templates.Add(new Template { Name = Helper.GetFileName(file.Name), Path = Helper.GetRelativePath(file.FullName, Request)});
            }
            return Json(templates, JsonRequestBehavior.AllowGet);
            
        }


        public JsonResult GetTags()
        {
            using (var db = new PublicationModelContext())
            {
                var collection = db.TagModels.Select(m => m.Content).ToArray();
                return Json(collection, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public ActionResult Publish(PublicationCreatingModel model)
        {
            string currentUserId = User.Identity.GetUserId();
            using (var db = new PublicationModelContext())
            {
                var Model = new PublicationModel()
                {
                    AdditionalUserInfoId = currentUserId,
                    Template = model.Template,
                    Title = model.Title,
                    Description = model.Description,
                    Markdown = model.Markdown ?? " ",
                    Images = model.Images ?? " ",
                    Videos = model.Videos ?? " ",
                    Date = model.Date,
                    Stars = 0
                };
                var tags = model.Tags.Split(' ');
                foreach (var tag in tags)
                {
                    var tagModel = db.TagModels.FirstOrDefault(m => m.Content == tag);
                    if (tagModel == null)
                    {
                        tagModel = new TagModel() { Content = tag, Rate = 1 };
                        db.TagModels.Add(tagModel);
                    }
                    else
                        tagModel.Rate++;
                    Model.TagModels.Add(tagModel);
                }

                db.PublicationModels.Add(Model);
                db.SaveChanges();
            }
            Helper.RecountUserMedals(currentUserId);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public void SendComment(CommentSendingModel model)
        {
            var currentUserId = User.Identity.GetUserId();
            using (var db = new PublicationModelContext())
            {
                var Model = new CommentModel()
                {
                    Content = model.Content,
                    Date = model.Date, 
                    Rating = 0, 
                    PublicationModelId = model.PublicationId,
                    AdditionalUserInfoId = currentUserId
                };

                db.CommentModels.Add(Model);
                db.SaveChanges();
            }
            Helper.RecountUserMedals(currentUserId);
        }

        [HttpPost]
        public JsonResult GetComments(int publicationId)
        {
            CommentShowingModel[] Comments;
            var currentUserId = User.Identity.GetUserId();
            using (var db = new PublicationModelContext())
            {
                var model = db.PublicationModels.FirstOrDefault(m => m.Id == publicationId);
                Comments = model.CommentModels.Select(c => new CommentShowingModel()
                {
                    Id = c.Id,
                    Author = c.User.Nickname,
                    Content = c.Content,
                    Date = c.Date,
                    Rating = c.LikeModels.Count(),
                    IsUserAlreadyLikedIt =
                                c.LikeModels.FirstOrDefault(l => l.AdditionalUserInfoId == currentUserId) == null ? false : true
                }).ToArray();
            }

            return Json(Comments);
        }

         
        public ActionResult SaveUploadedFile()
        {
            HttpPostedFileBase file = Request.Files["file"];
            int width = int.Parse(Request.Headers["width"]);
            int height = int.Parse(Request.Headers["height"]);
            int index = int.Parse(Request.Headers["index"]);
            string filePath = Helper.SaveTemporaryImage(file, Server.MapPath(@"~/Temp"));
            var result = CloudinaryAccountProvider.Upload(filePath, width, height);
            System.IO.File.Delete(filePath);
            return Json(new { Url = result.Uri, Index = index });
        }

        
    }

    public static class Helper
    {
        public static string GetRelativePath(string physicalPath, HttpRequestBase Request)
        {
            return "/" + physicalPath.Replace(Request.ServerVariables["APPL_PHYSICAL_PATH"], string.Empty);
        }
        public static string GetFileName(string fileName)
        {
            return Path.GetFileNameWithoutExtension(fileName);
        }

        public static string SaveTemporaryImage(HttpPostedFileBase file, string path)
        {
            bool isExists = System.IO.Directory.Exists(path);
            if (!isExists)
                System.IO.Directory.CreateDirectory(path);
            string filePhysicalPath = String.Format("{0}\\{1}", path, file.FileName);
            file.SaveAs(filePhysicalPath);
            return filePhysicalPath;
        }

        public static void RecountUserMedals(string userId)
        {
            if (userId == null) return;
            using (var db = new PublicationModelContext())
            {
                var user = db.AdditionalUserInfoes.FirstOrDefault(u => u.Id == userId);
                var userSummary = new AdditionalUserSummary()
                {
                    PublicationCount = user.PublicationModels.Count(),
                    CommentsCount = user.CommentModels.Count(),
                    CommentLikesCount = user.CommentModels.Sum(c => c.LikeModels.Count())
                };
                var medalsToUser = new List<int>();
                foreach (var medal in db.MedalModels)
                {
                    if (MedalCheckers.Id(medal.Id).CheckConditions(userSummary))
                        medalsToUser.Add(medal.Id);
                }
                foreach (var medalId in medalsToUser)
                {
                    var medalModel = db.MedalModels.FirstOrDefault(m => m.Id == medalId);
                    medalModel.AdditionalUserInfoes.Add(user);
                }
                db.SaveChanges();
            }
        }
    }

    public static class CloudinaryAccountProvider
    {
        private static Account account = new Account(
            "creeston",
            "966333823355385",
            "15TIDepx4NAg0m3nmTuU5EB4lOE");
        private static Cloudinary cloudinary = new Cloudinary(account);

        
        public static UploadResult Upload(string filePhysicalPath, int width, int height)
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(filePhysicalPath),
                Transformation = new Transformation().Width(width).Height(height).Crop("fit")
            };
            return cloudinary.Upload(uploadParams);
        }
    }

    public static class MedalCheckers
    {
        static List<IMedalChecker> Checkers = new List<IMedalChecker>() {
            new FirstPostMedal(), new TenPostsMedal(), new TenCommentsMedal(), new TenLikesMedal()
        };

        public static IMedalChecker Id(int id)
        {
            //Medal id starts with 2
            return Checkers[id - 2];
        }
    }

}