using Blackfinch.Domain.Events;
using Blackfinch.Domain.Exceptions;
using Blackfinch.Domain.Models;

namespace Blackfinch.Domain.Aggregates;

public class ApplicantAggregate
{
    private readonly List<LoanApplication> _applications;

    public ApplicantAggregate()
    {
        _applications = new List<LoanApplication>();
        UncommittedEvents = new List<dynamic>();
    }

    public ApplicantAggregate(IEnumerable<dynamic>? events)
    {
        _applications = new List<LoanApplication>();
        UncommittedEvents = new List<dynamic>();

        foreach (var @event in events ?? new List<dynamic>())
        {
            Apply(@event);
        }
    }

    public void ApplyForLoan(LoanDetails loanDetails)
    {
        const int minValue = 100000;
        const int maxValue = 1500000;

        if (loanDetails.Amount > maxValue || loanDetails.Amount < minValue)
        {
            var e = new LoanDeclined(loanDetails.ApplicantId, $"Loan declined due to amount {loanDetails.Amount} not in the range between {minValue} and {maxValue}", loanDetails.Amount, loanDetails.LoanToValue);
            UncommittedEvents.Add(e);
            Apply(e);
            return;
        }

        const int million = 1000000;
        
        if (loanDetails.Amount >= million)
        {
            if (loanDetails.LoanToValue > 60 || loanDetails.CreditScore < 950)
            {
                var e = new LoanDeclined(loanDetails.ApplicantId, $"Loan declined due loan amount {loanDetails.Amount} exceeding LTV {loanDetails.LoanToValue} and credit score too low {loanDetails.CreditScore}", loanDetails.Amount, loanDetails.LoanToValue);
                UncommittedEvents.Add(e);
                Apply(e);
                return;
            }
        }
        
        if (loanDetails
            is { LoanToValue: < 60, CreditScore: >= 750 }
            or { LoanToValue: < 80, CreditScore: >= 800 }
            or { LoanToValue: < 90, CreditScore: >= 900 })
        {
            var e = new LoanApplicationSucceeded(loanDetails.ApplicantId, loanDetails.Amount, loanDetails.LoanToValue);
            UncommittedEvents.Add(e);
            Apply(e);
            return;
        }

        throw new InvalidLoanApplicationException(loanDetails);
    }

    public int TotalNumberOfApplications()
    {
        return _applications.Count;
    }

    public decimal TotalValueOfLoans()
    {
        return _applications
            .Where(x => x.Success)
            .Sum(x => x.Amount);
    }

    public decimal AverageLoanToValue()
    {
        return _applications.Any(x => x.Success)
            ? _applications.Where(x => x.Success).Average(x => x.LoanToValue)
            : 0;
    }

    private void Apply(LoanApplicationSucceeded e)
    {
        _applications.Add(new LoanApplication(e.Amount, e.LoanToValue, true));
        IsLoanSuccessful = true;
        Id = e.ApplicantId;
    }

    private void Apply(LoanDeclined e)
    {
        _applications.Add(new LoanApplication(e.Amount, e.LoanToValue, false));
        IsLoanSuccessful = false;
        Id = e.ApplicantId;
    }
    
    public string Id { get; private set; }
    
    public bool IsLoanSuccessful { get; private set; }
    
    public List<dynamic> UncommittedEvents { get; }
}