using System.ComponentModel.DataAnnotations;

namespace APIhospital.Models
{
    public class RoleModel
    {
        [Key]
        public int roles_serial { get; set; }
        public string roles_name { get; set; }

        public RoleModel()
        {

        }

        public RoleModel(int roles_serial, string roles_name)
        {
            //this.roles_serial = roles_serial;
            this.roles_name = roles_name;
        }
    }
}
