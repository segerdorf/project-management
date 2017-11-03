using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using RosalieSegerdorf_MVC1.Data;
using RosalieSegerdorf_MVC1.Data.Models;
using RosalieSegerdorf_MVC1.Helpers;
using RosalieSegerdorf_MVC1.ViewModels.Competences;

namespace RosalieSegerdorf_MVC1.Controllers
{
    [Authorize]
    public class CompetencesController : Controller
    {
        private readonly DataContext _context;
        private ApplicationUserManager _userManager;

        public CompetencesController()
        {
            _context = new DataContext();
        }

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        public ActionResult Create(string userid)
        {
            return View(new CreateCompetenceViewModel{UserId = userid});
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(CreateCompetenceViewModel newCompetence)
        {
            if (!ModelState.IsValid)
                return View(newCompetence);

            var user = _context.Users.Include(u => u.Competences).SingleOrDefault(u => u.Id == newCompetence.UserId);

            if (user.Competences.Any(x => x.Expertise == newCompetence.Expertise))
            {
                TempData["Message"] = $"{newCompetence.Expertise} is already defined. You may edit the expertise from the list of competences." ;
                TempData["Type"] = "alert-danger";
                return View(newCompetence);
            }

            var c = new Competence
            {
                User = user,
                YearOfExperience = newCompetence.YearOfExperience,
                Expertise = newCompetence.Expertise
            };

            _context.Competences.Add(c);
            _context.SaveChanges();

            user.Competences.Add(c);
            _context.SaveChanges();

            return RedirectToAction("Details", "Users", new {id = user.Id});
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            var competence = _context.Competences.Include(c => c.User).SingleOrDefault(c => c.Id == id);


            return View(new CreateCompetenceViewModel
            {
                Id = competence.Id,
                Expertise = competence.Expertise,
                YearOfExperience = competence.YearOfExperience,
                UserId = competence.User.Id
            });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(CreateCompetenceViewModel updateCompetence)
        {
            if (!ModelState.IsValid)
            {
                return View(updateCompetence);
            }
            var competence = _context.Competences.Include(c => c.User).SingleOrDefault(c => c.Id == updateCompetence.Id);

            competence.YearOfExperience = updateCompetence.YearOfExperience;
            _context.SaveChanges();

            return RedirectToAction("Details", "Users", new{ id = competence.User.Id });
        }
        
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id, string userid)
        {
            var competence = _context.Competences.Include(c => c.User).SingleOrDefault(c => c.Id == id);

            if (competence == null)
            {
                TempData["Message"] = "The competence was not deleted";
                TempData["Type"] = "alert-danger";

                return RedirectToAction("Details", "Users", new { id = userid });

            }
            
            _context.Competences.Remove(competence);
            _context.SaveChanges();

            TempData["Message"] = $"{competence.Expertise.EnumToString()} was successfully deleted!";
            TempData["Type"] = "alert-success";

            return RedirectToAction("Details", "Users", new { id = userid });
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

    }
}