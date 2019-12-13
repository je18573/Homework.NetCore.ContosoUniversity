using Homework.NetCore.ContosoUniversity.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homework.NetCore.ContosoUniversity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseInstructorsController : ControllerBase
    {
        private readonly ContosouniversityContext _context;
        private readonly ILogger _logger;

        public CourseInstructorsController(ContosouniversityContext context,
            ILogger<CourseInstructorsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/CourseInstructors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseInstructor>>> GetCourseInstructor()
        {
            return await _context.CourseInstructor.ToListAsync();
        }

        // GET: api/CourseInstructors/5/5
        [HttpGet("{courseId}/{instructorId}")]
        public async Task<ActionResult<CourseInstructor>> GetCourseInstructor(int courseId, int instructorId)
        {
            var courseInstructor = await _context.CourseInstructor.FindAsync(courseId, instructorId);

            if (courseInstructor == null)
            {
                return NotFound();
            }

            return courseInstructor;
        }

        // PUT: api/CourseInstructors/5/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{courseId}/{instructorId}")]
        public async Task<IActionResult> PutCourseInstructor(int courseId, int instructorId, 
            CourseInstructor courseInstructor)
        {
            // 只更新 InstructorId
            if (!CourseInstructorExists(courseId, instructorId)
                || !PersonExists(courseInstructor.InstructorId))
            {
                return NotFound();
            }

            if (CourseInstructorExists(courseId, courseInstructor.InstructorId))
            {
                return Conflict();
            }

            await _context.Database.ExecuteSqlInterpolatedAsync(
                $"UPDATE dbo.CourseInstructor SET [InstructorID] = {courseInstructor.InstructorId} WHERE [InstructorID] = {instructorId} AND [CourseId] = {courseId}"
            );

            /*
              Question: 這張資料表只有兩個欄位對應，且都是Key，若要異動，只能刪除再新增。
                        => 是否有必要提供PUT呢？

              若依照自動產生器產生的程式碼，基本上不管傳送什麼都不會有資料異動；

              而經修改後的以下版本，再使用EF Core提供的SaveChange，則會出現錯誤說明↓↓
              System.InvalidOperationException: The property 'InstructorId' on entity type 'CourseInstructor' is part of a key and so cannot be modified or marked as modified. 
              To change the principal of an existing entity with an identifying foreign key first delete the dependent and invoke 'SaveChanges' then associate the dependent with the new principal.
            */

            //var dbData = await _context.CourseInstructor.FindAsync(courseId, instructorId);
            //if (dbData == null)
            //{
            //    return NotFound();
            //}
            //dbData.InstructorId = courseInstructor.InstructorId;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException ex)
            //{
            //    if (CourseInstructorExists(dbData.CourseId, courseInstructor.InstructorId))
            //    {
            //        return Conflict();
            //    }

            //    if (!PersonExists(courseInstructor.InstructorId))
            //    {
            //        return NotFound();
            //    }

            //    _logger.LogError("PutCourseInstructor Fail! ", ex);

            //    throw;
            //}

            return NoContent();
        }


        // POST: api/CourseInstructors
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<CourseInstructor>> PostCourseInstructor(CourseInstructor courseInstructor)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            _context.CourseInstructor.Add(courseInstructor);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (CourseInstructorExists(courseInstructor.CourseId, courseInstructor.InstructorId))
                {
                    return Conflict();
                }

                if (!CourseExists(courseInstructor.CourseId) 
                    || !PersonExists(courseInstructor.InstructorId))
                {
                    return NotFound();
                }

                _logger.LogError("PostCourseInstructor Fail! ", ex);

                throw;
            }

            return CreatedAtAction("GetCourseInstructor", new { id = courseInstructor.CourseId }, courseInstructor);
        }

        // DELETE: api/CourseInstructors/5/5
        [HttpDelete("{courseId}/{instructorId}")]
        public async Task<ActionResult<CourseInstructor>> DeleteCourseInstructor(int courseId, int instructorId)
        {
            var courseInstructor = await _context.CourseInstructor.FindAsync(courseId, instructorId);
            if (courseInstructor == null)
            {
                return NotFound();
            }

            _context.CourseInstructor.Remove(courseInstructor);
            await _context.SaveChangesAsync();

            return courseInstructor;
        }

        private bool CourseInstructorExists(int courseId, int instructorId)
        {
            return _context.CourseInstructor.Any(e => e.CourseId == courseId && e.InstructorId == instructorId);
        }
        private bool CourseExists(int id)
        {
            return _context.Course.Any(e => e.CourseId == id);
        }
        private bool PersonExists(int id)
        {
            return _context.Person.Any(e => e.Id == id);
        }
    }
}
