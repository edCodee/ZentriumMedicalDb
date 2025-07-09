namespace HospitalAPI.DTOs.DoctorProfileDTO
{
    public class DoctorProfileReadNameDTO
    {
        public int DoctorProfileSerial { get; set; }
        public string DoctorProfileFirstName { get; set; } = string.Empty;
        public string DoctorProfileLastName { get; set; } = string.Empty;
        public string DoctorProfileSpeciality { get; set; } = string.Empty;
        //public string DoctorProfileProfessionalLicense { get; set; } = string.Empty;
        public int? DoctorProfileYearsExperience { get; set; }
    }
}
