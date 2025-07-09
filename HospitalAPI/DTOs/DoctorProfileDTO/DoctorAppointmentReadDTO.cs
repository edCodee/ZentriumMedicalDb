namespace HospitalAPI.DTOs.DoctorProfileDTO
{
    public class DoctorAppointmentReadDTO
    {
        public int AppointmentSerial { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
        public DateTime ScheduledAt { get; set; }
        public string Reason { get; set; }
        public string StatusAppointment { get; set; }
    }
}
