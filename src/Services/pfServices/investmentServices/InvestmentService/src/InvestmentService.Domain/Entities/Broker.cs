namespace InvestmentService.Domain.Entities;

public class Broker
{
    public decimal BrokerId { get; set; }
    public string BrokerName { get; set; } = null!;
    public string BrokerStatus { get; set; } = null!;

    public ICollection<Investment> Investments { get; set; } = new List<Investment>();
}
