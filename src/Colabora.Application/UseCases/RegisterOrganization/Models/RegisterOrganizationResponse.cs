﻿using Colabora.Domain.Enums;

namespace Colabora.Application.UseCases.RegisterOrganization.Models;

public record RegisterOrganizationResponse(
    int OrganizationId,
    string Name,
    States State,
    List<Interests> Interests,
    int CreatedBy,
    DateTime CreatedAt);