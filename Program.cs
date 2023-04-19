using VRPTW.Model;

var fileName = "data.json";
var data = await Data.ReadFromJsonFile(fileName);

// limit the scope of the problem or else there will be waiting
data!.VehicleCount = 4;     
var customerLimit = 5;
data.Customers = data.Customers.Take(customerLimit + 1).ToList();

var solver = new Solver(data, false);
var plans = solver.GenerateValidPlans();

Console.WriteLine("routes\tvCnt\tcost\tdrive\twait");
plans.ForEach(Console.WriteLine);

var bestPlan = plans.First();
Console.WriteLine($"\nBEST PLAN: {bestPlan}");

for (int i = 0; i < bestPlan.Routes!.Count; i++)
{
    var r = bestPlan.Routes[i];
    Console.WriteLine($"route for vehicle #{i}:\t{r}");
}