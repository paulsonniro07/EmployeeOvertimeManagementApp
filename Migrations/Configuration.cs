namespace EmployeeOvertimeManagementApp.Migrations
{
    using EmployeeOvertimeManagementApp.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<EmployeeOvertimeManagementApp.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(EmployeeOvertimeManagementApp.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
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
