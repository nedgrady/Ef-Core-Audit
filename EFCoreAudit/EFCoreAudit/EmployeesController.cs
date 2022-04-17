using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace EFCoreAudit
{
    [Route("[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly AuditingDbContext _auditingDbContext;

        public EmployeesController(AuditingDbContext auditingDbContext)
        {
            auditingDbContext.Database.EnsureCreated();
            _auditingDbContext = auditingDbContext;
        }

        [HttpPost()]
        public void Create([FromBody] CreateEmployeeRequest createEmployeeRequest)
        {
            _auditingDbContext.Employees.Add(new Employee
            {
                Name = createEmployeeRequest.Name
            });
            _auditingDbContext.SaveChanges();
        }

        [HttpPost("{id}")]
        public void Update([FromRoute] int id, [FromBody] UpdateEmployeeRequest createEmployeeRequest)
        {
            _auditingDbContext.Employees.Single(e => e.Id == id).Name = createEmployeeRequest.Name;
            _auditingDbContext.SaveChanges();
        }

        [HttpPost("History/{id}")]
        public IEnumerable<EmployeeHistory> GetHistory([FromRoute] int id) {
            return _auditingDbContext
                .Employees
                .TemporalAll()
                .Where(employee => employee.Id == id)
                .Select(employee => new EmployeeHistory
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    WhenWasTheChangeMade = EF.Property<DateTime>(employee, "PeriodStart"),
                    WhoDidTheChange = employee.Username
                })
                .ToList();
        }

    }

    public class EmployeeHistory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string WhoDidTheChange { get; set; }
        public DateTime WhenWasTheChangeMade { get; set; }
    }

    public class CreateEmployeeRequest
    {
        [Required]
        public string Name { get; set; }        
    }

    public class UpdateEmployeeRequest
    {
        [Required]
        public string Name { get; set; }        
    }
}
