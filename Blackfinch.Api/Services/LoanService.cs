using Blackfinch.Api.Models;
using Blackfinch.Domain.Models;
using Blackfinch.Domain.Repositories;
using LoanApplication = Blackfinch.Api.Models.LoanApplication;

namespace Blackfinch.Api.Services;

public class LoanService(IDomainRepository repository)
{
    public async Task<LoanResponse> ApplyForLoan(string id, decimal amount, decimal assetValue, int creditScore)
    {
        var aggregate = await repository.Load(id);
        aggregate.ApplyForLoan(new LoanDetails(id, amount, assetValue, creditScore));

        await repository.Save(id, aggregate);

        return new LoanResponse
        {
            Success = aggregate.IsLoanSuccessful,
            TotalNumberOfApplications = aggregate.TotalNumberOfApplications(),
            TotalLoanAmount = aggregate.TotalValueOfLoans(),
            AverageLoanToValue = aggregate.AverageLoanToValue(),
            Applications = aggregate.ApplicationHistory.Select(x => new LoanApplication { Amount = x.Amount, Success = x.Success })
        };
    }
}