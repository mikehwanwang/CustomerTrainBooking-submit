using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ConsoleTrainBooking.Data;
using ConsoleTrainBooking.Models;

namespace CustomerTrainBooking.Pages.Trains
{
    public class CreateModel : PageModel
    {
        private readonly ConsoleTrainBooking.Data.CustomerTrainContext _context;

        public CreateModel(ConsoleTrainBooking.Data.CustomerTrainContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Train Train { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Train.Add(Train);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
