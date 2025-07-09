using APIhospital.Data;
using HospitalAPI.DTOs.DoctorProfileDTO;
using HospitalAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalAPI.Controllers
{
    [ApiController]
    [Route("/api[controller]")]
    public class DoctorProfileController:ControllerBase
    {
        private readonly AppDbContext _context;
        public DoctorProfileController(AppDbContext context)
        {
            _context = context;
        }

        ////GET: api/doctorprofile
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<DoctorProfileReadDTO>>> GetDoctorProfiles()
        //{
        //    var doctor = await _context.doctor_profile.ToListAsync();
        //    var doctorDTO = doctor.Select(f => new DoctorProfileReadDTO
        //    {
        //        DoctorProfileSerial = f.doctorProfile_serial,
        //        DoctorProfileSpeciality = f.doctorProfile_specialty,
        //        DoctorProfileProfessionalLicense = f.doctorProfile_professionalLicense,
        //        DoctorProfileYearsExperience = f.doctorProfile_yearsExperience
        //    });
        //    return Ok(doctorDTO);
        //}

        // POST: api/doctorprofile/{userSerial}
        [HttpPost("{userSerial}")]
        public async Task<ActionResult<DoctorProfileReadDTO>> CreateDoctorProfile(int userSerial, DoctorProfileCreateDTO doctorDTO)
        {
            // validar que el user existe
            var user = await _context.user.FindAsync(userSerial);
            if (user == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            // crear doctorProfile con el mismo id del user
            var doctor = new DoctorProfileModel
            {
                doctorProfile_serial = userSerial, // AQUÍ vinculas
                doctorProfile_specialty = doctorDTO.DoctorProfileSpeciality,
                doctorProfile_professionalLicense = doctorDTO.DoctorProfileProfessionalLicense,
                doctorProfile_yearsExperience = doctorDTO.DoctorProfileYearsExperience
            };

            _context.doctor_profile.Add(doctor);
            await _context.SaveChangesAsync();

            var result = new DoctorProfileReadDTO
            {
                DoctorProfileSerial = doctor.doctorProfile_serial,
                DoctorProfileSpeciality = doctor.doctorProfile_specialty,
                DoctorProfileProfessionalLicense = doctor.doctorProfile_professionalLicense,
                DoctorProfileYearsExperience = doctor.doctorProfile_yearsExperience
            };

            return CreatedAtAction(nameof(GetDoctorProfiles), new { id = doctor.doctorProfile_serial }, result);
        }

        // GET: api/DoctorProfile
        [HttpGet("profiledoctor")]
        public async Task<ActionResult<IEnumerable<DoctorProfileUserReadDTO>>> GetDoctorProfilesWithUser()
        {
            var query = from u in _context.user
                        join d in _context.doctor_profile
                          on u.user_serial equals d.doctorProfile_serial
                        select new DoctorProfileUserReadDTO
                        {
                            UserId = u.user_serial,
                            UserFirstName = u.user_firstName,
                            UserMiddleName = u.user_middleName,
                            UserLastName = u.user_lastName,
                            UserSecondLastName = u.user_secondLastName,
                            UserBirthDate = u.user_birthDate,
                            Specialty = d.doctorProfile_specialty,
                            ProfessionalLicense = d.doctorProfile_professionalLicense,
                            YearsExperience = d.doctorProfile_yearsExperience ?? 0
                        };

            var result = await query.ToListAsync();
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DoctorProfileReadDTO>>> GetDoctorProfiles()
        {
            var resultados = await (
                from d in _context.doctor_profile
                join u in _context.user            // Asumiendo DbSet<User> Users
                    on d.doctorProfile_serial equals u.user_serial
                select new DoctorProfileReadNameDTO
                {
                    DoctorProfileSerial=u.user_serial,
                    DoctorProfileFirstName = u.user_firstName,
                    DoctorProfileLastName = u.user_lastName,
                    DoctorProfileSpeciality = d.doctorProfile_specialty,
                    DoctorProfileYearsExperience = d.doctorProfile_yearsExperience
                }
            ).ToListAsync();

            return Ok(resultados);
        }


        [Authorize]
        [HttpGet("my-appointments")]
        public async Task<IActionResult> GetDoctorAppointments()
        {
            try
            {
                // Obtener el serial del doctor autenticado
                var userSerialClaim = User.Claims.FirstOrDefault(c => c.Type == "user_serial");
                if (userSerialClaim == null)
                    return Unauthorized("No se pudo obtener el serial del usuario autenticado.");

                int doctorSerial = int.Parse(userSerialClaim.Value);

                // Obtener las citas asignadas al doctor con estado "Programada"
                var appointments = await _context.appointment
                    .Where(a => a.appointment_status == "Programada" &&
                                a.appointment_doctorProfileSerial == doctorSerial)
                    .Join(_context.patient_profile,
                          a => a.appointment_patientProfileSerial,
                          p => p.patientProfile_serial,
                          (a, p) => new { a, p })
                    .Join(_context.user,
                          ap => ap.p.patientProfile_serial,
                          u => u.user_serial,
                          (ap, u) => new DoctorAppointmentReadDTO
                          {
                              AppointmentSerial = ap.a.appointment_serial,
                              PatientFirstName = u.user_firstName,
                              PatientLastName = u.user_lastName,
                              ScheduledAt = ap.a.appointment_scheduledDatetime,
                              Reason = ap.a.appointment_reason,
                              StatusAppointment = ap.a.appointment_status
                          })
                    .OrderBy(dto => dto.ScheduledAt)
                    .ToListAsync();

                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener las citas del doctor: {ex.Message}");
            }
        }














    }
}
