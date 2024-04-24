using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ConsoleTrainBooking.Data;
using ConsoleTrainBooking.Models;
using Azure;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using static Azure.Core.HttpHeader;
using Microsoft.IdentityModel.Tokens;

namespace CustomerTrainBooking.Pages.CustomerTrains
{
    public class CreateModel : PageModel
    {
        private readonly ConsoleTrainBooking.Data.CustomerTrainContext _context;

        public CreateModel(ConsoleTrainBooking.Data.CustomerTrainContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {  //Load list for selection list
            ViewData["CustomerID"] = new SelectList(_context.Customer, "CustomerID", "CustomerID");
            ViewData["TrainID"] = new SelectList(_context.Train, "TrainID", "TrainID");
            GetCustomerNameList();
            GetTimeTrainList();
            GetSeatTypeList();
            GetSeatNumberList();
            ViewData["CustomerName"] = new SelectList(CustomerNameList);
            ViewData["TimeDestination"] = new SelectList(TimeDestinationList);
           //mw ViewData["SeatType"] = new SelectList(SeatTypeList);
            
            return Page();
        }

        [BindProperty]
        public CustomerTrain CustomerTrain { get; set; } = default!;
        public IList<string>? TimeDestinationList { get; set; }
        public IList<string>? SeatTypeList { get; set; }
        public IList<string>? SeatNumberList { get; set; }

        public IList<string>? CustomerNameList { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SearchCustomer { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SearchDestination { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SearchSeatType { get; set; }

        [BindProperty(SupportsGet = true)]
        public string[]? SearchSeatNumber { get; set; }
        public string? CustomerSearch { get; set; }
        public string? TimeDestinationSearch { get; set; }
        public string? SeatTypeSearch { get; set; }
        public string[]? SeatNumberSearch { get; set; }
        public object SeatType { get; private set; }

       // [BindProperty(SupportsGet = true)]
        public string[]? SaveTrainID { get; set; } = null;

        public static string? SaveCustID { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            PopulateKeyIds();

            for (int x = 0; x < SaveTrainID.Count(); x++)
            {
                //Fill CustomerTrain info (TrainID , CustomerID, Customer ,Train, set seat Availbe = N for insert required
                CustomerTrain CustomerTrain = new CustomerTrain();
                CustomerTrain.TrainID = SaveTrainID[x];
                CustomerTrain.CustomerID = SaveCustID;
                Customer CustInsert = _context.Customer.First(x => x.CustomerID == SaveCustID);
                Train TrainInsert = _context.Train.First(c => c.TrainID == SaveTrainID[x]);
                TrainInsert.Available = "N";
                CustomerTrain.Customer = CustInsert;
                CustomerTrain.Train = TrainInsert;

                _context.CustomerTrain.Add(CustomerTrain);
            }
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
  
        }


        //}
        public async Task GetCustomerNameList()
        {   //Get CustomerID, first name, last name from customer table
            // Use LINQ to get list of Destination.
            IQueryable<string> CustomerTrainQuery = (from m in _context.Customer
                                                     orderby m.FirstName
                                                     select (m.CustomerID + " " + m.FirstName + " " + m.LastName)).Distinct();

            CustomerNameList = (CustomerTrainQuery.ToList());
            CustomerSearch = SearchCustomer;

        }
        public async Task GetTimeTrainList()
        {  //Get Departure time, destination from Train table
            // Use LINQ to get list of Destination.
            IQueryable<string> CustomerTrainQuery = (from m in _context.Train
                                                     orderby m.DepartureTime
                                                     select (m.DepartureTime + " " + m.Destination)).Distinct();
            TimeDestinationList = CustomerTrainQuery.ToList();
            TimeDestinationSearch = SearchDestination;
        }

        public async Task GetSeatTypeList()
        {
            // Use LINQ to get list of Destination. aisle, window, " "
            IQueryable<string> CustomerTrainQuery = (from m in _context.Train
                                                     orderby m.SeatType
                                                     select m.SeatType).Distinct();
            SeatTypeList = CustomerTrainQuery.ToList();
            SeatTypeList.Add(" ");
            SeatTypeSearch = SearchSeatType;
        }
    

        public async Task GetSeatNumberList()
        { //Get seats by Destination, SeatType and Available
            string strDestination = "";
            if (!string.IsNullOrEmpty(SearchDestination))
            {
                string[] columnsTrainTime = SearchDestination.Split(' ');
                strDestination = columnsTrainTime[1];
            }
            else
            {
                strDestination = SearchDestination;
            }
            // Use LINQ to get list of Destination.
            IQueryable<string> CustomerTrainQuery = (from m in _context.Train
                                                     where m.SeatType.Equals(SearchSeatType)
                                                     && m.Destination.Contains(strDestination)
                                                     && m.Available.Equals("Y")
                                                     orderby m.SeatNumber
                                                     select m.SeatNumber).Distinct();

            if (string.IsNullOrEmpty(SearchDestination))
            {
                SeatNumberList = null;
                { return; }
            }
            if (CustomerTrainQuery.Count() > 0)
            {
                SeatNumberList = CustomerTrainQuery.ToList();
                SeatNumberSearch = SearchSeatNumber;
            }

            else
            {
                IQueryable<string> CustomerTrainQuery2 = (from m in _context.Train
                                                          where m.Available.Equals("Y")
                                                          && m.Destination.Contains(strDestination)
                                                          orderby m.SeatNumber
                                                          select m.SeatNumber).Distinct();
                SeatNumberList = CustomerTrainQuery2.ToList();
                SeatNumberSearch = SearchSeatNumber;
            }
        }
        public async Task PopulateKeyIds()
        { // 1. Customer ID SeedData load 001
          // 2. Train ID 06.05 and seat id 1 , before 0605001 for train unique key
            if (!string.IsNullOrEmpty(SearchCustomer))
            {
                string[] columns = SearchCustomer.Split(" ");
                SaveCustID = columns[0];
            }
            if (!string.IsNullOrEmpty(SearchDestination))
            {
                string[] columnsTrainTime = SearchDestination.Split(' ');
                string columns1 = columnsTrainTime[0];
                string[] columns2 = columns1.Split('.');
                SaveTrainID = new string[SearchSeatNumber.Count()];
                for (int y = 0; y < SearchSeatNumber.Count(); y++)
                {
                    string combineKey = columns2[0] + columns2[1] + SearchSeatNumber[y];
                    SaveTrainID[y] = combineKey; //Form Post
                }
                int x = 99;
            }
        }
    }
}
