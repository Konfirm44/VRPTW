namespace VRPTW.Core;

public class Solver
{
    private readonly Data _data;
    private readonly bool _reportProgress;

    public Solver(Data data, bool reportProgress)
    {
        _data = data;
        _reportProgress = reportProgress;
    }

    public List<Plan> GenerateValidPlans()
    {
        var map = new Map(_data);

        var targets = _data.Customers.Skip(1)
            .Select(c => c.Id)
            .ToList();

        var permutations = GetPermutations(targets, targets.Count).ToList();

        var vehicleCount = _data.VehicleCount;
        var sensibleVehicleCount = Math.Min(vehicleCount, targets.Count);

        var plans = GetValidPlans(map, targets.Count, permutations, sensibleVehicleCount)
            .OrderBy(p => p.Cost)
            .ThenBy(p => p.VehicleCount)
            .ToList();
        return plans;
    }

    private static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length)
    {
        return length == 1
            ? list.Select(t => new T[] { t })
            : GetPermutations(list, length - 1)
                .SelectMany(
                    t => list.Where(e => !t.Contains(e)),
                    (t1, t2) => t1.Concat(new T[] { t2 })
                );
    }

    private static List<int[]> GetPartitions(int length, int n)
    {
        List<int[]> partitions = new();
        var mask = new int[length];
        for (int a = 0; a < Math.Pow(n, mask.Length); a++)
        {
            var t = a;
            for (int i = mask.Length - 1; i >= 0; i--)
            {
                mask[i] = t % n;
                t /= n;
            }
            partitions.Add((int[])mask.Clone());
        }
        return partitions;
    }

    private (Dictionary<string, RouteBuilder> routeBuilders, Dictionary<string, Plan> plans) CalculateAllRoutesAndPlans(List<IEnumerable<int>> permutations, int n, List<int[]> partitions)
    {
        var routeBuilders = new Dictionary<string, RouteBuilder>();
        var plans = new Dictionary<string, Plan>();
        var duplicateBuilders = 0;
        var duplicatePlans = 0;

        var index = 0;

        foreach (var perm in permutations)
        {
            var permArr = perm.ToArray();

            if (_reportProgress)
            {
                Console.WriteLine($"{++index}/{permutations.Count}:\t{string.Join(' ', permArr)}");
            }

            foreach (var part in partitions)
            {
                var builders = new RouteBuilder[n];
                for (int i = 0; i < builders.Length; i++)
                {
                    builders[i] = new();
                }

                for (int i = 0; i < part.Length; i++)
                {
                    var whichBuilder = part[i];
                    builders[whichBuilder].Add(permArr[i]);
                }

                var nonEmptyBuilders = builders.Where(b => b.Count != 0).OrderBy(b => b.Identifier).ToList();

                for (int i = 0; i < nonEmptyBuilders.Count; i++)
                {
                    var b = nonEmptyBuilders[i];
                    if (routeBuilders.ContainsKey(b.Identifier))
                    {
                        duplicateBuilders++;
                        nonEmptyBuilders[i] = routeBuilders[b.Identifier];
                    }
                    else
                    {
                        routeBuilders.Add(b.Identifier, b);
                    }
                }

                var plan = new Plan() { RouteBuilders = nonEmptyBuilders };
                var id = plan.Identifier;

                if (plans.ContainsKey(plan.Identifier))
                {
                    duplicatePlans++;
                }
                else
                {
                    plans.Add(plan.Identifier, plan);
                }
            }
        }

        Console.WriteLine($"duplicated plans: {duplicatePlans} out of {duplicatePlans + plans.Count}\nduplicated builders: {duplicateBuilders} out of {duplicateBuilders + routeBuilders.Count}\n");
        return (routeBuilders, plans);
    }

    private List<Plan> GetValidPlans(Map map, int targetCount, List<IEnumerable<int>> permutations, int vehicleCount)
    {
        var partitions = GetPartitions(targetCount, vehicleCount);

        var (routeBuilders, plans) = CalculateAllRoutesAndPlans(permutations, vehicleCount, partitions);

        foreach (var rb in routeBuilders.Values)
        {
            rb.CalculateRoute(map);
        }

        foreach (var p in plans.Values)
        {
            p.Evaluate();
        }

        var validPlans = plans
            .Where(p => p.Value.IsValid)
            .Select(p => p.Value)
            .ToList();
        return validPlans;
    }
}
