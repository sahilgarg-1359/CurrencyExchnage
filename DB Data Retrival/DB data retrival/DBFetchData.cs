
using ExchangeAPIservice;
using ExchangeRateInfo;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

using System.Threading.Tasks;


namespace DailyExchangeRateUpdater
{
    public class ExchangeRateDbContext
    {

        private static readonly string connectionString = "Server=tcp:yourserver.database.windows.net,1433;Database=YourDatabase;User Id=YourUser;Password=YourPassword;Encrypt=True;";

        //public DbSet<ExchangeRate> ExchangeRate { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    // Replace with your SQL Server connection string
        //    optionsBuilder.UseSqlServer(AppSettingConfig.Server_Name, AppSettingConfig.Database_Name, AppSettingConfig.User_Id,AppSettingConfig.Password);
        //}

        //public async Task SaveRatesToDatabaseAsync(Dictionary<string, decimal> rates, DateTime date)
        //{
        //    using var context = new ExchangeRateDbContext();

        //    // Create database if not exists
        //    await context.Database.EnsureCreatedAsync();

        //    foreach (var rate in rates)
        //    {
        //        var exchangeRate = new ExchangeRate
        //        {
        //            CurrencyCode = rate.Key,
        //            Rate = rate.Value,
        //            Date = date
        //        };

        //        // Add to the database
        //        await context.ExchangeRate.AddAsync(exchangeRate);
        //    }

        //    // Save changes
        //    await context.SaveChangesAsync();
        //}

        public async Task SaveRatesToDatabaseAsync(Dictionary<string, decimal> rates, DateTime date)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                foreach (var rate in rates)
                {
                    string query = "INSERT INTO Users (Id, Name, Email) VALUES (@Id, @Name, @Email)";
                    using SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@CurrencyCode", rate.Key);
                    cmd.Parameters.AddWithValue("@Rate", rate.Value);
                    cmd.Parameters.AddWithValue("@Date", date);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
