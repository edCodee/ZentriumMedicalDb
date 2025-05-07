using System.ComponentModel.DataAnnotations;

namespace APIhospital.Models
{
    public class UserModel
    {
        [Key]
        public int users_serial { get; set; }
        public string users_id { get; set; }
        public string users_firstName { get; set; }
        public string? users_middleName { get; set; }           // puede ser NULL
        public string users_lastName { get; set; }
        public string? users_secondLastName { get; set; }       // puede ser NULL
        public string users_email { get; set; }
        public string users_userName { get; set; }
        public byte[] users_password { get; set; }
        public DateTime users_dateOfBirth { get; set; }
        public DateTime? users_createdAt { get; set; }          // puede ser NULL
        public DateTime? users_updatedAt { get; set; }          // puede ser NULL
        public int users_roleSerial { get; set; }

        public byte[]? users_photo { get; set; }

        public UserModel() { }

        public UserModel(int users_serial, string users_id, string users_firstName, string? users_middleName, string users_lastName, string? users_secondLastName, string users_email, string users_userName, byte[] users_password, DateTime users_dateOfBirth, DateTime? users_createdAt, DateTime? users_updatedAt, int users_roleSerial, byte[]? users_photo)
        {
            this.users_serial = users_serial;
            this.users_id = users_id;
            this.users_firstName = users_firstName;
            this.users_middleName = users_middleName;
            this.users_lastName = users_lastName;
            this.users_secondLastName = users_secondLastName;
            this.users_email = users_email;
            this.users_userName = users_userName;
            this.users_password = users_password;
            this.users_dateOfBirth = users_dateOfBirth;
            this.users_createdAt = users_createdAt;
            this.users_updatedAt = users_updatedAt;
            this.users_roleSerial = users_roleSerial;
            this.users_photo = users_photo;
        }
    }
}