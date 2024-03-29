﻿using Colabora.Application.Commons;
using MediatR;

namespace Colabora.Application.Features.Organization.GetOrganizationById.Models;

public record GetOrganizationByIdQuery(Guid Id) : IRequest<Result<GetOrganizationByIdResponse>>;