using EmployeeOvertimeManagementApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace EmployeeOvertimeManagementApp.DAL
{
    public class DatabaseInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            var departments = new List<Department>
            {
                new Department { DepartmentName = "IT" },
                new Department { DepartmentName = "HR" },
                new Department { DepartmentName = "Production" },
            };

            departments.ForEach(d => context.Departments.Add(d));
            context.SaveChanges();

            base.Seed(context);
        }
    }
}