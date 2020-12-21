using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using intervention_management.Models;

namespace Intervention_management.Controllers
{
    [Produces("application/json")]

    [Route("api/[controller]")]
    [ApiController]

    public class EmployeesController : ControllerBase
    {
        private readonly Rocket_app_developmentContext _context;

        public EmployeesController(Rocket_app_developmentContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> Getemployees()
        {
            return await _context.employees.ToListAsync();
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployees(long id, string first_name, string last_name, string email, string title, string phone_number)
        {
            var employees = await _context.employees.FindAsync(id);

            if (employees == null)
            {
                return NotFound();
            }

            var jsonGet = new Newtonsoft.Json.Linq.JObject();
            jsonGet["id"] = employees.Id;
            jsonGet["first_name"] = employees.first_name;
            jsonGet["last_name"] = employees.last_name;
            jsonGet["email"] = employees.email;
            jsonGet["title"] = employees.title;
            jsonGet["phone_number"] = employees.phone_number;
            return Content(jsonGet.ToString(), "application/json");
        }

        // Checking  If the employee's email exists
        [HttpGet("email/{employee_email}")]
        public ActionResult<List<Employee>> VerifyEmployee(string employee_email)
        {
            var email = _context.employees.Where(t => t.email == employee_email).ToList();
            if (email.Count == 0) { return NotFound(); }
            return Ok();
        }
    }
}