namespace VRPTW.Core;

public class Map
{
    public Dictionary<int, Customer> Customers { get; private set; } = new();

    public double[,] CostMatrix { get; private set; } = new double[,] { { 0 } };

    public int Capacity { get; private set; } = 0;

    public Map(Data data)
    {
        var incomingCustomers = data.Customers;
        Customers = incomingCustomers.ToDictionary(c => c.Id);
        CostMatrix = GetCostMatrix(incomingCustomers);
        Capacity = data.Capacity;
    }

    private static double[,] GetCostMatrix(List<Customer> customers)
    {
        int size = customers.Count;
        var matrix = new double[size, size];
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                matrix[i, j] = matrix[j, i] = customers[i].CostTo(customers[j]);
            }
        }

        return matrix;
    }
}
