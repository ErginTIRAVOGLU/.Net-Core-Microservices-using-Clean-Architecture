using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Handlers
{
    public class GetAllTypesHandler : IRequestHandler<GetAllTypesQuery, IList<TypesResponse>>
    {
        private readonly ITypesRepository _typeRepository;

        public GetAllTypesHandler(ITypesRepository typeRepository)
        {
            _typeRepository = typeRepository;
        }

        public async Task<IList<TypesResponse>> Handle(GetAllTypesQuery request, CancellationToken cancellationToken)
        {
            var typeList= await _typeRepository.GetAllTypes();
            var typeResponseList = ProductMapper.Mapper.Map<IList<TypesResponse>>(typeList);
            return typeResponseList;
        }
    }
}
