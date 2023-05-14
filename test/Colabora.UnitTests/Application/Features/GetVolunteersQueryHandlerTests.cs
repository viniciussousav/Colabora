using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Colabora.Application.Features.Volunteer.GetVolunteers;
using Colabora.Application.Features.Volunteer.GetVolunteers.Models;
using Colabora.Application.Shared;
using Colabora.Domain.Entities;
using Colabora.Domain.Repositories;
using Colabora.TestCommons.Fakers.Domain;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace Colabora.UnitTests.Application.Features;

public class GetVolunteersQueryHandlerTests
{
    private readonly ILogger<GetVolunteersQueryHandler> _logger;
    private readonly IVolunteerRepository _volunteerRepository;
    
    public GetVolunteersQueryHandlerTests()
    {
        _logger = Substitute.For<ILogger<GetVolunteersQueryHandler>>();
        _volunteerRepository = Substitute.For<IVolunteerRepository>();
    }
    
    [Fact(DisplayName = "Given a query, when one or more volunteers exists, then a volunteer list should be returned")]
    public async Task Given_A_Query_When_One_Or_More_Volunteers_Exists_Then_A_Volunteer_List_Should_Be_Returned()
    {
        // Arrange
        var query = new GetVolunteersQuery();

        var response = new List<Volunteer>
        {
            FakeVolunteer.Create(),
            FakeVolunteer.Create(),
            FakeVolunteer.Create(),
        };

        _volunteerRepository.GetAllVolunteers().Returns(response);

        var handler = new GetVolunteersQueryHandler(_logger, _volunteerRepository);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);
        
        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        result.Value.Should().NotBeNull();
        result.Value!.Volunteers.Should().NotBeEmpty();
    }
    
    [Fact(DisplayName = "Given a query, when any volunteer exists, then a volunteer list should be returned")]
    public async Task Given_A_Query_When_Any_Volunteer_Exists_Then_A_Volunteer_List_Should_Be_Returned()
    {
        // Arrange
        var query = new GetVolunteersQuery();

        var response = new List<Volunteer>();

        _volunteerRepository.GetAllVolunteers().Returns(response);

        var handler = new GetVolunteersQueryHandler(_logger, _volunteerRepository);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);
        
        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        result.Value.Should().NotBeNull();
        result.Value!.Volunteers.Should().BeEmpty();
    }
    
    [Fact(DisplayName = "Given a query, when an exception occurs, then an error should be returned")]
    public async Task Given_A_Query_When_An_Exception_Occurs_Then_An_Error_Should_Be_Returned()
    {
        // Arrange
        var query = new GetVolunteersQuery();
        _volunteerRepository.GetAllVolunteers().Throws(new OperationCanceledException("Timeout to database"));

        var handler = new GetVolunteersQueryHandler(_logger, _volunteerRepository);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);
        
        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainEquivalentOf(ErrorMessages.CreateInternalError("Timeout to database"));
        result.Value.Should().BeNull();
    }
}