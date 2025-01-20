namespace TestsAPI.Model
{
    public class Test
    {
    
    public Guid Id { get; set; } = Guid.NewGuid();
    public string AlgorithmName { get; set; }

    public string FunctionName { get; set; }
    
    public List<int> Parameters { get; set; }

    public string Status { get; set; } = "created";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    

    }
}
