using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace HospitalAPI.DTOs.PatientProfileDTO
{
    public class PatientProfileReadDTO
    {
        public int PatientProfileSerial { get; set; }
        public string? PatientProfileBloodType { get; set; } = string.Empty;
        public string? PatientProfileAllergies { get; set; } = string.Empty;
        public string? PatientProfileChronicDiseases { get; set; } = string.Empty;
        public string? PatientProfileEmergencyContact { get; set; } = string.Empty;

    }
}
