namespace HospitalAPI.DTOs.AppointmentDTO
{
    public class AppointmentCreateDTO
    {
        public int AppointmentDoctorProfile { get; set; }
        public DateTime AppointmentScheduledDatetime { get; set; }
        public string AppointReason { get; set; } = string.Empty;
    }
}
