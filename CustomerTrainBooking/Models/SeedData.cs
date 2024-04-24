using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using ConsoleTrainBooking.Data;
using ConsoleTrainBooking.Models;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace ConsoleTrainBooking.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {

            using (var context = new CustomerTrainContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<CustomerTrainContext>>()))
            {
                if (context == null || context.Customer == null)
                {
                    throw new ArgumentNullException("Null CustomerTrainContext");
                }

                //Look for any Customer.
                if (context.Customer.Any())
                {
                    return;   // DB has been seeded
                }

                context.Customer.AddRange(
                    new Customer
                    {
                        CustomerID = "001",
                        FirstName = "Jeffery",
                        LastName = "Wells"
                    }
                );
                context.SaveChanges();
            }

            bool bResult = LoadTrains(serviceProvider);

        }

        public static bool LoadTrains(IServiceProvider serviceProvider)
        {
            bool result = false;
            //1. read file
            //2. laod file
            string strTrainID = string.Empty, strDepartureTime = string.Empty, strDestination = string.Empty, strSeatType = string.Empty;
            int intTotalSeatNumber = 0;
            string strCombineTrainID = string.Empty;
            //06.55, Chicago, 12
            //07.15, Portland, 24
            //08.47, Seattle, 48
            //09.15, San Francisco, 6
            //09.38, Los Angeles, 12
            //10.00, San Diego, 48
            //Default file. MAKE SURE TO CHANGE THIS LOCATION AND FILE PATH TO YOUR FILE
            string textFile = "C:\\Temp\\Data\\Train Data.txt";
            if (File.Exists(textFile))
            {
                // Read a text file line by line.
                string[] lines = File.ReadAllLines(textFile);
                foreach (string line in lines)
                {
                    string[] columns = line.Split(',');
                    if (columns.Length > 0)
                    {
                        strDepartureTime = columns[0];
                        strDestination = columns[1];
                        intTotalSeatNumber = int.Parse(columns[2]);

                        string[] columnsTrainTime = strDepartureTime.Split('.');
                        strTrainID = columnsTrainTime[0] + columnsTrainTime[1]; //departuretime witout .

                        using (var contextTrain = new CustomerTrainContext(
                            serviceProvider.GetRequiredService<
                                DbContextOptions<CustomerTrainContext>>()))
                        {
                            for (int i = 1; i <= intTotalSeatNumber; i = i + 1)
                            {
                              strCombineTrainID = strTrainID + i.ToString("000");
                              strSeatType = GetTranSeatType(i);
                              LoadEachTrain(contextTrain, strCombineTrainID, strDepartureTime, strDestination, i.ToString("000"), strSeatType);
                            }
                            contextTrain.SaveChanges();
                        }
                    }
                }
            }

            result = true;

            return result;
        }

        static void LoadEachTrain(CustomerTrainContext context, string inTrainID, string inDepartureTime, string inDestinaton, string inSeatNumber, string inSeatType)
        {
            context.Train.AddRange(
                                    new Train
                                    {
                                        TrainID = inTrainID,
                                        DepartureTime = inDepartureTime,
                                        Destination = inDestinaton,
                                        SeatNumber = inSeatNumber,
                                        SeatType = inSeatType,
                                        Available = "Y"
                                    }
                                );
        }
        static string GetTranSeatType(int SeatNumber)
        {
            int result_remainder = 0;
            string TranSeatType = "window";
            int quotient_ONE = Math.DivRem(SeatNumber, 4, out result_remainder);
            switch (result_remainder)
            {
                case 1:
                    TranSeatType = "window";
                    break;
                case 2:
                    TranSeatType = "aisle";
                    break;
                case 3:
                    TranSeatType = "aisle";
                    break;
                case 4:
                    TranSeatType = "window";
                    break;
                default:
                    break;
            }
            return TranSeatType;
        }

    }
}
