namespace Blackfinch.Domain.Models;

public record LoanApplication(decimal Amount, decimal LoanToValue, bool Success)
{
}