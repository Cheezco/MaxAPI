namespace MaxAPI.Models.Accounts
{
    public class RegisterUser
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        //Hospital
        //Doctor
        public string PersonalCode { get; set; }
    }
}
