namespace HospitalAPI.DTOs.DoctorProfileDTO
{
    public class DoctorProfileCreateDTO
    {
        public string DoctorProfileSpeciality { get; set; } = string.Empty;
        public string DoctorProfileProfessionalLicense { get; set; } = string.Empty;
        public int? DoctorProfileYearsExperience { get; set; }
    }
}
