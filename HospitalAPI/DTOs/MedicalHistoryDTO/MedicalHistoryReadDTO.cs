namespace HospitalAPI.DTOs.MedicalHistoryDTO
{
    public class MedicalHistoryReadDTO
    {
        public int medicalHistory_serial { get; set; }
        public int medicalHistory_patientSerial { get; set; }
        public int medicalHistory_doctorSerial { get; set; }
        public int medicalHistory_appointmentSerial { get; set; }
        public string medicalHistory_diagnosis { get; set; } = string.Empty;
        public string medicalHistory_treatment { get; set; } = string.Empty;
        public string medicalHistory_notes { get; set; } = string.Empty;
        public DateTime medicalHistory_createdAt { get; set; }
    }
}
