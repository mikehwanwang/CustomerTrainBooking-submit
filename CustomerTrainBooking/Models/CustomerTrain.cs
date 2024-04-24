using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTrainBooking.Models
{
    public class CustomerTrain
    {
        [Display(Name = "Customer ID")]
        public string CustomerID { get; set; }

        [Display(Name = "Train ID")]
        public string TrainID { get; set; }

        //-----------------------------
        //Relationships
        public Customer Customer { get; set; }
        public Train Train { get; set; }

    }
}
