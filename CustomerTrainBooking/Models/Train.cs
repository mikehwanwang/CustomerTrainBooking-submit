using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace ConsoleTrainBooking.Models
{
    public class Train
    {
        [Display(Name = "Train ID")]
        [Key] public string TrainID { get; set; }

        [Display(Name = "Departure Time")]
        public string DepartureTime { get; set; }

        [Display(Name = "Destination")]
        public string Destination { get; set; }

        [Display(Name = "Seat number")]
        public string SeatNumber { get; set; }

        [Display(Name = "Seat Type")]
        public string SeatType { get; set; }

        [Display(Name = "Available")]
        public string Available { get; set; }

        public List<CustomerTrain> CustomerTrainLink { get; set; }
        [NotMapped]
        public string[] TrainSelected { get; set; }
  
    }
}
