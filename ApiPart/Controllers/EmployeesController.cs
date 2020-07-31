using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ApiPart.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly ILogger<EmployeesController> _logger;

        public EmployeesController(ILogger<EmployeesController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Employee> Get()
        {
            return employeesArray;
        }

        [HttpGet("{id}")]
        public Employee GetItem(int id)
        {
            return employeesArray[id];
        }

        public Employee[] employeesArray = {
            new Employee{
                DateHired = new DateTime(2016, 7, 15, 3, 15, 0),
                BithYear = 1986,
                Role = "CEO",
                Salary = 530000,
                Department = "Administrative"
            },
            new Employee{
                DateHired = new DateTime(2017, 7, 27, 12, 15, 0),
                BithYear = 1992,
                Role = "Manager",
                Salary = 240000,
                Department = "Administrative"
            },
            new Employee{
                DateHired = new DateTime(2018, 5, 15, 3, 15, 0),
                BithYear = 1995,
                Role = "Developer",
                Salary = 110000,
                Department = "Development"
            },
            new Employee{
                DateHired = new DateTime(1986, 5, 17, 12, 0, 0),
                BithYear = 1995,
                Role = "QA",
                Salary = 105000,
                Department = "Development"
            },
        };
    }
}
