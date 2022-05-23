using MaxAPI.Models.Accounts;

namespace MaxAPI.Models.Doctors
{
    public class Doctor : User
    {
        public string Education { get; set; }
        //hospital
        public DateTime EmploymentDate { get; set; }
    }
}
