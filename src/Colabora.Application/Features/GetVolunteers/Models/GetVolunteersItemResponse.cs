﻿using Colabora.Domain.Enums;

namespace Colabora.Application.Features.GetVolunteers.Models;

public record GetVolunteersItemResponse(
    int VolunteerId,
    string FirstName,
    string LastName,
    string Email,
    States State,
    Gender Gender,
    List<Interests> Interests,
    DateTime Birthdate,
    DateTime CreatedAt
);