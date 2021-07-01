using Data.RequestModels;
using Data.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ISvService
    {
        Task<ResponseModel<ServiceResponse>> GetAll(PaginationRequest model);
        Task<ResponseModel<ServiceResponse>> SearchService(PaginationRequest model, string value);
        Task<ResponseModel<ServiceResponse>> CreateService(ServiceRequest model);
        Task<ResponseModel<ServiceResponse>> UpdateService(Guid id, ServiceRequest model);

    }
}
