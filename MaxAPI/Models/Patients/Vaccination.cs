using System.ComponentModel.DataAnnotations.Schema;

namespace MaxAPI.Models.Patients
{
    public class Vaccination
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public DateTime Expiration { get; set; }
        public string Notes { get; set; }
        [NotMapped]
        public int PatientId { get; set; }
    }
}
