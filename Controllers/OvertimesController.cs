using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing.Printing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using EmployeeOvertimeManagementApp.Models;

namespace EmployeeOvertimeManagementApp.Controllers
{
    public class OvertimesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Overtimes
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            //var overtimes = db.Overtimes.Include(o => o.Employee);
            var overtimes = db.Overtimes
                .Include(o => o.Employee)
                .OrderBy(e => e.Employee.NIK)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.TotalPages = Math.Ceiling((double)overtimes.Count() / 10);
            ViewBag.CurrentPage = 1;

            return View(overtimes);
        }

        // GET: Overtimes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Overtime overtime = db.Overtimes.Find(id);
            if (overtime == null)
            {
                return HttpNotFound();
            }
            return View(overtime);
        }

        // GET: Overtimes/Create
        public ActionResult Create()
        {
            ViewBag.EmployeeId = new SelectList(db.Employees, "EmployeeId", "NIK");
            return View();
        }

        // POST: Overtimes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Overtime overtime)
        {
            if (ModelState.IsValid)
            {
                // Calculate hours
                TimeSpan duration = overtime.TimeFinish - overtime.TimeStart;
                overtime.ActualHours = (decimal)duration.TotalHours;
                overtime.CalculatedHours = overtime.ActualHours * 2;

                if (overtime.TimeFinish <= overtime.TimeStart)
                {
                    ModelState.AddModelError("", "Time Finish must be after Time Start");
                }

                // Validate maximum hours
                if (overtime.ActualHours > 3)
                {
                    ModelState.AddModelError("", "Actual OT hours cannot exceed 3 hours");
                    ViewBag.EmployeeId = new SelectList(db.Employees.Select(e => new
                    {
                        EmployeeId = e.EmployeeId,
                        DisplayText = e.NIK + " - " + e.EmployeeName
                    }), "EmployeeId", "DisplayText", overtime.EmployeeId);
                }

                db.Overtimes.Add(overtime);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EmployeeId = new SelectList(db.Employees.Select(e => new
            {
                EmployeeId = e.EmployeeId,
                DisplayText = e.NIK + " - " + e.EmployeeName
            }), "EmployeeId", "DisplayText", overtime.EmployeeId);

            return View(overtime);
        }

        // GET: Overtimes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Overtime overtime = db.Overtimes.Find(id);
            if (overtime == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeId = new SelectList(db.Employees, "EmployeeId", "NIK", overtime.EmployeeId);
            return View(overtime);
        }

        // POST: Overtimes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Overtime overtime)
        {
            if (ModelState.IsValid)
            {
                // Recalculate hours
                TimeSpan duration = overtime.TimeFinish - overtime.TimeStart;
                overtime.ActualHours = (decimal)duration.TotalHours;
                overtime.CalculatedHours = overtime.ActualHours * 2;

                if (overtime.TimeFinish <= overtime.TimeStart)
                {
                    ModelState.AddModelError("", "Time Finish must be after Time Start");
                }

                // Validate maximum hours
                if (overtime.ActualHours > 3)
                {
                    ModelState.AddModelError("", "Actual OT hours cannot exceed 3 hours");

                    ViewBag.EmployeeId = new SelectList(db.Employees.Select(e => new
                    {
                        EmployeeId = e.EmployeeId,
                        DisplayText = e.NIK + " - " + e.EmployeeName
                    }), "EmployeeId", "DisplayText", overtime.EmployeeId);

                    return View(overtime);
                }

                db.Entry(overtime).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EmployeeId = new SelectList(db.Employees.Select(e => new
            {
                EmployeeId = e.EmployeeId,
                DisplayText = e.NIK + " - " + e.EmployeeName
            }), "EmployeeId", "DisplayText", overtime.EmployeeId);

            return View(overtime);
        }

        // GET: Overtimes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var overtime = db.Overtimes
                .Include(o => o.Employee)
                .FirstOrDefault(o => o.OverTimeId == id);

            if (overtime == null)
            {
                return HttpNotFound();
            }
            return View(overtime);
        }

        // POST: Overtimes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Overtime overtime = db.Overtimes.Find(id);

            db.Overtimes.Remove(overtime);
            db.SaveChanges();
            return RedirectToAction("Index");
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
