using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PagedList;
using Sample.Core.Models;

namespace Sample.Web.Controllers
{
    public class LeadController : Controller
    {
        private readonly DataContext _context;

        public LeadController(DataContext context)
        {
            _context = context;
        }

        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "Name_desc" : "";
            ViewBag.DateSortParam = sortOrder == "Date" ? "Date_desc" : "Date";
            ViewBag.FNameSortParam = sortOrder == "FName" ? "FName_desc" : "FName";
            ViewBag.MNameSortParam = sortOrder == "MName" ? "MName_desc" : "MName";
            ViewBag.LNameSortParam = sortOrder == "LName" ? "LName_desc" : "LName";
            ViewBag.EmailSortParam = sortOrder == "Email" ? "Email_desc" : "Email";
            ViewBag.PhoneSortParam = sortOrder == "Phone" ? "Phone_desc" : "Phone";
            ViewBag.StreetSortParam = sortOrder == "Street" ? "Street_desc" : "Street";
            ViewBag.CitySortParam = sortOrder == "City" ? "City_desc" : "City";
            ViewBag.StateSortParam = sortOrder == "State" ? "State_desc" : "State";
            ViewBag.ZipSortParam = sortOrder == "Zip" ? "Zip_desc" : "Zip";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            IQueryable<Lead> leads = from s in _context.Leads select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                leads = leads.Where(s =>
                    s.LastName.ToLower().Contains(searchString.ToLower()) ||
                    s.FirstName.ToLower().Contains(searchString.ToLower()) ||
                    s.MiddleName.ToLower().Contains(searchString.ToLower()) ||
                    s.Email.ToLower().Contains(searchString.ToLower()) ||
                    s.Phone.ToLower().Contains(searchString.ToLower()) ||
                    s.Address.Street.ToLower().Contains(searchString.ToLower()) ||
                    s.Address.City.ToLower().Contains(searchString.ToLower()) ||
                    s.Address.State.ToLower().Contains(searchString.ToLower()) ||
                    s.Address.Zip.ToLower().Contains(searchString.ToLower())
                );
            }
            switch (sortOrder)
            {
                // TODO: Refactor this, or replace with Angular on the client
                // This is just for demo purposes.
                case "Name_desc":
                    leads = leads.OrderByDescending(s => s.LastName);
                    break;
                case "FName":
                    leads = leads.OrderBy(s => s.FirstName);
                    break;
                case "FName_desc":
                    leads = leads.OrderByDescending(s => s.FirstName);
                    break;
                case "MName":
                    leads = leads.OrderBy(s => s.MiddleName);
                    break;
                case "MName_desc":
                    leads = leads.OrderByDescending(s => s.MiddleName);
                    break;
                case "LName":
                    leads = leads.OrderBy(s => s.LastName);
                    break;
                case "LName_desc":
                    leads = leads.OrderByDescending(s => s.LastName);
                    break;
                case "Email":
                    leads = leads.OrderBy(s => s.Email);
                    break;
                case "Email_desc":
                    leads = leads.OrderByDescending(s => s.Email);
                    break;
                case "Phone":
                    leads = leads.OrderBy(s => s.Phone);
                    break;
                case "Phone_desc":
                    leads = leads.OrderByDescending(s => s.Phone);
                    break;
                case "Street":
                    leads = leads.OrderBy(s => s.Address.Street);
                    break;
                case "Street_desc":
                    leads = leads.OrderByDescending(s => s.Address.Street);
                    break;
                case "City":
                    leads = leads.OrderBy(s => s.Address.City);
                    break;
                case "City_desc":
                    leads = leads.OrderByDescending(s => s.Address.City);
                    break;
                case "State":
                    leads = leads.OrderBy(s => s.Address.State);
                    break;
                case "State_desc":
                    leads = leads.OrderByDescending(s => s.Address.State);
                    break;
                case "Zip":
                    leads = leads.OrderBy(s => s.Address.Zip);
                    break;
                case "Zip_desc":
                    leads = leads.OrderByDescending(s => s.Address.Zip);
                    break;
                default: // Name ascending 
                    leads = leads.OrderBy(s => s.LastName);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(leads.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lead lead = _context.Leads.Find(id);
            if (lead == null)
            {
                return HttpNotFound();
            }
            return View(lead);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Lead lead)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Leads.Add(lead);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("",
                    "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(lead);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lead leads = _context.Leads.Find(id);
            if (leads == null)
            {
                return HttpNotFound();
            }
            return View(leads);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Lead lead)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Entry(lead).State = EntityState.Modified;
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("",
                    "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(lead);
        }

        public ActionResult Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage =
                    "Delete failed. Try again, and if the problem persists see your system administrator.";
            }
            Lead lead = _context.Leads.Find(id);
            if (lead == null)
            {
                return HttpNotFound();
            }
            return View(lead);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                Lead lead = _context.Leads.Find(id);
                _context.Leads.Remove(lead);
                _context.SaveChanges();
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("Delete", new { id, saveChangesError = true });
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}