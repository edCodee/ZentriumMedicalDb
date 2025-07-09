namespace HospitalAPI.DTOs.DoctorProfileDTO
{
    public class DoctorProfileUserReadDTO
    {
        public int UserId { get; set; }
        public string UserFirstName { get; set; }
        public string UserMiddleName { get; set; }
        public string UserLastName { get; set; }
        public string UserSecondLastName { get; set; }
        public DateTime UserBirthDate { get; set; }
        public string Specialty { get; set; }
        public string ProfessionalLicense { get; set; }
        public int YearsExperience { get; set; }

    }
}
