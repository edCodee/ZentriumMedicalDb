using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalAPI.Models
{
    public class UserRoleModel
    {
        public int userrole_userserial { get; set; }
        public int userrole_roleserial { get; set; }

        [ForeignKey("userrole_userserial")]
        public UserModel User { get; set; } // Ahora es anulable

        [ForeignKey("userrole_roleserial")]
        public RoleModel Role { get; set; } // Ahora es anulable

        public DateTime assigned_at { get; set; }

        public UserRoleModel() { }

        public UserRoleModel(int userrole_userserial, int userrole_roleserial, UserModel? user, RoleModel? role, DateTime assigned_at)
        {
            this.userrole_userserial = userrole_userserial;
            this.userrole_roleserial = userrole_roleserial;
            User = user;
            Role = role;
            this.assigned_at = assigned_at;
        }
    }
}
