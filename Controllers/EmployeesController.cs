using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EmployeeOvertimeManagementApp.Models;

namespace EmployeeOvertimeManagementApp.Controllers
{
    public class EmployeesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Employees
        public ActionResult Index(int page =1, int pageSize = 10)
        {
            var employees = db.Employees
                .Include(e => e.Department)
                .OrderBy(e => e.NIK) 
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.TotalPages = Math.Ceiling((double)employees.Count() / 10);
            ViewBag.CurrentPage = 1; 

            return View(employees);
        }

        // GET: Employees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            ViewBag.Departments = db.Departments.OrderBy(d => d.DepartmentName).ToList();
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                // Check for duplicate NIK
                if (db.Employees.Any(e => e.NIK == employee.NIK))
                {
                    ModelState.AddModelError("NIK", "NIK already exists");
                    ViewBag.Departments = db.Departments.OrderBy(d => d.DepartmentName).ToList();
                    return View(employee);
                }

                db.Employees.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Departments = db.Departments.OrderBy(d => d.DepartmentName).ToList();
            return View(employee);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }

            // Populate departments for dropdown
            ViewBag.Departments = db.Departments.ToList();

            // Add position options
            var positionOptions = new List<string> { "Operator", "Technician", "Leader", "Supervisor", "Manager" };
            ViewBag.PositionList = new SelectList(positionOptions, employee.Position);

            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                var normalizedNIK = employee.NIK.Trim().ToLower();

                // Check for duplicate NIK (excluding current employee)
                if (db.Employees.Any(e => e.NIK.Trim().ToLower() == normalizedNIK && e.EmployeeId != employee.EmployeeId))
                {
                    ModelState.AddModelError("NIK", "NIK already exists");
                    ViewBag.Departments = db.Departments.ToList();
                    ViewBag.PositionList = new SelectList(new List<string> { "Operator", "Technician", "Leader", "Supervisor", "Manager" }, employee.Position);
                    return View(employee);
                }

                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Repopulate dropdowns if model state is invalid
            ViewBag.Departments = db.Departments.ToList();
            ViewBag.PositionList = new SelectList(new List<string> { "Operator", "Technician", "Leader", "Supervisor", "Manager" }, employee.Position);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
             Employee employee = db.Employees
            .Include(e => e.Overtimes)
            .FirstOrDefault(e => e.EmployeeId == id);

            if (employee == null)
                return HttpNotFound();

            if (employee.Overtimes.Any())
            {
                ModelState.AddModelError("", "Cannot delete employee with existing overtime records.");
                return View(employee); 
            }

            db.Employees.Remove(employee);
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
