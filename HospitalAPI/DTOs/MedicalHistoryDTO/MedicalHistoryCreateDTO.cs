namespace HospitalAPI.DTOs.MedicalHistoryDTO
{
    public class MedicalHistoryCreateDTO
    {
        public int MedicalHistoryPatientSerial { get; set; }
        public int MedicalHistoryAppointmentSerial { get; set; }
        public string MedicalHistoryDiagnosis { get; set; }
        public string MedicalHistoryTreatment { get; set; }
        public string? MedicalHistoryNotes { get; set; }
    }
}
