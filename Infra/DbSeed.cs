using Domain.Entities;
using Domain.Exceptions;
using Domain.Messages;
using MongoDB.Driver;

namespace Infra;

public class DbSeed
{
    private readonly IMongoCollection<Plan> _collection;

    private List<Plan> planList = new List<Plan> {
        new Plan("7 dias com um custo de R$30,00 por dia", 30, 7, 0.20M, 50),
        new Plan("15 dias com um custo de R$28,00 por dia", 28, 15, 0.40M, 50),
        new Plan("30 dias com um custo de R$22,00 por dia", 22, 30, 0.60M, 50),
        new Plan("45 dias com um custo de R$20,00 por dia", 20, 45, 0, 50),
        new Plan("50 dias com um custo de R$18,00 por dia", 18, 50, 0, 50),
    };

    public DbSeed(IMongoDatabase database)
        => _collection = database.GetCollection<Plan>("plan");

    public void Populate()
    {
        try
        {
            if (_collection.CountDocuments(c => true) == 0)
                _collection.InsertMany(planList);
        }
        catch (Exception ex)
        {
            throw new HttpExceptionCustom(ExceptionMessage.ERROR_500, ExceptionMessage.ERROR_500_MSG, ex);
        }
    }
    
}