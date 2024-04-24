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
    public class IndexModel : PageModel
    {
        private readonly ConsoleTrainBooking.Data.CustomerTrainContext _context;

        public IndexModel(ConsoleTrainBooking.Data.CustomerTrainContext context)
        {
            _context = context;
        }

        public IList<CustomerTrain> CustomerTrain { get;set; } = default!;

        public async Task OnGetAsync()
        {
            CustomerTrain = await _context.CustomerTrain
                .Include(c => c.Customer)
                .Include(c => c.Train).ToListAsync();
        }
    }
}
