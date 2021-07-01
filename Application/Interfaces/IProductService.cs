using Data.RequestModels;
using Data.ResponseModels;
using System;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IProductService
    {
        Task<ResponseModel<ProductResponse>> GetAll(PaginationRequest model);
        Task<ResponseModel<ProductResponse>> SearchProduct(PaginationRequest model, string value);
        Task<ResponseModel<ProductResponse>> SearchProductByPrice(PaginationRequest model, decimal min, decimal max);
        Task<ResponseModel<ProductResponse>> CreateProduct(ProductRequest model);
        Task<ResponseModel<ProductResponse>> UpdateProduct(Guid id, ProductRequest model);

    }
}
