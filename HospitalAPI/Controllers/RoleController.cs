using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIhospital.Data;
using APIhospital.Models;
using System.Data;
using Microsoft.Extensions.Logging;  // Asegúrate de tener esta directiva

namespace APIhospital.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<RoleController> _logger;


        public RoleController(AppDbContext context, ILogger<RoleController> logger)
        {
            _context = context;
            _logger = logger;
        }

        //GET: api/role
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleModel>>> GetRoles()
        {
            return await _context.roles.ToListAsync();
        }

        //POST: api/role
        [HttpPost]
        public async Task<ActionResult<RoleModel>> CreateRole(RoleModel role)
        {
            // Verificar si el modelo es válido
            if (role == null)
            {
                return BadRequest("Role data is required.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Verificar si el rol ya existe en la base de datos
                var existingRole = await _context.roles
                    .FirstOrDefaultAsync(r => r.roles_name == role.roles_name);

                if (existingRole != null)
                {
                    return Conflict($"Role with the name '{role.roles_name}' already exists.");
                }

                // Crear nuevo rol
                var newRole = new RoleModel
                {
                    roles_name = role.roles_name,
                    // Si tienes otras propiedades en tu modelo Role (como roles_serial), puedes asignarlas aquí.
                    // Si roles_serial es una clave primaria, generalmente se maneja automáticamente en la base de datos, por lo que no es necesario establecerlo manualmente.
                };

                // Guardar en la base de datos
                _context.roles.Add(newRole);
                await _context.SaveChangesAsync();

                // Responder con el nuevo recurso creado
                return CreatedAtAction(nameof(GetRoles), new { id = newRole.roles_serial}, newRole);
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                _logger.LogError($"Error creating role: {ex.Message}");
                return StatusCode(500, "An unexpected error occurred while creating the role.");
            }
        }


    }
}
