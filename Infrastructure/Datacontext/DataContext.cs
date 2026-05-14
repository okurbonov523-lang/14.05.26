using Microsoft.Extensions.Logging;
using Npgsql;

namespace Infrastructure.Datacontext;

public class DataContext(Logger<DataContext> logger)
{

    public NpgsqlConnection GetNpgsqlConnection()
    {
    const string connectionString = "host = localhost; Database = postgres; Password = sherowv77";
    logger.LogInformation("getting connection");
        try
        {
            return new NpgsqlConnection(connectionString);
        }
        catch (System.Exception)
        {
            
            throw;
        }
    }
}
