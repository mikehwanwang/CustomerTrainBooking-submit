using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ConsoleTrainBooking.Data;
using ConsoleTrainBooking.Models;

namespace CustomerTrainBooking.Pages.Trains
{
    public class EditModel : PageModel
    {
        private readonly ConsoleTrainBooking.Data.CustomerTrainContext _context;

        public EditModel(ConsoleTrainBooking.Data.CustomerTrainContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Train Train { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var train =  await _context.Train.FirstOrDefaultAsync(m => m.TrainID == id);
            if (train == null)
            {
                return NotFound();
            }
            Train = train;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Train).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrainExists(Train.TrainID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool TrainExists(string id)
        {
            return _context.Train.Any(e => e.TrainID == id);
        }
    }
}
