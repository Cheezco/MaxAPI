using MaxAPI.Models.Accounts;
using MaxAPI.Models.Doctors;

namespace MaxAPI.Models.Patients
{
    public class Patient : User
    {
        public string PersonalCode { get; set; }
        public List<Vaccination>? Vaccinations { get; set; }
        public List<Diagnosis>? Diagnoses { get; set; }
        public Doctor? Doctor { get; set; }
    }
}
