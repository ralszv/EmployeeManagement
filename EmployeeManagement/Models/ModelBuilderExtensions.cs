using EmployeeManagement.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Models
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
                 
                new Employee
                {
                    Id = 1,
                    Name = "Mark Stark",
                    Department = Dept.IT,
                    Email = "Jack@gmail.com"
                },
                new Employee
                {
                    Id = 2,
                    Name = "Jane Stark",
                    Department = Dept.HR,
                    Email = "Jane@gmail.com"
                }
            );
        }
    }
}
