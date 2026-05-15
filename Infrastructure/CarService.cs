using Dapper;
using Domain;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Datacontext;

public class CarService(DataContext datacontext, ILogger<CarService> logger)
{
    public async Task<List<Cars>> GetAllCarsAsync()
    {
        using var connection = datacontext.GetNpgsqlConnection();
        try
        {
            connection.Open();
        }
        catch (System.Exception)
        {
            logger.LogWarning("connection is not connected!!");
            throw;
        }
        var result = await connection.QueryAsync<Cars>("select * from cars");
        return result.ToList();
    }

    public async Task<Cars?> GetCarsByIdAsync(int id)
    {
        using var connection = datacontext.GetNpgsqlConnection();
        try
        {
            connection.Open();
        }
        catch (System.Exception)
        {
            logger.LogWarning("connection is not connected!!");
            throw;
        }
        var checking = "select * from cars where id = @id";
        var exist = await connection.ExecuteAsync(checking, new { id });
        if (exist == 0)
        {
            logger.LogInformation("Cars Not founded!!");
        }
        return await connection.QuerySingleOrDefaultAsync<Cars>("select * from cars where id = @id", new { id });
    }

    public async Task<bool> CreateCarsAsync(Cars cars)
    {
        using var connection = datacontext.GetNpgsqlConnection();
        try
        {
            connection.Open();
        }
        catch (System.Exception)
        {
            logger.LogWarning("connection is not connected!!");
            throw;
        }
        if (string.IsNullOrWhiteSpace(cars.Model))
        {
            logger.LogWarning("Model can not be empty!!");
            return false;
        }
        if (string.IsNullOrWhiteSpace(cars.Manufacturer))
        {
            logger.LogWarning("can not be empty!!!");
            return false;
        }
        if (cars.Year < 1900 || cars.Year > DateTime.Now.Year)
        {
            logger.LogWarning("Year must be between 1900 and {CurrentYear}", DateTime.Now.Year);
            return false;
        }
        if (cars.PricePerDay <= 0)
        {
            logger.LogWarning("Price per day must be more than 0!!");
            return false;
        }
        await connection.ExecuteAsync("insert into Cars (Model, Manufacturer, Year, PricePerDay) values(@Model, @Manufacturer, @Year, @PricePerDay)", new { cars });
        return true;
    }

     public async Task<bool> UpdateCarsAsync(int id, Cars cars)
    {
        using var connection = datacontext.GetNpgsqlConnection();
        try
        {
            connection.Open();
        }
        catch (System.Exception)
        {
            logger.LogWarning("connection didnt open!");
            throw;
        }
        var checking = "select * from cars where carid = @id";
        var exist = await connection.ExecuteAsync(checking, new { id });
        if (exist == 0)
        {
            logger.LogInformation("not founded!");
            return false;
        }
        await connection.ExecuteAsync("update cars set Model = @Model, Manufacturer = @Manufacturer, Year = @Year, PricePerDay = @PricePerDay", new{cars});
        return false;
    }

    public async Task<bool> DeleteCarsAsync(int id)
    {
         using var connection = datacontext.GetNpgsqlConnection();
        try
        {
            connection.Open();
        }
        catch (System.Exception)
        {
            logger.LogWarning("connection didnt open!");
            throw;
        }
        var checking = "select * from cars where id = @id";
        var exist = await connection.ExecuteAsync(checking, new{id});
        if(exist == 0)
        {
            logger.LogWarning("Cars not founded!!");
        }
        await connection.ExecuteAsync("delete from cars where id = @id", new{id});
    }
}
