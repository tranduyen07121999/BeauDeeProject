using Application.Interfaces;
using Data.DataAccess;
using Data.Entities;
using Data.RequestModels;
using Data.ResponseModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class SvService : ISvService
    {
        private readonly BeauDeeProjectContext _context;
        public SvService(BeauDeeProjectContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel<ServiceResponse>> CreateService(ServiceRequest model)
        {
            var service = new Service
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Status = model.Status,
                Image = model.Image
            };
            var list = new List<ServiceResponse>();
            var message = "blank";
            var status = 500;
            var servicename = await _context.Services.Where(x => x.Name.Equals(service.Name)).FirstOrDefaultAsync();
            if (servicename != null)
            {
                status = 400;
                message = "Service is already exists!";
            }
            else
            {
                message = "Successfully";
                status = 201;
                await _context.Services.AddAsync(service);
                await _context.SaveChangesAsync();
                list.Add(new ServiceResponse
                {
                    Id = service.Id,
                    Name = service.Name,
                    Status = service.Status,
                    Image = service.Image
                });
            }
            return new ResponseModel<ServiceResponse>(list)
            {
                Message = message,
                Status = status,
                Total = list.Count,
                Type = "Service"
            };
        }

        public async Task<ResponseModel<ServiceResponse>> GetAll(PaginationRequest model)
        {
            var services = await _context.Services.Select(s => new ServiceResponse
            {
                Id = s.Id,
                Name = s.Name,
                Status = s.Status,
                Image = s.Image
            }).OrderBy(x => x.Name).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            return new ResponseModel<ServiceResponse>(services)
            {
                Total = services.Count,
                Type = "Services"
            };
        }

        public async Task<ResponseModel<ServiceResponse>> SearchService(PaginationRequest model, string value)
        {
            var services = await _context.Services.Where(s => s.Name.Contains(value)).Select(s => new ServiceResponse
            {
                Id = s.Id,
                Name = s.Name,
                Status = s.Status,
                Image = s.Image
            }).OrderBy(x => x.Name).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            return new ResponseModel<ServiceResponse>(services)
            {
                Total = services.Count,
                Type = "Services"
            };
        }

        public async Task<ResponseModel<ServiceResponse>> UpdateService(Guid id, ServiceRequest model)
        {
            var service = await _context.Services.Where(x => x.Id.Equals(id)).Select(x => new Service
            {
                Id = id,
                Name = model.Name,
                Status = model.Status,
                Image = model.Image
            }).FirstOrDefaultAsync();
            _context.Services.Update(service);
            await _context.SaveChangesAsync();
            var list = new List<ServiceResponse>();
            list.Add(new ServiceResponse
            {
                Id = service.Id,
                Name = service.Name,
                Status = service.Status,
                Image = service.Image
            });
            return new ResponseModel<ServiceResponse>(list)
            {
                Status = 201,
                Total = list.Count,
                Type = "Service"
            };
        }
        public async Task<ResponseModel<ServiceResponse>> DisableService(Guid id)
        {
            var service = await _context.Services.Where(x => x.Id.Equals(id)).Select(x => new Service
            {
                Id = id,
                Name = x.Name,
                Status = "Disable",
                Image = x.Image
            }).FirstOrDefaultAsync();
            _context.Services.Update(service);
            await _context.SaveChangesAsync();
            var list = new List<ServiceResponse>();
            list.Add(new ServiceResponse
            {
                Id = service.Id,
                Name = service.Name,
                Status = service.Status,
                Image = service.Image
            });
            return new ResponseModel<ServiceResponse>(list)
            {
                Status = 201,
                Total = list.Count,
                Type = "Service"
            };
        }
    }
}
