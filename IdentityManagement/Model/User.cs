namespace IdentityManagement.Model
{
    public class User                       //Used for Token Authentication
    {
        public string UserName { get; set; } = String.Empty;
        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

    }
}
