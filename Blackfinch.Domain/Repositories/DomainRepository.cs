using Blackfinch.Domain.Aggregates;

namespace Blackfinch.Domain.Repositories;

public interface IDomainRepository
{
    Task<ApplicantAggregate> Load(string id);

    Task Save(string id, ApplicantAggregate aggregate);
}

/* This is a fake persistence repo, ideally this would call something like Cosmos / Mongo / EventStore but to make 
 * make things simple I am returning the instance as opposed to storing the events. I have a unit test on ApplicantAggregateTests
 * demonstrating how the hydration of the aggregate would work by passing the events to the constructor.
 */
public class InMemoryDomainRepository : IDomainRepository
{
    private static readonly Dictionary<string, ApplicantAggregate> Data;

    static InMemoryDomainRepository()
    {
        Data = new Dictionary<string, ApplicantAggregate>();
    }

    public async Task<ApplicantAggregate> Load(string id)
    {
        if (Data.ContainsKey(id))
        {
            return Data[id];
        }

        return new ApplicantAggregate();
    }

    public Task Save(string id, ApplicantAggregate aggregate)
    {
        if (Data.ContainsKey(id))
        {
            Data.Remove(id);
        }
        
        Data.Add(id, aggregate);
        return Task.CompletedTask;
    }
}