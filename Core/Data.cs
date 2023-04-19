using System.Text.Json;
using System.Text.Json.Serialization;

namespace VRPTW.Core;

public class Data
{
    [JsonPropertyName("instance")]
    public string Instance { get; set; } = "";

    [JsonPropertyName(name: "vehicle-nr")]
    public int VehicleCount { get; set; } = 0;

    [JsonPropertyName(name: "capacity")]
    public int Capacity { get; set; } = 0;

    [JsonPropertyName(name: "customers")]
    public List<Customer> Customers { get; set; } = new();

    public static async Task<Data?> ReadFromJsonFile(string fileName)
    {
        using var fileStream = File.OpenRead(fileName);
        return await JsonSerializer.DeserializeAsync<Data>(fileStream);
    }
}
