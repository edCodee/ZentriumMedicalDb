namespace HospitalAPI.DTOs.AppointmentDTO
{
    public class AppointmentReadDTO
    {
        public int AppointmentSerial { get; set; }
        public int AppointmentPatientProfile { get; set; }
        public int AppointmentDoctorProfile { get; set; }
        public DateTime AppointmentScheduledDatetime { get; set; }
        public string? AppointReason { get; set; }
        public string? AppointStatus { get; set; }
        public DateTime? AppointCreatedAt { get; set; }
    }
}
