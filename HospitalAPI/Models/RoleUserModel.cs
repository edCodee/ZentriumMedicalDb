namespace HospitalAPI.Models
{
    public class RoleUserModel
    {
        public string users_id { get; set; }
        public int users_roleSerial { get; set; }

        public RoleUserModel() { }

        public RoleUserModel(string users_id, int users_roleSerial)
        {
            this.users_id = users_id;
            this.users_roleSerial = users_roleSerial;
        }
    }
}
