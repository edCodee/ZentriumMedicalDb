using APIhospital.Data;
using HospitalAPI.DTOs.MedicalHistoryDTO;
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
    }
}
