using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsoleTrainBooking.Models
{
    public class Customer
    {
        [Display(Name = "Customer ID")]

        [Key][DatabaseGenerated(DatabaseGeneratedOption.None)] public string CustomerID { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }
    }
}
