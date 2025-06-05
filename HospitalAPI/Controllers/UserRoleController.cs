using APIhospital.Data;
using HospitalAPI.DTOs.UserRoleDTOs;
using HospitalAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserRoleController:ControllerBase
    {
        private readonly AppDbContext _context;

        public UserRoleController(AppDbContext context)
        {
            _context = context;
        }

        //GET: api/userrole
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserRoleReadDTOs>>> GetUserRoles()
        {
            var userRoles = await _context.user_role.ToListAsync();

            var userRoleDTOs = userRoles.Select(f => new UserRoleReadDTOs
            {
                UserRoleUserSerial = f.userrole_userserial,
                UserRoleRoleSerial = f.userrole_roleserial,
                UserRoleAssignedAt = f.assigned_at
            });

            return Ok(userRoleDTOs);
        }

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<UserRoleReadDTOs>>> GetUserRoles()
        //{
        //    var userRoles = await _context.userRole
        //        .Include(ur => ur.User)
        //        .Include(ur => ur.Role)
        //        .ToListAsync();

        //    var userRoleDTOs = userRoles.Select(f => new UserRoleReadDTOs
        //    {
        //        UserRole_UserSerial = f.userrole_userserial,
        //        UserRole_RoleSerial = f.userrole_roleserial,
        //        UserRoleAssigned_at = f.assigned_at
        //    });

        //    return Ok(userRoleDTOs);
        //}

        //POST: api/userrole
        [HttpPost]
        public async Task<ActionResult<UserRoleReadDTOs>> CreateUserRole([FromBody] UserRoleCreateDTOs userRoleDTOs)
        {
            // Validación opcional: evitar duplicados
            var exists = await _context.user_role.FindAsync(userRoleDTOs.UserRoleUserSerial, userRoleDTOs.UserRoleRoleSerial);
            if (exists != null)
            {
                return Conflict("Este rol ya está asignado al usuario.");
            }

            var userRole = new UserRoleModel
            {
                userrole_userserial = userRoleDTOs.UserRoleUserSerial,
                userrole_roleserial = userRoleDTOs.UserRoleRoleSerial,
                assigned_at = userRoleDTOs.UserRoleAssignedAt
            };

            _context.user_role.Add(userRole);
            await _context.SaveChangesAsync();

            var result = new UserRoleReadDTOs
            {
                UserRoleUserSerial = userRole.userrole_userserial,
                UserRoleRoleSerial = userRole.userrole_roleserial,
                UserRoleAssignedAt = userRole.assigned_at
            };

            // Como no hay ID único, devolvemos los dos seriales como identificación
            return CreatedAtAction(nameof(GetUserRoles), new
            {
                userSerial = userRole.userrole_userserial,
                roleSerial = userRole.userrole_roleserial
            }, result);
        }
    }
}
