using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EmployeeOvertimeManagementApp.Models
{
    public class Overtime
    {
        public int OverTimeId { get; set; }

        public int EmployeeId { get; set; }

        [Required]
        public DateTime TimeStart { get; set; }

        [Required]
        public DateTime TimeFinish { get; set; }

        public decimal ActualHours { get; set; }
        public decimal CalculatedHours { get; set; }

        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }
    }
}