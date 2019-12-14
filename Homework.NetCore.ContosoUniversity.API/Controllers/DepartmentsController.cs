using Homework.NetCore.ContosoUniversity.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace Homework.NetCore.ContosoUniversity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly ContosouniversityContext _context;
        private readonly ILogger _logger;

        public DepartmentsController(ContosouniversityContext context,
            ILogger<DepartmentsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Departments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetDepartment()
        {
            return await _context.Department.ToListAsync();
        }

        // GET: api/Departments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Department>> GetDepartment(int id)
        {
            var department = await _context.Department.FindAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            return department;
        }

        // PUT: api/Departments/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDepartment(int id, Department department)
        {
            if (id != department.DepartmentId)
            {
                return BadRequest();
            }

            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"EXEC [dbo].[Department_Update] {department.DepartmentId}, {department.Name}, {department.Budget}, {department.StartDate}, {department.InstructorId}, {department.RowVersion}"
                );
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!DepartmentExists(id))
                {
                    return NotFound();
                }

                _logger.LogError("PutCourse Fail! ", ex);
                throw;
            }

            return NoContent();
        }

        // POST: api/Departments
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Department>> PostDepartment(Department department)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var insertedResult = await _context.SpDepartmentInsertResults.FromSqlInterpolated(
                $"EXEC [dbo].[Department_Insert] {department.Name}, {department.Budget}, {department.StartDate}, {department.InstructorId} "
            ).Select(s => s.DepartmentId).ToListAsync();

            department.DepartmentId = insertedResult.First();
            
            return CreatedAtAction("GetDepartment", new {id = department.DepartmentId }, department);
        }

        // DELETE: api/Departments/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Department>> DeleteDepartment(int id)
        {
            var department = await _context.Department.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            await _context.Database.ExecuteSqlInterpolatedAsync(
                $"EXEC [dbo].[Department_Delete] {department.DepartmentId}, {department.RowVersion}"
            );

            return department;
        }

        // GET: api/Departments/CourseCount
        [HttpGet("CourseCount")]
        public async Task<ActionResult<IEnumerable<VwDepartmentCourseCount>>> GetDepartmentCourseCount()
        {
            var data = await _context.VwDepartmentCourseCount.FromSqlRaw("SELECT * FROM VwDepartmentCourseCount").ToListAsync();
            return data;
        }

        private bool DepartmentExists(int id)
        {
            return _context.Department.Any(e => e.DepartmentId == id);
        }
    }
}
