namespace VRPTW.Model;

public class Plan
{
    public List<RouteBuilder> RouteBuilders { get; set; } = new();
    public string Identifier { get => string.Join('/', RouteBuilders.Select(b => b.Identifier)); }
    public bool IsValid { get; private set; } = false;
    public List<Route>? Routes { get; private set; } = null;
    public double Cost { get => Routes?.Sum(r => r.DrivingCost + r.WaitingCost) ?? 0; }
    public double DrivingCost { get => Routes?.Sum(r => r.DrivingCost) ?? 0; }
    public double WaitingCost { get => Routes?.Sum(r => r.WaitingCost) ?? 0; }
    public int VehicleCount { get => Routes?.Count ?? 0; }

    public void Evaluate()
    {
        var routes = new List<Route>();

        foreach (var rb in RouteBuilders)
        {
            if (rb.Route is not null)
            {
                routes.Add(rb.Route);
            }
        }
        Routes = routes;

        if (routes.TrueForAll(r => r.IsValid))
        {
            IsValid = true;
        }

    }

    public override string ToString()
    {
        return $"{Identifier}\tv{Routes?.Count}\t{Cost:F}\t{DrivingCost:F}\t{WaitingCost:F}";
    }
}
