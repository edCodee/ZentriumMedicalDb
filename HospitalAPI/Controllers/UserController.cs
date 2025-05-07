using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIhospital.Models;
using APIhospital.Data;
using Microsoft.AspNetCore.Identity.Data;
using HospitalAPI.Models;

namespace MiniMarketAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetUsers()
        {
            var users = await _context.users.ToListAsync();

            var result = users.Select(u => new
            {
                u.users_serial,
                u.users_id,
                u.users_firstName,
                u.users_middleName,
                u.users_lastName,
                u.users_secondLastName,
                u.users_email,
                u.users_userName,
                users_dateOfBirth = u.users_dateOfBirth.ToString("yyyy-MM-dd"),
                users_createdAt = u.users_createdAt?.ToString("yyyy-MM-dd HH:mm:ss"),
                users_updatedAt = u.users_updatedAt?.ToString("yyyy-MM-dd HH:mm:ss"),
                u.users_roleSerial,
                users_photo = u.users_photo != null ? Convert.ToBase64String(u.users_photo) : null
            });

            return Ok(result);
        }


        //POST: api/users
       [HttpPost]
        public async Task<ActionResult<UserModel>> CreateUser(UserModel user)
        {
            user.users_createdAt = DateTime.UtcNow;
            user.users_updatedAt = DateTime.UtcNow;

            _context.users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsers), new { id = user.users_serial }, user);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel request)
        {
            if (string.IsNullOrWhiteSpace(request.users_id) || string.IsNullOrWhiteSpace(request.users_password))
                return BadRequest("Cédula y contraseña son requeridas.");

            try
            {
                var user = await _context.users.FirstOrDefaultAsync(u => u.users_id == request.users_id);

                if (user == null)
                    return Unauthorized("Usuario no encontrado.");

                var inputPasswordBytes = System.Text.Encoding.UTF8.GetBytes(request.users_password);
                bool passwordsMatch = inputPasswordBytes.SequenceEqual(user.users_password);

                if (!passwordsMatch)
                    return Unauthorized("Contraseña incorrecta.");

                return Ok(new
                {
                    user.users_id,
                    user.users_firstName,
                    user.users_lastName,
                    user.users_email,
                    user.users_userName
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("role/{userId}")]
        public async Task<IActionResult> GetUserRole(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new { message = "El userId es obligatorio." });
            }

            var user = await _context.users
                .FirstOrDefaultAsync(u => u.users_id == userId); // <-- Cambia 'UsersId' si tu campo real se llama diferente

            if (user == null)
            {
                return NotFound(new { message = "Usuario no encontrado." });
            }

            // Creamos un objeto RoleUserModel con los datos que quieres exponer
            var roleUser = new RoleUserModel(user.users_id, user.users_roleSerial);

            return Ok(roleUser);
        }





    }
}
