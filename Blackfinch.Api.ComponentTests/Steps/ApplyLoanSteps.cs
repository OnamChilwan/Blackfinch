using System.Net;
using System.Net.Http.Json;
using Blackfinch.Api.Models;
using Blackfinch.Domain.Aggregates;
using Blackfinch.Domain.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace Blackfinch.Api.ComponentTests.Steps;

public class ApplyLoanSteps
{
    private readonly TestServer _server = new(new WebHostBuilder().UseStartup<TestStartup>());
    private HttpResponseMessage _httpResponse = null!;
    private LoanResponse? _result;

    public void ApplicantDoesNotExist(string id)
    {
        _server.Services
            .GetRequiredService<IDomainRepository>()
            .Load(id)
            .Returns(new ApplicantAggregate());
    }

    public void ApplicantExists(string id, ApplicantAggregate aggregate)
    {
        _server.Services
            .GetRequiredService<IDomainRepository>()
            .Load(id)
            .Returns(aggregate);
    }
    
    public async Task RequestIsSent(string id, LoanRequest request)
    {
        var client = _server.CreateClient();
        _httpResponse = await client.PutAsJsonAsync($"/applicants/{id}/loans", request);
    }
    
    public void LoanResponseIsCorrect(LoanResponse response)
    {
        _result.Should().BeEquivalentTo(response);
    }

    public async Task CreatedResponseIsReturned()
    {
        _httpResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        _result = await _httpResponse.Content.ReadFromJsonAsync<LoanResponse>();
    }
    
    public async Task UnprocessableResponseIsReturned()
    {
        _httpResponse.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
        _result = await _httpResponse.Content.ReadFromJsonAsync<LoanResponse>();
    }
    
    public void BadRequestResponseIsReturned()
    {
        _httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    public void AggregateIsSaved(string id)
    {
        _server.Services
            .GetRequiredService<IDomainRepository>()
            .Received(1)
            .Save(id, Arg.Any<ApplicantAggregate>());
    }
    
    public void DomainRepositoryIsNotCalled()
    {
        _server.Services
            .GetRequiredService<IDomainRepository>()
            .Received(0)
            .Load(Arg.Any<string>());
    }
}