using APIhospital.Data;
using HospitalAPI.DTOs.RoleDTOs;
using HospitalAPI.DTOs.UserDTOs;
using HospitalAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.NetworkInformation;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace HospitalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController:ControllerBase
    {
        private readonly AppDbContext _context;
        public UserController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Hash con SHA256
        /// </summary>
        private static byte[] HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        [HttpGet("roles")]
        public async Task<ActionResult<IEnumerable<RoleReadDTO>>> GetRoles()
        {
            var roles = await _context.role
                .Select(r => new RoleReadDTO
                {
                    RoleSerial = r.role_serial,
                    RoleName = r.role_name
                })
                .ToListAsync();

            return Ok(roles);
        }

        //GET: api/user
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserReadDTO>>> GetUsers()
        {
            var users=await _context.user.ToListAsync();

            var userDTOs = users.Select(f => new UserReadDTO
            {
                UserSerial = f.user_serial,
                UserPhoto = f.user_photo !=null?Convert.ToBase64String(f.user_photo) : null,
                UserId = f.user_id,
                UserFirstName = f.user_firstName,
                UserMiddleName = f.user_middleName,
                UserLastName = f.user_lastName,
                UserSecondLastName = f.user_secondLastName,
                UserBirthDate = f.user_birthDate,
                UserUserName = f.user_userName,
                UserEmail = f.user_email
            });

            return Ok(userDTOs);
        }

        //POST: api/createuser
        [HttpPost]
        public async Task<ActionResult<UserReadDTO>> CreateUser(UserCreateDTO dto)
        {
            var user = new UserModel
            {
                //user_photo = string.IsNullOrWhiteSpace(dto.UserPhoto) ? null : Convert.FromBase64String(dto.UserPhoto),
                user_id = dto.UserId,
                user_firstName = dto.UserFirstName,
                user_middleName = string.IsNullOrWhiteSpace(dto.UserMiddleName) ? null : dto.UserMiddleName,
                user_lastName = dto.UserLastName,
                user_secondLastName = string.IsNullOrWhiteSpace(dto.UserSecondLastName) ? null : dto.UserSecondLastName,
                user_birthDate = dto.UserBirthDate.Date,
                user_userName = dto.UserUsername,
                user_email = string.IsNullOrWhiteSpace(dto.UserEmail) ? null : dto.UserEmail,
                user_password = HashPassword(dto.UserPassword)
            };

            _context.user.Add(user);
            await _context.SaveChangesAsync();

            // Asignar rol por defecto (paciente)
            var userRole = new UserRoleModel
            {
                userRole_userSerial = user.user_serial,
                userRole_roleSerial = 3, // rol 2 por defecto
                assigned_at = DateTime.Now
            };
            _context.user_role.Add(userRole);
            await _context.SaveChangesAsync();

            var result = new UserReadDTO
            {
                UserSerial = user.user_serial,
                UserPhoto = user.user_photo != null ? Convert.ToBase64String(user.user_photo) : null,
                UserId = user.user_id,
                UserFirstName = user.user_firstName,
                UserMiddleName = user.user_middleName,
                UserLastName = user.user_lastName,
                UserSecondLastName = user.user_secondLastName,
                UserBirthDate = user.user_birthDate,
                UserUserName = user.user_userName,
                UserEmail = user.user_email
            };

            return CreatedAtAction(nameof(GetUsers), new { id = user.user_serial }, result);
        }

        // GET: api/User/ByCedula/{cedula}
        [HttpGet("ByCedula/{cedula}")]
        public async Task<ActionResult<UserReadDTO>> GetUserByCedula(string cedula)
        {
            var user = await _context.user
                .FirstOrDefaultAsync(u => u.user_id == cedula);

            if (user == null)
            {
                return NotFound(new { message = "No se encontró ningún usuario con la cédula proporcionada." });
            }

            var userDTO = new UserReadDTO
            {
                UserSerial = user.user_serial,
                UserPhoto = user.user_photo != null ? Convert.ToBase64String(user.user_photo) : null,
                UserId = user.user_id,
                UserFirstName = user.user_firstName,
                UserMiddleName = user.user_middleName,
                UserLastName = user.user_lastName,
                UserSecondLastName = user.user_secondLastName,
                UserBirthDate = user.user_birthDate,
                UserUserName = user.user_userName,
                UserEmail = user.user_email
            };

            return Ok(userDTO);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.UserId) || string.IsNullOrWhiteSpace(request.UserPassword))
                return BadRequest("Cédula y contraseña requeridas.");

            try
            {
                var user = await _context.user
                    .Include(u => u.UserRoles)
                        .ThenInclude(ur => ur.Role)
                    .FirstOrDefaultAsync(u => u.user_id == request.UserId);

                if (user == null)
                    return Unauthorized("Usuario no encontrado.");

                var inputPasswordHashed = HashPassword(request.UserPassword);

                if (!inputPasswordHashed.SequenceEqual(user.user_password))
                    return Unauthorized("Contraseña incorrecta.");

                var key = Encoding.ASCII.GetBytes("pSKD93kdFJls00dkfJ2kfjLSKdj38DKSlskfjd94sldkfj"); // ⚠️ mover a appsettings

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.user_userName),
                    new Claim("cedula", user.user_id),
                    new Claim("user_serial", user.user_serial.ToString()),
                    new Claim("nombre", user.user_firstName),
                    new Claim("apellido", user.user_lastName),
                    new Claim("roles", string.Join(",", user.UserRoles.Select(r => r.Role!.role_name)))
                };

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddHours(2),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = new JwtSecurityTokenHandler().WriteToken(
                    new JwtSecurityTokenHandler().CreateToken(tokenDescriptor)
                );

                return Ok(new
                {
                    token,
                    user_serial = user.user_serial,
                    cedula = user.user_id,
                    userName = user.user_userName,
                    nombre = user.user_firstName,
                    segundoNombre = user.user_middleName,
                    apellido = user.user_lastName,
                    segundoApellido = user.user_secondLastName,
                    email = user.user_email,
                    roles = user.UserRoles.Select(r => new
                    {
                        roleSerial = r.Role!.role_serial,
                        roleName = r.Role.role_name
                    })
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

    }
}
