using System.Text.Json.Serialization;
using Colabora.Application.Commons;
using Colabora.Domain.Shared.Enums;
using Colabora.Domain.Volunteer;
using MediatR;

namespace Colabora.Application.Features.Volunteer.RegisterVolunteer.Models;

public record RegisterVolunteerCommand : IRequest<Result<RegisterVolunteerResponse>>
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    [JsonIgnore] public string Email { get; init; } = string.Empty;
    public States State { get; init; } = States.Undefined;
    public Gender Gender { get; init; } = Gender.Undefined;
    public IEnumerable<Interests> Interests { get; init; } = Enumerable.Empty<Interests>();
    public DateTimeOffset Birthdate { get; init; } = DateTimeOffset.MinValue;
}