using Blackfinch.Api.Models;
using Blackfinch.Api.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Blackfinch.Api.Controllers;

[ApiController]
public class LoanController(IValidator<LoanRequest> validator, LoanService loanService) : ControllerBase
{
    [HttpPut("applicants/{id}/loans")]
    public async Task<IActionResult> Put([FromRoute] string id, [FromBody] LoanRequest request)
    {
        var validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return new BadRequestObjectResult(validationResult.Errors);
        }
        
        var result = await loanService.ApplyForLoan(id, request.LoanAmount, request.AssetValue, request.CreditScore);
        
        return result.Success
            ? new CreatedAtRouteResult(null, result) // The locationUri would be set if we had an endpoint where we can retrieve the resource
            : new UnprocessableEntityObjectResult(result);
    }
}