using Blackfinch.Api.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Blackfinch.Api.Controllers;

[ApiController]
public class LoanController(IValidator<LoanRequest> validator) : ControllerBase
{
    [HttpPut("applicants/{id}/loans")]
    public async Task<IActionResult> Put([FromRoute] string id, [FromBody] LoanRequest request)
    {
        var validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return new BadRequestObjectResult(validationResult.Errors);
        }
        
        return new CreatedAtRouteResult(null, null);
    }
}