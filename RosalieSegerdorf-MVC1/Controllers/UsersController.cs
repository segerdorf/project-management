using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using RosalieSegerdorf_MVC1.Data;
using RosalieSegerdorf_MVC1.Data.Models;
using RosalieSegerdorf_MVC1.ViewModels.User;

namespace RosalieSegerdorf_MVC1.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly DataContext _context;
        private ApplicationUserManager _userManager;

        public UsersController()
        {
            _context = new DataContext();
        }

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }
        
        public ActionResult Index()
        {
            var employees = new List<UsersViewModel>();

            var projects = _context.Projects.Include(p => p.ProjectLeader);

            employees = _context.Users
                .Include(u => u.Competences)
                .Include(u => u.Roles)
                .Select(x => new UsersViewModel
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    Phone = x.PhoneNumber,
                    Competences = x.Competences.ToList(),
                    ProjectLeaderFor = projects.Where(p => p.ProjectLeader.Id == x.Id).ToList()
                }).OrderBy(e => e.LastName).ToList();

            return View(employees);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create(CreateUserViewModel model)
        {
            if (UserManager.Users.Any(u => u.UserName == model.Username))
            {
                ModelState.AddModelError("usernameTaken", "Username is already taken.");
                return View();
            }

            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.Username,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Address = model.Address,
                    PhoneNumber = model.Phone,
                    StartOfEmployment = model.StartOfEmployment
                };

                var result = await UserManager.CreateAsync(user, model.Password);
                if (!result.Succeeded) return Content("Failed");

                await UserManager.AddToRoleAsync(user.Id, "Employee");

                if (model.IsAdmin)
                    await UserManager.AddToRoleAsync(user.Id, "Admin");

                if (model.IsProjectLeader)
                    await UserManager.AddToRoleAsync(user.Id, "Projectleader");

                TempData["Message"] = $"{user.FirstName} {user.LastName} was successfully added!" + " ";
                TempData["Type"] = "alert-success";

                return RedirectToAction("Index");
            }

            TempData["Message"] = "Someting went wrong, please try again";
            TempData["Type"] = "alert-danger";

            return View(model);
        }
        
        public async Task<ActionResult> Details(string id)
        {
            if (id != null)
            {
                if (User.Identity.GetUserId() == id || User.IsInRole("Admin"))
                {
                    var user = _context.Users
                        .Include(u => u.Competences)
                        .Include(u => u.Roles)
                        .Include(u => u.Projects)
                        .Include("Projects.Projectleader")
                        .Include("Projects.Employees")
                        .SingleOrDefault(u => u.Id == id);

                    var employee = new UserDetailsViewModel
                    {
                        Id = id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Address = user.Address,
                        StartOfEmployment = user.StartOfEmployment,
                        Email = user.Email,
                        Phone = user.PhoneNumber,
                        Competences = user.Competences.ToList(),
                        Projects = user.Projects.ToList()
                    };

                    var userRoles = await UserManager.GetRolesAsync(user.Id);

                    if (userRoles.Contains("Admin"))
                        employee.IsAdmin = true;
                    if (userRoles.Contains("ProjectLeader"))
                        employee.IsProjectLeader = true;

                    return View(employee);
                }
                TempData["Message"] = "You are not authorized to see this page!";
                TempData["Type"] = "alert-danger";
                return RedirectToAction("Index");
            }

            TempData["Message"] = $"Something went wrong, please try again";
            TempData["Type"] = "alert-danger";

            return RedirectToAction("Index");

        }
        
        [HttpPost]
        public async Task<ActionResult> Details(UserDetailsViewModel model)
        {
            var user = UserManager.Users
                .Include(u => u.Competences)
                .Include(u => u.Roles)
                .Include(u => u.Projects)
                .Include("Projects.Projectleader")
                .Include("Projects.Employees")
                .SingleOrDefault(u => u.Id == model.Id);

            if (ModelState.IsValid) { 
                if (User.Identity.GetUserId() == model.Id || User.IsInRole("Admin"))
                {
                    user.Email = model.Email;
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Address = model.Address;
                    user.PhoneNumber = model.Phone;
                    user.StartOfEmployment = model.StartOfEmployment;
                
                    await UserManager.UpdateAsync(user);

                    var roles = await UserManager.GetRolesAsync(user.Id);
                    await UserManager.RemoveFromRolesAsync(user.Id, roles.ToArray());
                    await UserManager.AddToRoleAsync(user.Id, "Employee");
                    if (model.IsAdmin)
                        await UserManager.AddToRoleAsync(user.Id, "Admin");
                    if (model.IsProjectLeader)
                        await UserManager.AddToRoleAsync(user.Id, "Projectleader");

                    HttpContext.GetOwinContext().Get<DataContext>().SaveChanges();

                    TempData["Message"] = $"{user.FirstName} {user.LastName} was successfully updated!";
                    TempData["Type"] = "alert-success";

                    return RedirectToAction("Details", new {user.Id});
                }
                TempData["Message"] = "You are not authorized for this action";
                TempData["Type"] = "alert-danger";

                return RedirectToAction("Index");
            }
            TempData["Message"] = "The profile was not updated!";
            TempData["Type"] = "alert-danger";

            model.Competences = user.Competences.ToList();
            model.Projects = user.Projects.ToList();

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(string id)
        {
            var user = UserManager.Users.Include(u => u.Competences).SingleOrDefault(u => u.Id == id);
            if (user != null)
            {
                var roles = await UserManager.GetRolesAsync(user.Id);
                await UserManager.RemoveFromRolesAsync(user.Id, roles.ToArray());
                // todo: nånting med competenses failar när såna finns, kanske iterera och ta bort dess först
                var result = await UserManager.DeleteAsync(user);

                TempData["Message"] = $"{user.FirstName} {user.LastName} was successfully deleted!";
                TempData["Type"] = "alert-success";

                if (result.Succeeded) return RedirectToAction("Index");
                
            }
            TempData["Message"] = $"Something went wrong! {user.FirstName} was not deleted.";
            TempData["Type"] = "alert-danger";

            return RedirectToAction("Index");
        }

        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ResetPassword(ResetPassword model, string id)
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user.Id == id)
            {
                if (ModelState.IsValid)
                {
                    var isPasswordOk = await UserManager.CheckPasswordAsync(user, model.CurrentPassword);
                    if (!isPasswordOk)
                    {
                        ModelState.AddModelError("incorrectPassword", "Input was not your current password");

                        TempData["Message"] = "Your current password was incorrect";
                        TempData["Type"] = "alert-danger";

                        return View();
                    }
                    await UserManager.ChangePasswordAsync(user.Id, model.CurrentPassword, model.Password);

                    TempData["Message"] = "Your password was successfully changed!";
                    TempData["Type"] = "alert-success";
                
                    return RedirectToAction("Details", "Users", new {id});
                }
                TempData["Message"] = "Something went wrong! Your password was not changed!";
                TempData["Type"] = "alert-danger";

                return View();
            }
            TempData["Message"] = "You are not authorized to reset the password";
            TempData["Type"] = "alert-danger";

            return RedirectToAction("Details", "Users", new {id});
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }


    }
}