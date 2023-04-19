using System.Text.Json.Serialization;

namespace VRPTW.Model;

public class Customer
{
    [JsonPropertyName("cust-nr")]
    public int Id { get; set; } = 0;

    [JsonPropertyName("x")]
    public int X { get; set; } = 0;

    [JsonPropertyName("y")]
    public int Y { get; set; } = 0;

    [JsonPropertyName("demand")]
    public int Demand { get; set; } = 0;

    [JsonPropertyName("earliest")]
    public int ReadyTime { get; set; } = 0;

    [JsonPropertyName("latest")]
    public int DueTime { get; set; } = 0;

    [JsonPropertyName("cost")]
    public int ServiceTime { get; set; } = 0;

    public double CostTo(Customer customer)
    {
        var dx = customer.X - X;
        var dy = customer.Y - Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }

    public override string ToString()
    {
        return Id.ToString();
    }
}
