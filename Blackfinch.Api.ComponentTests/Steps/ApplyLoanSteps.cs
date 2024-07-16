using System.Net;
using System.Net.Http.Json;
using Blackfinch.Api.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace Blackfinch.Api.ComponentTests.Steps;

public class ApplyLoanSteps
{
    private readonly TestServer _server = new(new WebHostBuilder().UseStartup<TestStartup>());
    private HttpResponseMessage _httpResponse = null!;
    private LoanRequest _request = null!;

    public void RequestOf(LoanRequest request)
    {
        _request = request;
    }
    
    public async Task RequestIsSent(string id)
    {
        var client = _server.CreateClient();
        _httpResponse = await client.PutAsJsonAsync($"/applicants/{id}/loans", _request);
    }

    public void CreatedResponseIsReturned()
    {
        _httpResponse.StatusCode.Should().Be(HttpStatusCode.Created);
    }
    
    public void BadRequestResponseIsReturned()
    {
        _httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}