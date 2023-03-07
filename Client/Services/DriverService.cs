
using System.Text;
using System.Text.Json;
using FormulaOne.Shared.Models;

namespace FormulaOne.Client.Services;

public class DriverService : IDriverService
{
    private readonly HttpClient _httpClient;
    public DriverService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<Driver?> AddDriver(Driver driver)
    {
        try
        {
            var itemJson = new StringContent(JsonSerializer.Serialize(driver), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/drivers", itemJson);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStreamAsync();
                var addedDriver = await JsonSerializer.DeserializeAsync<Driver>(responseBody, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return addedDriver;
            }

            return null;
        }
        catch (Exception ex)
        {
            Console.Write($"Error:{ex.Message}");
            throw ex;
        }
    }

    public async Task<IEnumerable<Driver>?> All()
    {
        try
        {
            var apiResponse = await _httpClient.GetStreamAsync("api/drivers]");
            var drivers = await JsonSerializer.DeserializeAsync<IEnumerable<Driver>>(apiResponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return drivers;
        }
        catch (Exception ex)
        {
            Console.Write($"Error:{ex.Message}");
            throw ex;
        }
    }

    public async Task<bool> Delete(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/drivers/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.Write($"Error:{ex.Message}");
            throw ex;
        }
    }

    public Task<bool> Delete(Driver driver)
    {
        throw new NotImplementedException();
    }

    public async Task<Driver?> GetDriver(int id)
    {
        try
        {
            var response = await _httpClient.GetStreamAsync($"api/drivers/{id}");
            var driver = await JsonSerializer.DeserializeAsync<Driver>(response, new JsonSerializerOptions()
            {

            });

            return driver;
        }
        catch (Exception ex)
        {
            Console.Write($"Error:{ex.Message}");
            throw ex;
        }
    }

    public async Task<bool> Update(Driver driver)
    {
        try
        {

            var itemJson = new StringContent(JsonSerializer.Serialize(driver), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"api/drivers/{driver.Id}", itemJson);

            return response.IsSuccessStatusCode;


        }
        catch (Exception ex)
        {
            Console.Write($"Error:{ex.Message}");
            throw ex;
        }
    }
}