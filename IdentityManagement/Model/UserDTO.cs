namespace IdentityManagement.Model
{
    public class UserDTO                    //Used for Token Authentication
    {
        public string UserName { get; set; }=String.Empty;
        public string Password { get; set; }=String.Empty;
    }
}
