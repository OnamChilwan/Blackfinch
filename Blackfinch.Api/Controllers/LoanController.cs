using Blackfinch.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blackfinch.Api.Controllers;

[ApiController]
public class LoanController : ControllerBase
{
    [HttpPut("applicants/{id}/loans")]
    public async Task<IActionResult> Put([FromRoute] string id, [FromBody] LoanRequest request)
    {
        return new CreatedAtRouteResult(null, null);
    }
}