using System.ComponentModel.DataAnnotations;

namespace HospitalAPI.Models
{
    public class PatientProfileModel
    {
        [Key]
        public int patientProfile_serial { get; set; }
        public string? patientProfile_bloodType { get; set; } = string.Empty;
        public string? patientProfile_allergies { get; set; } = string.Empty;
        public string? patientProfile_chronicDiseases { get; set; } = string.Empty;
        public string? patientProfile_emergencyContact { get; set; } = string.Empty;
        public PatientProfileModel() { }

        public PatientProfileModel(int patientProfile_serial, string? patientProfile_bloodType, string? patientProfile_allergies, string? patientProfile_chronicDiseases, string? patientProfile_emergencyContact)
        {
            this.patientProfile_serial = patientProfile_serial;
            this.patientProfile_bloodType = patientProfile_bloodType;
            this.patientProfile_allergies = patientProfile_allergies;
            this.patientProfile_chronicDiseases = patientProfile_chronicDiseases;
            this.patientProfile_emergencyContact = patientProfile_emergencyContact;
        }
    }
}
