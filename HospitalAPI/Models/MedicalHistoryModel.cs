using System.ComponentModel.DataAnnotations;

namespace HospitalAPI.Models
{
    public class MedicalHistoryModel
    {
        [Key]
        public int medicalHistory_serial { get; set; }
        public int medicalHistory_patientSerial { get; set; }
        public int medicalHistory_doctorSerial { get; set; }
        public int medicalHistory_appointmentSerial { get; set; }
        public string medicalHistory_diagnosis { get; set; }=string.Empty;
        public string medicalHistory_treatment { get; set; }= string.Empty;
        public string medicalHistory_notes { get; set; } = string.Empty;
        public DateTime medicalHistory_createdAt { get; set; }
        public MedicalHistoryModel() { }

        public MedicalHistoryModel(int medicalHistory_serial, int medicalHistory_patientSerial, int medicalHistory_doctorSerial, int medicalHistory_appointmentSerial, string medicalHistory_diagnosis, string medicalHistory_treatment, string medicalHistory_notes, DateTime medicalHistory_createdAt)
        {
            this.medicalHistory_serial = medicalHistory_serial;
            this.medicalHistory_patientSerial = medicalHistory_patientSerial;
            this.medicalHistory_doctorSerial = medicalHistory_doctorSerial;
            this.medicalHistory_appointmentSerial = medicalHistory_appointmentSerial;
            this.medicalHistory_diagnosis = medicalHistory_diagnosis;
            this.medicalHistory_treatment = medicalHistory_treatment;
            this.medicalHistory_notes = medicalHistory_notes;
            this.medicalHistory_createdAt = medicalHistory_createdAt;
        }
    }
}
