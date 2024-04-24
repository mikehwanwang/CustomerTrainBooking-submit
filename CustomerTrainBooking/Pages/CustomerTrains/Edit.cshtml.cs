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

namespace CustomerTrainBooking.Pages.CustomerTrains
{
    public class EditModel : PageModel
    {
        private readonly ConsoleTrainBooking.Data.CustomerTrainContext _context;

        public EditModel(ConsoleTrainBooking.Data.CustomerTrainContext context)
        {
            _context = context;
        }

        [BindProperty]
        public CustomerTrain CustomerTrain { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customertrain =  await _context.CustomerTrain.FirstOrDefaultAsync(m => m.CustomerID == id);
            if (customertrain == null)
            {
                return NotFound();
            }
            CustomerTrain = customertrain;
           ViewData["CustomerID"] = new SelectList(_context.Customer, "CustomerID", "CustomerID");
           ViewData["TrainID"] = new SelectList(_context.Train, "TrainID", "TrainID");
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

            _context.Attach(CustomerTrain).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerTrainExists(CustomerTrain.CustomerID))
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

        private bool CustomerTrainExists(string id)
        {
            return _context.CustomerTrain.Any(e => e.CustomerID == id);
        }
    }
}
