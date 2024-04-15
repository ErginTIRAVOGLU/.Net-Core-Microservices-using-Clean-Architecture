using Catalog.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Queries
{
    public class GetProductByBrandQuery:IRequest<List<ProductResponse>>
    {
        public string BrandName { get; set; }

        public GetProductByBrandQuery(string brandName)
        {
            BrandName = brandName;
        }
    }
}
