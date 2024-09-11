using CourseEnrollment.Server.Data;
using CourseEnrollment.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using System;
using System.ComponentModel;

namespace CourseEnrollment.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EnrollmentsController> _logger;

        public EnrollmentsController(ApplicationDbContext context, ILogger<EnrollmentsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("{studentId}")]
        public List<Enrollment> GetEnrollments(int studentId)
        {
            try
            {
                var enrollments = _context.Enrollments
                                  .Include(e => e.Course)
                                  .Where(e => e.StudentId == studentId).ToList();
                return enrollments;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving enrollments.");
                return null;
            }
        }      

        [HttpPost]
        public async Task<ActionResult<List<Enrollment>>> CreateEnrollment(Enrollment enrollment)
        {
            try
            {                
                var existingEnrollment = await _context.Enrollments
                    .AnyAsync(e => e.StudentId == enrollment.StudentId && e.CourseId == enrollment.CourseId);

                if (existingEnrollment)
                {
                    return Conflict("This enrollment already exists.");
                }

                _context.Enrollments.Add(enrollment);
                await _context.SaveChangesAsync();

                var enrollments = await _context.Enrollments
                    .Include(e => e.Course)
                    .Where(e => e.StudentId == enrollment.StudentId)
                    .ToListAsync();

                return Ok(enrollments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in CreateEnrollment() method");
                return null;
            }
        }


        [HttpDelete("{studentId}/{courseId}")]
        public async Task<IActionResult> DeleteEnrollment(int studentId, int courseId)
        {
            try
            {
                var enrollment = await _context.Enrollments
                    .FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId);                

                _context.Enrollments.Remove(enrollment);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in DeleteEnrollment() methid");
                return null;
            }
        }
    }
}
