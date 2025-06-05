using System.ComponentModel.DataAnnotations;

namespace HospitalAPI.Models
{
    public class RoleModel
    {
        [Key]
        public int role_serial { get; set; }
        public string role_name { get; set; } = string.Empty;

        public ICollection<UserRoleModel> UserRoles { get; set; } = new List<UserRoleModel>();

        public RoleModel() { }

        public RoleModel(int role_serial, string role_name)
        {
            this.role_serial = role_serial;
            this.role_name = role_name;
            this.UserRoles = new List<UserRoleModel>();
        }
    }
}
