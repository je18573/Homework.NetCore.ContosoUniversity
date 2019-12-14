﻿using Homework.NetCore.ContosoUniversity.API.Models;
using Homework.NetCore.ContosoUniversity.API.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homework.NetCore.ContosoUniversity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ContosouniversityContext _context;
        private readonly ILogger _logger;

        public CoursesController(ContosouniversityContext context,
            ILogger<CoursesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Courses/ErrorTest
        [HttpGet("ErrorTest")]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourseErrorTest()
        {
            var i = 0;
            var d = 30 / i;
            return await _context.Course.ToListAsync();
        }

        // GET: api/Courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourse()
        {
            return await _context.Course.ToListAsync();
        }

        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            var course = await _context.Course.FindAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            return course;
        }

        // PUT: api/Courses/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, Course course)
        {
            if (id != course.CourseId)
            {
                return BadRequest();
            }

            _context.Entry(course).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!CourseExists(id))
                {
                    return NotFound();
                }

                _logger.LogError("PutCourse Fail! ", ex);
                throw;
            }

            return NoContent();
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> PatchCourse(int id, PatchCourseViewModel course)
        {
            var dbCourse = _context.Course.Find(id);
            if (dbCourse == null)
            {
                return NotFound();
            }

            if (!await TryUpdateModelAsync(course))
            {
                return BadRequest();
            }

            course.Credits += 1;

            if (!TryValidateModel(course))
            {
                return BadRequest();
            }

            dbCourse.Credits = course.Credits;
            dbCourse.Title = course.Title;
            _context.Entry(dbCourse).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!CourseExists(id))
                {
                    return NotFound();
                }

                _logger.LogError("PatchCourse Fail! ", ex);
                throw;
            }

            return NoContent();
        }

        // POST: api/Courses
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Course>> PostCourse(Course course)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            _context.Course.Add(course);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCourse), new { id = course.CourseId }, course);
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Course>> DeleteCourse(int id)
        {
            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            _context.Course.Remove(course);
            await _context.SaveChangesAsync();

            return course;
        }

        // GET: api/Courses/Students
        [HttpGet("Students")]
        public async Task<ActionResult<IList<VwCourseStudents>>> GetCourseStudents()
        {
            var data = await _context.VwCourseStudents.ToListAsync();
            return data;
        }

        // GET: api/Courses/StudentCount/
        [HttpGet("StudentCount")]
        public async Task<ActionResult<IList<VwCourseStudentCount>>> GetCourseStudentCount()
        {
            var data = await _context.VwCourseStudentCount.ToListAsync();
            return data;
        }

        private bool CourseExists(int id)
        {
            return _context.Course.Any(e => e.CourseId == id);
        }
    }
}
