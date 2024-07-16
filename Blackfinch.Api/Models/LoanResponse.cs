namespace Blackfinch.Api.Models;

public class LoanResponse
{
    public bool Success { get; set; }
    
    public int TotalNumberOfApplications { get; set; }
    
    public decimal TotalLoanAmount { get; set; }
    
    public decimal AverageLoanToValue { get; set; }
    
    public IEnumerable<LoanApplication>? Applications { get; set; }
}