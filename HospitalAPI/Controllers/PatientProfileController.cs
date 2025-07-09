using APIhospital.Data;
using HospitalAPI.DTOs.PatientProfileDTO;
using HospitalAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalAPI.Controllers
{
    [ApiController]
    [Route("/api[controller]")]
    public class PatientProfileController:ControllerBase
    {
        private readonly AppDbContext _context;
        public PatientProfileController(AppDbContext context)
        {
            _context = context;
        }

        //GET:api/patient/profile
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientProfileReadDTO>>> GetPatientProfiles()
        {
            var patient = await _context.patient_profile.ToListAsync();

            var patientDTO = patient.Select(f => new PatientProfileReadDTO
            {
                PatientProfileSerial = f.patientProfile_serial,
                PatientProfileBloodType = f.patientProfile_bloodType,
                PatientProfileAllergies = f.patientProfile_allergies,
                PatientProfileChronicDiseases = f.patientProfile_chronicDiseases,
                PatientProfileEmergencyContact = f.patientProfile_emergencyContact
            });
            
            return Ok(patientDTO);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreatePatientProfile([FromBody] PatientProfileCreateDTO dto)
        {
            try
            {
                // Obtener el 'user_serial' desde los claims del token
                var userSerialClaim = User.Claims.FirstOrDefault(c => c.Type == "user_serial");
                if (userSerialClaim == null)
                    return Unauthorized("No se pudo obtener el serial del usuario autenticado.");

                int userSerial = int.Parse(userSerialClaim.Value);

                // Crear el nuevo perfil del paciente
                var newProfile = new PatientProfileModel
                {
                    patientProfile_serial = userSerial, // se asocia automáticamente al usuario logueado
                    patientProfile_bloodType = dto.PatientProfileBloodType,
                    patientProfile_allergies = dto.PatientProfileAllergies,
                    patientProfile_chronicDiseases = dto.PatientProfileChronicDiseases,
                    patientProfile_emergencyContact = dto.PatientProfileEmergencyContact
                };

                _context.patient_profile.Add(newProfile);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Perfil del paciente creado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al crear perfil: {ex.Message}");
            }
        }


        [Authorize]
        [HttpGet("exists")]
        public async Task<ActionResult<bool>> GetPatientProfileExists()
        {
            try
            {
                // Extraer 'user_serial' desde el token del usuario autenticado
                var userSerialClaim = User.Claims.FirstOrDefault(c => c.Type == "user_serial");
                if (userSerialClaim == null)
                    return Unauthorized("No se pudo obtener el serial del usuario autenticado.");

                int userSerial = int.Parse(userSerialClaim.Value);

                // Verificar si existe un perfil para este usuario
                bool exists = await _context.patient_profile
                    .AnyAsync(p => p.patientProfile_serial == userSerial);

                return Ok(exists);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al verificar existencia del perfil: {ex.Message}");
            }
        }














    }
}
