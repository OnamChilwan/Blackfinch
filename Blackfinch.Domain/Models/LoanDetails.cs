namespace Blackfinch.Domain.Models;

public class LoanDetails
{
    public LoanDetails(string applicantId, decimal amount, decimal assetValue, int creditScore)
    {
        ApplicantId = string.IsNullOrWhiteSpace(applicantId) ? throw new ArgumentException("ApplicantId must not be empty", nameof(applicantId)) : applicantId;
        Amount = amount == 0 ? throw new ArgumentException("Amount must be greater than 0", nameof(amount)) : amount;
        AssetValue = assetValue == 0 ? throw new ArgumentException("Asset value must be greater than 0", nameof(assetValue)) : assetValue;
        CreditScore = creditScore == 0 ? throw new ArgumentException("Credit score must be greater than 0", nameof(creditScore)) : creditScore;
        LoanToValue = decimal.Round(Amount / AssetValue, 2) * 100;
    }
    
    public string ApplicantId { get; }
    
    public decimal Amount { get; }
    
    public decimal AssetValue { get; }
    
    public int CreditScore { get; }
    
    public decimal LoanToValue { get; }
}