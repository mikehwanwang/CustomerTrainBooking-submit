using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ConsoleTrainBooking.Data;
using ConsoleTrainBooking.Models;

namespace CustomerTrainBooking.Pages.CustomerTrains
{
    public class DetailsModel : PageModel
    {
        private readonly ConsoleTrainBooking.Data.CustomerTrainContext _context;

        public DetailsModel(ConsoleTrainBooking.Data.CustomerTrainContext context)
        {
            _context = context;
        }

        public CustomerTrain CustomerTrain { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customertrain = await _context.CustomerTrain.FirstOrDefaultAsync(m => m.CustomerID == id);
            if (customertrain == null)
            {
                return NotFound();
            }
            else
            {
                CustomerTrain = customertrain;
            }
            return Page();
        }
    }
}
