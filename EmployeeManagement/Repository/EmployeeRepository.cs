using System.Collections.Generic;
using System.Linq;
using EmployeeManagement.Interface;
using EmployeeManagement.Models;
using EmployeeManagement.Models.Enum;

namespace EmployeeManagement.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private List<Employee> _employeeList;

        public EmployeeRepository()
        {
            _employeeList = new List<Employee>()
            {
                new Employee() {Id = 1, Name = "Jack Straw", Department = Dept.IT, Email = "jackstraw@gmail.com"},
                new Employee() {Id = 2, Name = "Peter Straw", Department = Dept.HR, Email = "peterstraw@gmail.com"},
                new Employee() {Id = 3, Name = "Tom Straw", Department = Dept.Finance, Email = "tomstraw@gmail.com"}

            };
        }
        public Employee GetEmployee(int Id)
        {
            return _employeeList.FirstOrDefault(x => x.Id == Id);
        }
        public IEnumerable<Employee> GetEmployeeList()
        {
            return _employeeList;
        }

        public Employee AddEmployee(Employee employee)
        {
            employee.Id = _employeeList.Max((e => e.Id)) + 1;
            _employeeList.Add((employee));

            return employee;
        }

        public Employee UpdateEmployee(Employee employeeChanges)
        {
            Employee employee = _employeeList.FirstOrDefault(e => e.Id == employeeChanges.Id);
            if (employee != null)
            {
                employee.Name = employeeChanges.Name;
                employee.Email = employeeChanges.Email;
                employee.Department = employeeChanges.Department;
            }

            return employee;
        }

        public Employee DeleteEmployee(int id)
        {
            Employee employee =  _employeeList.FirstOrDefault(e => e.Id == id);
            if (employee != null)
            {
                _employeeList.Remove(employee);
            }

            return employee;
        }
    }
}