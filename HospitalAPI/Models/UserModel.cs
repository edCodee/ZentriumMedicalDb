using System.ComponentModel.DataAnnotations;

namespace HospitalAPI.Models
{
    public class UserModel
    {
        [Key]
        public int user_serial { get; set; }
        public byte[]? user_photo { get; set; }
        public string user_id { get; set; } = string.Empty;
        public string user_firstName { get; set; } = string.Empty;
        public string? user_middleName { get; set; }
        public string user_lastName { get; set; } = string.Empty;
        public string? user_secondLastName { get; set; }
        public DateTime user_birthDate { get; set; }
        public string user_userName { get; set; } = string.Empty;
        public string? user_email { get; set; }
        public byte[] user_password { get; set; } = Array.Empty<byte>();
        public ICollection<UserRoleModel> UserRoles { get; set; } = new List<UserRoleModel>();

        public UserModel() { }

        public UserModel(int user_serial, byte[]? user_photo, string user_id, string user_firstName, string? user_middleName, string user_lastName, string? user_secondLastName, DateTime user_birthDate, string user_userName, string? user_email, byte[] user_password, ICollection<UserRoleModel> userRoles)
        {
            this.user_serial = user_serial;
            this.user_photo = user_photo;
            this.user_id = user_id;
            this.user_firstName = user_firstName;
            this.user_middleName = user_middleName;
            this.user_lastName = user_lastName;
            this.user_secondLastName = user_secondLastName;
            this.user_birthDate = user_birthDate;
            this.user_userName = user_userName;
            this.user_email = user_email;
            this.user_password = user_password;
            UserRoles = userRoles;
        }
    }
}
