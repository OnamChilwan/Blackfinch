namespace Blackfinch.Domain.Events;

public class LoanDeclined(string applicantId, string reason, decimal amount, decimal loanToValue)
{
    public string ApplicantId { get; } = applicantId;
    
    public string Reason { get; } = reason;
    
    public decimal Amount { get; } = amount;
    
    public decimal LoanToValue { get; } = loanToValue;
}