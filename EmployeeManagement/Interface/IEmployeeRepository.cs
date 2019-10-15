using System.Collections.Generic;
using EmployeeManagement.Models;

namespace EmployeeManagement.Interface
{
    public interface IEmployeeRepository
    {
        Employee GetEmployee(int Id);

        IEnumerable<Employee> GetEmployeeList();

        Employee AddEmployee(Employee employee);

        Employee UpdateEmployee(Employee employee);
        Employee DeleteEmployee(int id);

    }
}