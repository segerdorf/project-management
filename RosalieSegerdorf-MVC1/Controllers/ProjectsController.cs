using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using RosalieSegerdorf_MVC1.Data;
using RosalieSegerdorf_MVC1.Data.Models;
using RosalieSegerdorf_MVC1.ViewModels.Project;

namespace RosalieSegerdorf_MVC1.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private readonly DataContext _context;
        private ApplicationUserManager _userManager;

        public ProjectsController()
        {
            _context = new DataContext();
        }
        
        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AllProjects()
        {
            var projects = new List<ProjectsViewModel>();

            projects = _context.Projects
                .Select(p => new ProjectsViewModel
                {
                    Id = p.Id,
                    Title = p.Title,
                    Platform = p.Platform,
                    ProjectLeader = p.ProjectLeader.FirstName + " " + p.ProjectLeader.LastName,
                    AmountOfEmployees = p.Employees.Count(),
                    ReleaseDate = p.ReleaseDate,
                    IsActive = p.IsActive
                }).OrderBy(p => p.ReleaseDate).ToList();

            return View(projects);
        }

        [Authorize(Roles = "ProjectLeader")]
        public ActionResult MyProjects()
        {
            var projects = new List<ProjectsViewModel>();
            var id = User.Identity.GetUserId();
            projects = _context.Projects.Include(u => u.ProjectLeader).Where(p => p.ProjectLeader.Id == id)
                .Select(p => new ProjectsViewModel
                {
                    Id = p.Id,
                    Title = p.Title,
                    Platform = p.Platform,
                    ProjectLeader = p.ProjectLeader.FirstName + " " + p.ProjectLeader.LastName,
                    AmountOfEmployees = p.Employees.Count(),
                    ReleaseDate = p.ReleaseDate,
                    IsActive = p.IsActive
                }).OrderBy(p => p.ReleaseDate).ToList();

            return View(projects);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            CreateDetailViewBags(new Project());

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(CreateProjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                var projectLeader = _context.Users.SingleOrDefault(u => u.Id == model.ProjectLeaderId) ?? new User();

                List<User> employees = new List<User>();
                foreach (var e in model.EmployeeIds)
                {
                    employees.Add(_context.Users.SingleOrDefault(u => u.Id == e));
                }

                var project = new Project
                {
                    Title = model.Title,
                    ProjectLeader = projectLeader,
                    Platform = model.Platform,
                    Description = model.Description,
                    Employees = employees,
                    StartDate = model.StartDate,
                    ReleaseDate = model.ReleaseDate,
                    IsActive = model.IsActive
                };

                _context.Projects.Add(project);
                _context.SaveChanges();

                TempData["Message"] = $"{project.Title} was successfully added!";
                TempData["Type"] = "alert-success";

                return RedirectToAction("Details", new {id = project.Id});
            }

            CreateDetailViewBags(new Project());

            TempData["Message"] = "Something went wrong and your project was not saved!";
            TempData["Type"] = "alert-danger";

            return View(model);
        }
        
        public ActionResult Details(int? id)
        {
            var userId = User.Identity.GetUserId();
            if (id == null)
            {
                return RedirectToAction("Details", "Users", new { id = userId });
            }

            try
            {
                var project = _context.Projects
                                  .Include("Employees")
                                  .Include("ProjectLeader")
                                  .Include("Employees.Competences")
                                  .Include("Employees.Roles")
                                  .SingleOrDefault(p => p.Id == id) ?? new Project();

                if (project.Employees.Any(pe => pe.Id == userId)
                    || project.ProjectLeader.Id == userId
                    || User.IsInRole("Admin"))
                {
                    var model = new ProjectDetailsViewModel
                    {
                        Id = project.Id,
                        Title = project.Title,
                        Platform = project.Platform,
                        Description = project.Description,
                        ProjectLeader = $"{project.ProjectLeader.FirstName} {project.ProjectLeader.LastName}",
                        Employees = project.Employees ?? new List<User>(),
                        StartDate = project.StartDate,
                        ReleaseDate = project.ReleaseDate,
                        IsActive = project.IsActive
                    };
                    CreateDetailViewBags(project);

                    return View(model);
                }

                TempData["Message"] = $"You are not authorized to view {project.Title}";
                TempData["Type"] = "alert-danger";

                return RedirectToAction("Details", "Users", new {userId});
            }
            catch (Exception ex)
            {

                TempData["Message"] = $"Something went wrong - {ex}";
                TempData["Type"] = "alert-danger";
                return RedirectToAction("Index", "Users", new { userId});
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,ProjectLeader")] 
        public ActionResult Details(ProjectDetailsViewModel model) 
        {
            var project = _context.Projects
                .Include("Employees")
                .Include("ProjectLeader")
                .Include("Employees.Competences")
                .Include("Employees.Roles")
                .SingleOrDefault(p => p.Id == model.Id) ?? new Project();

            if (User.Identity.GetUserId() == project.ProjectLeader.Id || User.IsInRole("Admin"))
            {
                if (ModelState.IsValid)
                { 
                    project.Title = model.Title;
                    project.Description = model.Description;
                    project.Platform = model.Platform;
                    project.StartDate = model.StartDate;
                    project.ReleaseDate = model.ReleaseDate;
                    project.IsActive = model.IsActive;

                    _context.SaveChanges();

                    TempData["Message"] = $"{project.Title} was successfully updated!";
                    TempData["Type"] = "alert-success";

                    return RedirectToAction("Details", new {id = model.Id});
                }
                CreateDetailViewBags(project);

                TempData["Message"] = $"{model.Title} was not updated!";
                TempData["Type"] = "alert-danger";
                model.Employees = project.Employees ?? new List<User>();
                
                return View(model);
            }

            TempData["Message"] = $"You are not authorized to update {project.Title}!";
            TempData["Type"] = "alert-danger";

            return RedirectToAction("Details", new { id = model.Id });
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            var project = _context.Projects.SingleOrDefault(p => p.Id == id);

            if (project == null)
            {
                TempData["Message"] = $"{project.Title} was not deleted!";
                TempData["Type"] = "alert-danger";

                return RedirectToAction("AllProjects");
            }

            _context.Projects.Remove(project);
            _context.SaveChanges();

            TempData["Message"] = $"{project.Title} was successfully deleted!";
            TempData["Type"] = "alert-success";

            return RedirectToAction("AllProjects");
        }

        [Authorize(Roles = "Admin, ProjectLeader")]
        public ActionResult RemoveUser(int id, string userid)
        {
            var project = _context.Projects
                .Include(p => p.Employees)
                .Include(p => p.ProjectLeader)
                .SingleOrDefault(p => p.Id == id) ?? new Project();

            if (User.Identity.GetUserId() == project.ProjectLeader.Id || User.IsInRole("Admin"))
            {
                if (project.Employees.Count < 0)
                {
                    TempData["Message"] = $"¨Thera are no employees in {project.Title} to remove";
                    TempData["Type"] = "alert-danger";

                    return RedirectToAction("Details", new { id });
                }

                var employee = project.Employees.SingleOrDefault(e => e.Id == userid) ?? new User();

                project.Employees.Remove(employee);
                _context.SaveChanges();

                TempData["Message"] = $"{employee.FirstName} was successfully removed from {project.Title}!";
                TempData["Type"] = "alert-success";

                return RedirectToAction("Details", new { id });
            }

            TempData["Message"] = $"You are not authorized to remove employees from {project.Title}";
            TempData["Type"] = "alert-danger";

            return RedirectToAction("Details", new { id });

        }
        
        [HttpPost]
        [Authorize(Roles = "Admin, ProjectLeader")]
        public ActionResult AddEmployee(int id, string userId)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == userId);
            var project = _context.Projects
                .Include(p => p.ProjectLeader)
                .Include(p => p.Employees)
                .SingleOrDefault(p => p.Id == id) ?? new Project();

            if (User.Identity.GetUserId() == project.ProjectLeader.Id || User.IsInRole("Admin"))
            {
                if (user == null)
                {
                    TempData["Message"] = $"There are no employyes to add in {project.Title}";
                    TempData["Type"] = "alert-danger";

                    return RedirectToAction("Details", new { id });
                }

                project.Employees.Add(user);
                _context.SaveChanges();

                TempData["Message"] = $"{user.FirstName} was successfully added!";
                TempData["Type"] = "alert-success";

                return RedirectToAction("Details", new {id});
            }
            TempData["Message"] = $"You are not authorized to add employees to {project.Title}";
            TempData["Type"] = "alert-danger";

            return RedirectToAction("Details", new { id });
        }

        [Authorize(Roles="Admin")]
        public ActionResult ChangeProjectLeader(int id, string userId) 
        {
            var projectLeader = _context.Users.SingleOrDefault(u => u.Id == userId);

            var project = _context.Projects
                .Include(p => p.ProjectLeader)
                .SingleOrDefault(p => p.Id == id);
            if (projectLeader == null)
            {
                TempData["Message"] = $"Something went wrong - Project leaders could not be found";
                TempData["Type"] = "alert-danger";

                return RedirectToAction("Details", new { id });
            }

            project.ProjectLeader = projectLeader;
            _context.SaveChanges();
            
            TempData["Message"] = $"{projectLeader.FirstName} is now project leader for \"{project.Title}\"";
            TempData["Type"] = "alert-success";

            return RedirectToAction("Details", new { id });
        }

        #region Helpers
        private void CreateDetailViewBags(Project project)
        {
            var employees = _context.Users.ToList();

            ViewBag.Employees = employees.Select(e => new SelectListItem { Value = e.Id, Text = $"{e.FirstName} {e.LastName}" });

            ViewBag.AvailableEmployees = employees
                .Where(e => !project.Employees.Any(pe => pe.Id == e.Id) && e.Id != project.ProjectLeader.Id)
                .Select(e => new SelectListItem { Value = e.Id, Text = $"{e.FirstName} {e.LastName}" });
            //?? new SelectListItem {Text = "There is no avalible employee"};

            ViewBag.ProjectLeaders = employees
                .Where(e => UserManager.IsInRole(e.Id, "ProjectLeader"))
                .Select(e => new SelectListItem { Value = e.Id, Text = $"{e.FirstName} {e.LastName}" });
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

    }
}