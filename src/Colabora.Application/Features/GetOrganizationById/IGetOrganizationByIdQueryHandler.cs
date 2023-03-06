﻿using Colabora.Application.Commons;
using Colabora.Application.Features.GetOrganizationById.Models;
using MediatR;

namespace Colabora.Application.Features.GetOrganizationById;

public interface IGetOrganizationByIdQueryHandler : IRequestHandler<GetOrganizationByIdQuery, Result<GetOrganizationByIdResponse>>
{
    
}