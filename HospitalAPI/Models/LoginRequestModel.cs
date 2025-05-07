namespace HospitalAPI.Models
{
    public class LoginRequestModel
    {
        public string users_id { get; set; }
        public string users_password { get; set; }

        public LoginRequestModel() { }

        public LoginRequestModel(string users_id, string users_password)
        {
            this.users_id = users_id;
            this.users_password = users_password;
        }
    }
}
