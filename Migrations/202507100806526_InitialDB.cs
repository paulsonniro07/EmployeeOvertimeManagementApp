namespace EmployeeOvertimeManagementApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        DepartmentId = c.Int(nullable: false, identity: true),
                        DepartmentName = c.String(),
                    })
                .PrimaryKey(t => t.DepartmentId);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        EmployeeId = c.Int(nullable: false, identity: true),
                        NIK = c.String(nullable: false, maxLength: 20),
                        EmployeeName = c.String(nullable: false, maxLength: 100),
                        Position = c.String(nullable: false, maxLength: 50),
                        BpjsAllowance = c.Boolean(nullable: false),
                        MealAllowance = c.Boolean(nullable: false),
                        LaptopAllowance = c.Boolean(nullable: false),
                        DepartmentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EmployeeId)
                .ForeignKey("dbo.Departments", t => t.DepartmentId, cascadeDelete: true)
                .Index(t => t.NIK, unique: true)
                .Index(t => t.DepartmentId);
            
            CreateTable(
                "dbo.Overtimes",
                c => new
                    {
                        OverTimeId = c.Int(nullable: false, identity: true),
                        EmployeeId = c.Int(nullable: false),
                        TimeStart = c.DateTime(nullable: false),
                        TimeFinish = c.DateTime(nullable: false),
                        ActualHours = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CalculatedHours = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.OverTimeId)
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: true)
                .Index(t => t.EmployeeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Overtimes", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.Employees", "DepartmentId", "dbo.Departments");
            DropIndex("dbo.Overtimes", new[] { "EmployeeId" });
            DropIndex("dbo.Employees", new[] { "DepartmentId" });
            DropIndex("dbo.Employees", new[] { "NIK" });
            DropTable("dbo.Overtimes");
            DropTable("dbo.Employees");
            DropTable("dbo.Departments");
        }
    }
}
