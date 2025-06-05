using System.ComponentModel.DataAnnotations;

namespace HospitalAPI.Models
{
    public class UserModel
    {
        [Key]
        public int user_serial { get; set; }
        public byte[]? user_photo { get; set; }
        public string user_id { get; set; } = string.Empty;
        public string user_firstname { get; set; } = string.Empty;
        public string? user_middlename { get; set; }
        public string user_lastname { get; set; } = string.Empty;
        public string? user_secondlastname { get; set; }
        public DateTime user_birthdate { get; set; }
        public string user_username { get; set; } = string.Empty;
        public string? user_email { get; set; }
        public byte[] user_password { get; set; } = Array.Empty<byte>();
        public ICollection<UserRoleModel> UserRoles { get; set; } = new List<UserRoleModel>();

        public UserModel() { }

        public UserModel(int user_serial, byte[]? user_photo, string user_id, string user_firstname, string? user_middlename, string user_lastname, string? user_secondlastname, DateTime user_birthdate, string user_username, string user_email, byte[] user_password)
        {
            this.user_serial = user_serial;
            this.user_photo = user_photo;
            this.user_id = user_id;
            this.user_firstname = user_firstname;
            this.user_middlename = user_middlename;
            this.user_lastname = user_lastname;
            this.user_secondlastname = user_secondlastname;
            this.user_birthdate = user_birthdate;
            this.user_username = user_username;
            this.user_email = user_email;
            this.user_password = user_password;
            this.UserRoles = new List<UserRoleModel>();
        }
    }
}
