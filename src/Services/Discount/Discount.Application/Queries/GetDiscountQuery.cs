using Discount.Grpc.Protos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Application.Queries
{
    public class GetDiscountQuery : IRequest<CouponModel>
    {
        public string ProductName { get; set; }

        public GetDiscountQuery(string productName)
        {
            ProductName = productName;
        }
    }
}
