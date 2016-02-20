using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using EducationSalvation.Models;
using System.Web.Security;
using System.Collections.Generic;
using System.Net;

namespace EducationSalvation.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        [HttpGet]
        public ActionResult Profiles(string Id)
        {
            ShowingAdditionalUserInfo Model;
            using (var db = new PublicationModelContext())
            {
                var model = db.AdditionalUserInfoes.FirstOrDefault(u => u.Nickname == Id);
                Model = new ShowingAdditionalUserInfo()
                {
                    Age = model.Age,
                    Nickname = model.Nickname,
                    FirstName = model.FirstName,
                    Gender = model.Gender,
                    Interests = model.Interests,
                    LastName = model.LastName,
                    Location = model.Location,
                    Publications = model.PublicationModels.Select(p => new {
                        Date = p.Date, Description = p.Description, Id = p.Id, Stars = p.Stars, Tags = p.TagModels.Select(t => t.Content), Title = p.Title, UserNickname = p.User.Nickname
                    }).Select(obj => new PublicationThumbnailModel()
                    {
                        Date = obj.Date, Description = obj.Description, Id = obj.Id, Stars = obj.Stars, Tags = obj.Tags.ToArray(), Title = obj.Title, UserNickname = obj.UserNickname
                    }).ToArray()
                };
            }
            return View(Model);
        }

        [HttpGet]
        public ActionResult CreateAdditionalUserInfo()
        {
            var userId = User.Identity.GetUserId();
            CreatableAdditionalUserInfo Model;
            using (var db = new PublicationModelContext())
            {
                Guid g = Guid.NewGuid();
                string GuidString = Convert.ToBase64String(g.ToByteArray());
                GuidString = GuidString.Replace("=", "");
                GuidString = GuidString.Replace("+", "");

                var model = new AdditionalUserInfo()
                {
                    Id = userId,
                    Nickname = GuidString,
                    Age = 0,
                    FirstName = "",
                    LastName = "",
                    Gender = "",
                    Interests = "",
                    Location = ""
                };
                db.AdditionalUserInfoes.Add(model);
                db.NicknameModels.Add(new NicknameModel() { Nickname = GuidString, UserId = userId });
                Model = new CreatableAdditionalUserInfo() { Nicknames = db.NicknameModels.Select(n => n.Nickname).ToArray() };
                db.SaveChanges();
            }
            return View(Model);
        }
        [HttpPost]
        public ActionResult CreateAdditionalUserInfo(CreatableAdditionalUserInfo model)
        { 
            var userId = User.Identity.GetUserId();
            using (var db = new PublicationModelContext())
            {
                var UserModel = db.AdditionalUserInfoes.FirstOrDefault(u => u.Id == userId);
                var NicknameModel = db.NicknameModels.FirstOrDefault(n => n.UserId == userId);
                UserModel.Age = model.Age;
                UserModel.FirstName = model.FirstName ?? "";
                UserModel.Gender = model.Gender ?? "";
                UserModel.Interests = model.Interests ?? "";
                UserModel.LastName = model.LastName ?? "";
                UserModel.Location = model.Location ?? "";
                UserModel.Nickname = model.Nickname ?? "";
                NicknameModel.Nickname = model.Nickname ?? NicknameModel.Nickname;
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Manage");
        }


        [Authorize]
        [HttpGet]
        public ActionResult EditAdditionalInfo()
        {
            var userId = User.Identity.GetUserId();
            using (var db = new PublicationModelContext())
            {
                var model = db.AdditionalUserInfoes.FirstOrDefault(m => m.Id == userId);
                var Model = new EditableAdditionalUserInfo()
                {
                    Age = model.Age,
                    FirstName = model.FirstName,
                    Gender = model.Gender,
                    Interests = model.Interests,
                    LastName = model.LastName,
                    Location = model.Location
                };

                return View(Model);
            }
        }
        [HttpPost]
        public ActionResult EditAdditionalInfo(EditableAdditionalUserInfo model)
        {
            AdditionalUserInfo Model;
            var id = User.Identity.GetUserId();
            using (var db = new PublicationModelContext())
            {
                Model = db.AdditionalUserInfoes.SingleOrDefault(p => p.Id == id);
                Model.Age = model.Age;
                Model.FirstName = model.FirstName ?? "";
                Model.LastName = model.LastName ?? "";
                Model.Location = model.Location ?? "";
                Model.Gender = model.Gender ?? "";
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Manage");
        }

        public JsonResult DoesUserNicknameExist(string Nickname)
        {
            bool result;
            using (var db = new PublicationModelContext())
            {
                if (db.NicknameModels.FirstOrDefault(n => n.Nickname == Nickname) == null)
                    result = false;
                else result = true;
            }
            return Json(result);
        }




        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [Authorize(Roles = "admin")]
        public ActionResult GetAllUsers()
        {
            List<ApplicationUser> model;
            using (var db = new ApplicationDbContext())
            {
                model = db.Users.ToList();
            }
            return View(model);
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public ActionResult Create()
        {
            var model = new CreatingViewModel();
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Create(CreatingViewModel model)
        {
            if (ModelState.IsValid) {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = UserManager.Create(user, model.Password);
                if (result.Succeeded)
                {
                    //await UserManager.AddToRoleAsync(user.Id, "user");
                    UserManager.AddToRole(user.Id, model.Role);
                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("GetAllUsers", "Manage");
                }
                AddErrors(result);
            }
            return View();
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public ActionResult Edit(string id = "")
        {
            ApplicationUser user;
            using (var db = new ApplicationDbContext())
            {
                user = db.Users.SingleOrDefault(p => p.Id == id);
            }
            if (user == null)
            {
                return HttpNotFound();
            }
            var model = new EditableViewModel(user);
            return View(model);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult Edit(EditableViewModel model)
        {
            ApplicationUser user;
            var id = model.Id;
            using (var db = new ApplicationDbContext())
            {
                user = db.Users.SingleOrDefault(p => p.Id == id);
                user.LockoutEnabled = model.LockoutEnabled;
                user.LockoutEndDateUtc = model.LockoutEndDateUtc;
                if (model.Password != null)
                    user.PasswordHash = UserManager.PasswordHasher.HashPassword(model.Password);
                user.PhoneNumber = model.PhoneNumber;
                user.PhoneNumberConfirmed = model.PhoneNumberConfirmed;
                user.Email = model.Email;
                user.EmailConfirmed = model.EmailConfirmed;
                user.UserName = model.UserName;

                UserManager.RemoveFromRoles(id, "user", "admin");
                UserManager.AddToRole(id, model.Role);

                db.SaveChanges();
            }
            return RedirectToAction("GetAllUsers", "Manage");
        }

        [Authorize(Roles = "admin")]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = UserManager.FindById(id);
            var logins = user.Logins;

            foreach (var login in logins.ToList())
            {
                UserManager.RemoveLogin(login.UserId, new UserLoginInfo(login.LoginProvider, login.ProviderKey));
            }

            var rolesForUser = UserManager.GetRoles(id);

            foreach (var item in rolesForUser.ToList())
            {
                UserManager.RemoveFromRole(user.Id, item);
            }

            UserManager.Delete(user);

            return RedirectToAction("GetAllUsers", "Manage");
        }

        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : "";

            var userId = User.Identity.GetUserId();
            //User can be guest
            var userRole = UserManager.GetRolesAsync(userId).Result[0];

            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
            };
            using (var db = new PublicationModelContext())
            {
                var Model = db.AdditionalUserInfoes.FirstOrDefault(m => m.Id == userId);
                ViewBag.AdditionalUserInfo = new ShowingAdditionalUserInfo()
                {
                    Age = Model.Age,
                    FirstName = Model.FirstName,
                    Gender = Model.Gender,
                    Interests = Model.Interests,
                    LastName = Model.LastName,
                    Location = Model.Location,
                    Publications = Model.PublicationModels.Select(p => new
                    {
                        Id = p.Id,
                        UserNickname = p.User.Nickname,
                        Date = p.Date,
                        Description = p.Description,
                        Stars = p.Stars,
                        Tags = p.TagModels.Select(t => t.Content),
                        Title = p.Title

                    }).ToArray().Select(obj => new PublicationThumbnailModel()
                    {
                        Id = obj.Id,
                        UserNickname = obj.UserNickname,
                        Date = obj.Date,
                        Description = obj.Description,
                        Stars = obj.Stars,
                        Tags = obj.Tags.ToArray(),
                        Title = obj.Title
                    }).ToArray()
                };
            }
            return View(userRole + "Index", model);
        }

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generate the token and send it
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Your security code is: " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
            // Send an SMS through the SMS provider to verify the phone number
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Failed to verify phone");
            return View(model);
        }

        //
        // GET: /Manage/RemovePhoneNumber
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

#region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

#endregion
    }
}