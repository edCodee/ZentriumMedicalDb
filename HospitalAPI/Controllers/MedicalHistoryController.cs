using APIhospital.Data;
using HospitalAPI.DTOs.MedicalHistoryDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalAPI.Controllers
{
    [ApiController]
    [Route("/api[controller]")]
    public class MedicalHistoryController:ControllerBase
    {
        private readonly AppDbContext _context;
        public MedicalHistoryController(AppDbContext context)
        {
            _context = context;
        }

        //GET:api/historymedical
        public async Task<ActionResult<IEnumerable<MedicalHistoryReadDTO>>> GetMedicalHistorys()
        {
            var medical=await _context.medical_history.ToListAsync();

            var medicalDTO = medical.Select(f => new MedicalHistoryReadDTO
            {
                medicalHistory_serial = f.medicalHistory_serial,
                medicalHistory_patientSerial = f.medicalHistory_patientSerial,
                medicalHistory_doctorSerial = f.medicalHistory_doctorSerial,
                medicalHistory_appointmentSerial = f.medicalHistory_appointmentSerial,
                medicalHistory_diagnosis = f.medicalHistory_diagnosis,
                medicalHistory_treatment = f.medicalHistory_treatment,
                medicalHistory_notes = f.medicalHistory_notes,
                medicalHistory_createdAt = f.medicalHistory_createdAt
            });

            return Ok(medicalDTO);
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateMedicalHistory([FromBody] MedicalHistoryCreateDTO dto)
        {
            try
            {
                // Obtener el serial del doctor autenticado
                var doctorSerialClaim = User.Claims.FirstOrDefault(c => c.Type == "user_serial");
                if (doctorSerialClaim == null)
                    return Unauthorized("No se pudo obtener el serial del usuario autenticado.");

                int doctorSerial = int.Parse(doctorSerialClaim.Value);

                // Validación básica: opcional, pero recomendado
                var appointmentExists = await _context.appointment
                    .AnyAsync(a => a.appointment_serial == dto.MedicalHistoryAppointmentSerial &&
                                   a.appointment_doctorProfileSerial == doctorSerial &&
                                   a.appointment_patientProfileSerial == dto.MedicalHistoryPatientSerial);

                if (!appointmentExists)
                    return BadRequest("La cita no corresponde al paciente y/o al doctor autenticado.");

                // Crear historial médico
                var history = new Models.MedicalHistoryModel
                {
                    medicalHistory_patientSerial = dto.MedicalHistoryPatientSerial,
                    medicalHistory_doctorSerial = doctorSerial,
                    medicalHistory_appointmentSerial = dto.MedicalHistoryAppointmentSerial,
                    medicalHistory_diagnosis = dto.MedicalHistoryDiagnosis,
                    medicalHistory_treatment = dto.MedicalHistoryTreatment,
                    medicalHistory_notes = dto.MedicalHistoryNotes,
                    medicalHistory_createdAt = DateTime.UtcNow.AddHours(-5) // Ecuador
                };

                _context.medical_history.Add(history);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Historial médico creado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al crear historial médico: {ex.Message}");
            }
        }

    }
}
