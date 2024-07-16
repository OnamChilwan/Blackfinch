using Microsoft.AspNetCore.Mvc;

namespace Blackfinch.Api.Models;

public class LoanRequest
{
    [FromRoute]
    public string? Id { get; set; }

    [FromBody]
    public decimal LoanAmount { get; set; }
    
    [FromBody]
    public decimal AssetValue { get; set; }
    
    [FromBody]
    public int CreditScore { get; set; }
}