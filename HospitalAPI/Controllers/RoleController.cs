using APIhospital.Data;
using HospitalAPI.DTOs.RoleDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace HospitalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController:ControllerBase
    {
        private readonly AppDbContext _context;

        public RoleController(AppDbContext context)
        {
            _context = context;
        }

        //GET: api/role
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleReadDTOs>>> GetRoles()
        {
            var roles = await _context.role.ToListAsync();

            var roleDTOs = roles.Select(f => new RoleReadDTOs
            {
                RoleSerial = f.role_serial,
                RoleName = f.role_name
            });

            return Ok(roleDTOs);
        }
    }
}
