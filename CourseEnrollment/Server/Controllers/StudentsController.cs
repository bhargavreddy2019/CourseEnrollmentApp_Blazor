using CourseEnrollment.Server.Data;
using CourseEnrollment.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CourseEnrollment.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<Student>> Register(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, student);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginRequest loginRequest)
        {
            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.Email == loginRequest.Email && s.Password == loginRequest.Password);

            if (student == null)
            {
                return Unauthorized();
            }

            var token = GenerateJwtToken(student);
            return Ok(token);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return student;
        }

        private string GenerateJwtToken(Student student)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, student.Email),
                new Claim(JwtRegisteredClaimNames.Jti, student.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSecretKey"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "yourdomain.com",
                audience: "yourdomain.com",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}