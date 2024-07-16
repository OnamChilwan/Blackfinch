using Blackfinch.Domain.Models;

namespace Blackfinch.Domain.Exceptions;

public class InvalidLoanApplicationException(LoanDetails loanDetails) 
    : Exception($"Unable to determine loan outcome from amount {loanDetails.Amount}, asset value {loanDetails.AssetValue}, credit score {loanDetails.CreditScore}, LTV of {loanDetails.LoanToValue}");