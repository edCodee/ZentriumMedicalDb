using APIhospital.Data;
using HospitalAPI.DTOs.RoleDTOs;
using HospitalAPI.DTOs.UserDTOs;
using HospitalAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;
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

        //Hashear password
        private byte[] HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        [HttpGet("role")]
        public async Task<ActionResult<IEnumerable<RoleReadDTOs>>> GetRoles()
        {
            var roles = await _context.role.ToListAsync();

            var rolesDTOs = roles.Select(f => new RoleReadDTOs
            {
                RoleSerial = f.role_serial,
                RoleName = f.role_name
            });
            return Ok(rolesDTOs);
        }

        //GET: api/user
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserReadDTOs>>> GetUsers()
        {
            var users=await _context.user.ToListAsync();

            var userDTOs = users.Select(f => new UserReadDTOs
            {
                UserSerial = f.user_serial,
                UserPhoto = f.user_photo !=null?Convert.ToBase64String(f.user_photo) : null,
                UserId = f.user_id,
                UserFirstName = f.user_firstname,
                UserMiddleName = f.user_middlename,
                UserLastName = f.user_lastname,
                UserSecondLastName = f.user_secondlastname,
                UserBirthDate = f.user_birthdate,
                UserUserName = f.user_username,
                UserEmail = f.user_email
            });

            return Ok(userDTOs);
        }

        // GET: api/User/ByCedula/{cedula}
        [HttpGet("ByCedula/{cedula}")]
        public async Task<ActionResult<UserReadDTOs>> GetUserByCedula(string cedula)
        {
            var user = await _context.user
                .FirstOrDefaultAsync(u => u.user_id == cedula);

            if (user == null)
            {
                return NotFound(new { message = "No se encontró ningún usuario con la cédula proporcionada." });
            }

            var userDTO = new UserReadDTOs
            {
                UserSerial = user.user_serial,
                UserPhoto = user.user_photo != null ? Convert.ToBase64String(user.user_photo) : null,
                UserId = user.user_id,
                UserFirstName = user.user_firstname,
                UserMiddleName = user.user_middlename,
                UserLastName = user.user_lastname,
                UserSecondLastName = user.user_secondlastname,
                UserBirthDate = user.user_birthdate,
                UserUserName = user.user_username,
                UserEmail = user.user_email
            };

            return Ok(userDTO);
        }


        //POST: api/user
        [HttpPost]
        public async Task<ActionResult<UserReadDTOs>> CreateUsers(UserCreateDTOs userDTOs)
        {
            //map dto entity PIlas
            var users = new UserModel
            {
                user_photo = string.IsNullOrWhiteSpace(userDTOs.UserPhoto) ? null : Convert.FromBase64String(userDTOs.UserPhoto),
                user_id = userDTOs.UserId,
                user_firstname = userDTOs.UserFirstName,
                user_middlename = string.IsNullOrWhiteSpace(userDTOs.UserMiddleName) ? null : userDTOs.UserMiddleName,
                user_lastname = userDTOs.UserLastName,
                user_secondlastname = string.IsNullOrWhiteSpace(userDTOs.UserSecondLastName) ? null : userDTOs.UserMiddleName,
                user_birthdate = userDTOs.UserBirthDate,
                user_username = userDTOs.UserUsername,
                user_email = string.IsNullOrWhiteSpace(userDTOs.UserEmail) ? null : userDTOs.UserEmail,
                user_password = HashPassword(userDTOs.UserPassword)
            };

            //Insert user in Db
            _context.user.Add(users);
            await _context.SaveChangesAsync();

            //asigned role for defect
            var defaultRoleId = 3;
            _context.user_role.Add(new UserRoleModel
            {
                userrole_userserial = users.user_serial,
                userrole_roleserial = defaultRoleId,
                assigned_at = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();

            // 4. Preparar DTO de respuesta (sin contraseña)
            var result = new UserReadDTOs
            {
                UserSerial = users.user_serial,
                UserPhoto = users.user_photo != null ? Convert.ToBase64String(users.user_photo) : null,
                UserId = users.user_id,
                UserFirstName = users.user_firstname,
                UserMiddleName = users.user_middlename,
                UserLastName = users.user_lastname,
                UserSecondLastName = users.user_secondlastname,
                UserBirthDate = users.user_birthdate,
                UserUserName = users.user_username,
                UserEmail = users.user_email
            };

            return CreatedAtAction(nameof(GetUsers), new { id = users.user_serial }, result);

        }

        // POST: Api/User/Login
        [HttpPost("Login")]
        public async Task<ActionResult<string>> Login(UserLoginDTOs loginDTO)
        {
            var user = await _context.user.FirstOrDefaultAsync(u => u.user_id == loginDTO.UserId);

            if (user == null)
            {
                return Unauthorized("Usuario no encontrado.");
            }

            // Hasheamos la contraseña ingresada por el usuario
            var hashedInputPassword = HashPassword(loginDTO.UserPassword);

            // Comparamos los bytes de la contraseña almacenada vs. la ingresada
            if (!user.user_password.SequenceEqual(hashedInputPassword))
            {
                return Unauthorized("Contraseña incorrecta.");
            }

            var roles = await _context.user_role
                .Where(ur => ur.userrole_userserial == user.user_serial)
                .Include(ur => ur.Role)
                .Select(ur => new RoleReadDTOs
                {
                    RoleSerial = ur.Role.role_serial,
                    RoleName = ur.Role.role_name,
                }).ToListAsync();

            return Ok(new
            {
                Message = "Login exitoso.",
                UserId = user.user_serial,
                Roles = roles
            });
        }

    }
}
