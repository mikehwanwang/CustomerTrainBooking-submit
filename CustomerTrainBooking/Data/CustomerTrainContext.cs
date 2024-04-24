using ConsoleTrainBooking.Models;
using Microsoft.EntityFrameworkCore;


namespace ConsoleTrainBooking.Data
{
    public class CustomerTrainContext : DbContext
    {
        public CustomerTrainContext(DbContextOptions<CustomerTrainContext> options)
            : base(options)
        {
        }

        public DbSet<ConsoleTrainBooking.Models.Customer> Customer { get; set; } = default!;
        public DbSet<ConsoleTrainBooking.Models.Train> Train { get; set; } = default!;
        public DbSet<ConsoleTrainBooking.Models.Train> TrainAll { get; set; } = default!;
        public DbSet<ConsoleTrainBooking.Models.CustomerTrain> CustomerTrain { get; set; } = default!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomerTrain>()
                .HasKey(eo => new { eo.CustomerID, eo.TrainID });

        }
    }
}
