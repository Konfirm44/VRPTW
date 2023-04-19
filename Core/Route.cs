using System.Text;

namespace VRPTW.Core;

public class Route : List<Customer>
{
    public double DrivingCost { get; private set; } = 0;
    public double WaitingCost { get; private set; } = 0;
    public List<double> Arrivals { get; private set; } = new();
    public List<double> Departures { get; private set; } = new();
    //public List<int> RemainingCapacity { get; private set; } = new();
    public bool IsArrivalTimeValid { get; private set; } = true;
    public bool IsCapacityValid { get; private set; } = true;

    public bool IsValid { get => IsArrivalTimeValid && IsCapacityValid; }

    public List<int> ExpectedFrom { get => this.Select(c => c.ReadyTime).ToList(); }
    public List<int> ExpectedTo { get => this.Select(c => c.DueTime).ToList(); }

    public Route(IEnumerable<int> targets, Map map)
    {
        var depot = map.Customers[0];
        Add(depot);
        foreach (var t in targets)
        {
            Add(map.Customers[t]);
        }
        Add(depot);

        var capacity = map.Capacity;
        //RemainingCapacity.Add(capacity);

        Arrivals.Add(0);

        var firstReadyTime = this[1].ReadyTime;
        var firstDeparture = firstReadyTime - map.CostMatrix[0, 1] > depot.ReadyTime ? firstReadyTime - map.CostMatrix[0, 1] : 0;

        Departures.Add(firstDeparture);
        for (int i = 1; i < Count; i++)
        {
            var previousId = this[i - 1].Id;
            var currentId = this[i].Id;

            var cost = map.CostMatrix[previousId, currentId];
            DrivingCost += cost;

            var arrival = Departures[i - 1] + cost;

            var dueTime = this[i].DueTime;
            if (arrival > dueTime)
            {
                IsArrivalTimeValid = false;
                break;
            }
            Arrivals.Add(arrival);

            var readyTime = this[i].ReadyTime;
            var departure = arrival;
            if (arrival < readyTime)
            {
                departure = readyTime;
                WaitingCost += readyTime - arrival;
            }

            var serviceTime = this[i].ServiceTime;
            departure += serviceTime;
            Departures.Add(departure);

            var demand = this[i].Demand;
            capacity -= demand;
            //RemainingCapacity.Add(capacity);
            if (capacity < 0)
            {
                IsCapacityValid = false;
                break;
            }
        }
    }

    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.Append(string.Join(" ", this));
        //sb.AppendLine(string.Join("\t", Arrivals.Select(a => a.ToString("F"))));
        //sb.AppendLine(string.Join("\t", ExpectedFrom.Select(a => a.ToString("F"))));
        //sb.AppendLine(string.Join("\t", ExpectedTo.Select(d => d.ToString("F"))));
        //sb.AppendLine(string.Join("\t", Departures.Select(d => d.ToString("F"))));
        //sb.AppendLine(string.Join("\t", RemainingCapacity.Select(d => d.ToString("F"))));

        //sb.AppendLine($"\t{IsArrivalTimeValid}");
        //sb.AppendLine($"\t{IsCapacityValid}");
        //sb.Append($"\t{IsValid}");
        return sb.ToString();
    }
}
