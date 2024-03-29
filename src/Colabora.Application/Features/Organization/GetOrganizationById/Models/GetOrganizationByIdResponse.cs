﻿using Colabora.Domain.Shared.Enums;

namespace Colabora.Application.Features.Organization.GetOrganizationById.Models;

public record GetOrganizationByIdResponse(
    Guid OrganizationId,
    string Name,
    string Email,
    States State,
    IEnumerable<Interests> Interests,
    Guid VolunteerCreatorId,
    DateTimeOffset CreatedAt);