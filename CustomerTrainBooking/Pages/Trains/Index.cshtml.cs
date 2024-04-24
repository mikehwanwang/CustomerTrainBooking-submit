using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ConsoleTrainBooking.Data;
using ConsoleTrainBooking.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CustomerTrainBooking.Pages.Trains
{
    public class IndexModel : PageModel
    {
        private readonly ConsoleTrainBooking.Data.CustomerTrainContext _context;

        public IndexModel(ConsoleTrainBooking.Data.CustomerTrainContext context)
        {
            _context = context;
        }

        public IList<Train> Train { get;set; } = default!;

        [BindProperty(SupportsGet = true)] 
        public string? SearchString { get; set; } 

        public IList<string>? DestinationList { get; set; }

        public string? DestinationSearch { get; set; }
        public async Task OnGetAsync()
        {
            await GetDestinationList().ConfigureAwait(false);
        }
        public async Task GetDestinationList()
        {
            // Use LINQ to get list of Destination.
            IQueryable<string> destinationQuery = (from m in _context.Train
                                                   orderby m.Destination
                                                   select m.Destination).Distinct();
            DestinationList = destinationQuery.ToList();
            var trains = from m in _context.Train
                         select m;

            if (!string.IsNullOrEmpty(SearchString))
            {
                trains = trains.Where(s => s.Destination.Contains(SearchString));
            }

            DestinationSearch = SearchString;
            Train = await trains.ToListAsync();
        }
    }
}
