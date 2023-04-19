namespace VRPTW.Model;

public class RouteBuilder : List<int>
{
    public string Identifier { get => string.Join('-', this); }

    public Route? Route { get; private set; } = null;

    public void CalculateRoute(Map map)
    {
        Route = new Route(this, map);
    }
}
