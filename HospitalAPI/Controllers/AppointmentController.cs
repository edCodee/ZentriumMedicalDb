using APIhospital.Data;
using HospitalAPI.DTOs.AppointmentDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalAPI.Controllers
{
    [ApiController]
    [Route("/api[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly AppDbContext _context;
        public AppointmentController(AppDbContext context)
        {
            _context = context;
        }

        //GET: api/appointment
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppointmentReadDTO>>> GetAppointments()
        {
            var appointment = await _context.appointment.ToListAsync();

            var appointmentDTO = appointment.Select(f => new AppointmentReadDTO
            {
                AppointmentSerial = f.appointment_serial,
                AppointmentPatientProfile = f.appointment_patientProfileSerial,
                AppointmentDoctorProfile = f.appointment_doctorProfileSerial,
                AppointmentScheduledDatetime = f.appointment_scheduledDatetime,
                AppointReason = f.appointment_reason,
                AppointStatus = f.appointment_status,
                AppointCreatedAt = f.appointment_createdAt
            });

            return Ok(appointmentDTO);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateAppointment([FromBody] AppointmentCreateDTO dto)
        {
            try
            {
                // Obtener el serial del usuario autenticado
                var userSerialClaim = User.Claims.FirstOrDefault(c => c.Type == "user_serial");
                if (userSerialClaim == null)
                    return Unauthorized("No se pudo obtener el serial del usuario autenticado.");

                int userSerial = int.Parse(userSerialClaim.Value);

                // Verificar si el paciente tiene un perfil registrado
                var patientProfile = await _context.patient_profile
                    .FirstOrDefaultAsync(p => p.patientProfile_serial == userSerial);

                if (patientProfile == null)
                    return BadRequest("El paciente no tiene un perfil registrado.");

                // Crear la cita médica
                var appointment = new Models.AppointmentModel
                {
                    appointment_patientProfileSerial = patientProfile.patientProfile_serial,
                    appointment_doctorProfileSerial = dto.AppointmentDoctorProfile,
                    appointment_scheduledDatetime = dto.AppointmentScheduledDatetime,
                    appointment_reason = dto.AppointReason,
                    appointment_status = "Programada", // Estado inicial
                    appointment_createdAt = DateTime.UtcNow.AddHours(-5) // Ecuador GMT-5
                };

                _context.appointment.Add(appointment);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Cita médica agendada correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al agendar la cita: {ex.Message}");
            }
        }














    }
}
