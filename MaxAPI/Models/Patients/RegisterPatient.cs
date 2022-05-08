using MaxAPI.Models.Accounts;

namespace MaxAPI.Models.Patients
{
    public class RegisterPatient : RegisterUser
    {
        public string PersonalCode { get; set; }
        //Hospital
        public int? DoctorId { get; set; }
    }
}
