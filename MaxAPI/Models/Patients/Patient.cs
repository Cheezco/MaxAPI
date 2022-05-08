using MaxAPI.Models.Accounts;
using MaxAPI.Models.Doctors;

namespace MaxAPI.Models.Patients
{
    public class Patient : User
    {
        public string PersonalCode { get; set; }
        public Doctor? Doctor { get; set; }
    }
}
