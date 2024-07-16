namespace Blackfinch.Domain.Events;

public class LoanApplicationSucceeded(string applicantId, decimal amount, decimal loanToValue)
{
    public string ApplicantId { get; } = applicantId;
    
    public decimal Amount { get; } = amount;

    public decimal LoanToValue { get; } = loanToValue;
}