namespace HospitalAPI.DTOs.PatientProfileDTO
{
    public class PatientProfileCreateDTO
    {
        public string? PatientProfileBloodType { get; set; } = string.Empty;
        public string? PatientProfileAllergies { get; set; } = string.Empty;
        public string? PatientProfileChronicDiseases { get; set; } = string.Empty;
        public string? PatientProfileEmergencyContact { get; set; } = string.Empty;
    }
}
