using System.ComponentModel.DataAnnotations;

namespace HospitalAPI.Models
{
    public class DoctorProfileModel
    {
        [Key]
        public int doctorProfile_serial { get; set; }
        public string doctorProfile_specialty { get; set; } = string.Empty;
        public string doctorProfile_professionalLicense { get; set; } = string.Empty;
        public int? doctorProfile_yearsExperience { get; set; }

        public DoctorProfileModel() { }

        public DoctorProfileModel(int doctorProfile_serial, string doctorProfile_specialty, string doctorProfile_professionalLicense, int? doctorProfile_yearsExperience)
        {
            this.doctorProfile_serial = doctorProfile_serial;
            this.doctorProfile_specialty = doctorProfile_specialty;
            this.doctorProfile_professionalLicense = doctorProfile_professionalLicense;
            this.doctorProfile_yearsExperience = doctorProfile_yearsExperience;
        }
    }
}
