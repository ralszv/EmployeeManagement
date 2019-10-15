using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement.Interface;
using Microsoft.AspNetCore.Mvc;
using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Controllers
{
    //[Route("[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger _logger;

        public HomeController(IEmployeeRepository employeeRepository,
                              IHostingEnvironment hostingEnvironment,
                              ILogger<HomeController> logger)
        {
            _employeeRepository = employeeRepository;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }

        public string Index()
        {
            return _employeeRepository.GetEmployee(2).Name;
            //return View();
        }

        public IActionResult List()
        {
             
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                EmployeeList = _employeeRepository.GetEmployeeList(),
                PageTitle = "Employee Details"
            };

            return View(homeDetailsViewModel);
        }

        //[Route("{id?}")]
        public IActionResult Details(int? id)
        {
            //throw new Exception("Error in details view");
            _logger.LogTrace("Trace Log");
            _logger.LogDebug("Debug Log");
            _logger.LogInformation("Information Log");
            _logger.LogWarning("Warning Log");
            _logger.LogError("Error Log");
            _logger.LogCritical("Critical Log");

            Employee model = _employeeRepository.GetEmployee(id.Value);
            if (model == null)
            {
                return View("EmployeeNotFound", id.Value);
            }

            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                EmployeeList = new List<Employee>() { model },
                PageTitle = "Employee Details"
            };

            return View(homeDetailsViewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(EmployeeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                {
                    string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName.Split(@"\").Last();
                    string filePath = Path.Combine(uploadFolder, uniqueFileName);
                    model.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
                }

                Employee newEmployee = new Employee
                {
                    Name = model.Name,
                    Email = model.Email,
                    Department = model.Department,
                    Image = uniqueFileName
                };

                newEmployee = _employeeRepository.AddEmployee(newEmployee);
                return RedirectToAction("details", new {id = newEmployee.Id});
            }

            return View();
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Employee employee = _employeeRepository.GetEmployee(id);
            EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel
            {
                Id = employee.Id,
                Name = employee.Name,
                Department = employee.Department,
                Email = employee.Email,
                ExistingPhotoPath = employee.Image
            };
            return View(employeeEditViewModel);
        }

        [HttpPost]
        public IActionResult Edit(EmployeeEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Employee employee = _employeeRepository.GetEmployee(model.Id);

                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Department = model.Department;

                if (model.Photo != null)
                {
                    if (model.ExistingPhotoPath != null)
                    {
                        string filePath = Path.Combine(_hostingEnvironment.WebRootPath, "images",
                            model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }

                    employee.Image = ProcessUploadedFile(model);
                }

                Employee updatedEmployee = _employeeRepository.UpdateEmployee(employee);
            }

            return View(model);
        }

        public IActionResult Delete(int id)
        {
            Employee deletedEmployee = _employeeRepository.DeleteEmployee(id);
            return RedirectToAction("List");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        private string ProcessUploadedFile(EmployeeEditViewModel model)
        {
            string uniqueFileName = null;

            if (model.Photo != null)
            {
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName.Split(@"_").Last();
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
