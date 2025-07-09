using System.ComponentModel.DataAnnotations;

namespace HospitalAPI.Models
{
    public class AppointmentModel
    {
        [Key]
        public int appointment_serial { get; set; }
        public int appointment_patientProfileSerial { get; set; }
        public int appointment_doctorProfileSerial { get; set; }
        public DateTime appointment_scheduledDatetime { get; set; }
        public string? appointment_reason { get; set; }
        public string? appointment_status { get; set; }
        public DateTime? appointment_createdAt { get; set; }

        public AppointmentModel() { }

        public AppointmentModel(int appointment_serial, int appointment_patientProfileSerial, int appointment_doctorProfileSerial, DateTime appointment_scheduledDatetime, string? appointment_reason, string? appointment_status, DateTime? appointment_createdAt)
        {
            this.appointment_serial = appointment_serial;
            this.appointment_patientProfileSerial = appointment_patientProfileSerial;
            this.appointment_doctorProfileSerial = appointment_doctorProfileSerial;
            this.appointment_scheduledDatetime = appointment_scheduledDatetime;
            this.appointment_reason = appointment_reason;
            this.appointment_status = appointment_status;
            this.appointment_createdAt = appointment_createdAt;
        }
    }
}
