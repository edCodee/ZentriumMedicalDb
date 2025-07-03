using HospitalAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;

public class UserRoleModel
{
    public int userRole_userSerial { get; set; }
    public int userRole_roleSerial { get; set; }

    [ForeignKey("userRole_userSerial")]
    public UserModel? User { get; set; }

    [ForeignKey("userRole_roleSerial")]
    public RoleModel? Role { get; set; }

    public DateTime? assigned_at { get; set; }
    public UserRoleModel() { }

    public UserRoleModel(int userRole_userSerial, int userRole_roleSerial, UserModel? user, RoleModel? role, DateTime? assigned_at)
    {
        this.userRole_userSerial = userRole_userSerial;
        this.userRole_roleSerial = userRole_roleSerial;
        User = user;
        Role = role;
        this.assigned_at = assigned_at;
    }
}
