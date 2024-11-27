using System.Data.SqlClient;

namespace EcoSyncPickUpAPI.data
{
    public class SQLDB
    {
        public static string Dashboard_Qry = "SELECT * FROM main_dashboard";

        public SQLDB() { }

        public static string GetConnectionString()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                                                       .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                                                       .AddJsonFile("appsettings.json")
                                                       .Build();

            return configuration.GetConnectionString("DefaultConnection");
        }
    }
}
