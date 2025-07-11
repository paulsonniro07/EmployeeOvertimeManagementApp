using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EmployeeOvertimeManagementApp.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        [Required]
        [StringLength(20)]
        [Index(IsUnique = true)]
        public string NIK { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = " Name")]
        public string EmployeeName { get; set; }

        [Required]
        [StringLength(50)]
        public string Position { get; set; }

        public bool BpjsAllowance { get; set; }
        public bool MealAllowance { get; set; }
        public bool LaptopAllowance { get; set; }

        public int DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }
        public virtual ICollection<Overtime> Overtimes { get; set; }

    }
}