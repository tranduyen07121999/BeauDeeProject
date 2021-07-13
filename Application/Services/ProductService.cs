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
    public class ProductService : IProductService
    {
        private readonly BeauDeeProjectContext _context;
        public ProductService(BeauDeeProjectContext context)
        {
            _context = context;
        }
        public async Task<ResponseModel<ProductResponse>> GetAll(PaginationRequest model)
        {
            var products = await _context.Products.Select(p => new ProductResponse
            {
                Id = p.Id,
                Service = _context.Services.Where(x => x.Id.Equals(p.ServiceId)).Select(x => x.Name).FirstOrDefault(),
                Name = p.Name,
                Brand = p.Brand,
                Price = p.Price,
                Image = p.Image,
                Status = p.Status,
                Expiration = p.Expiration
            }).OrderBy(x => x.Expiration).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            return new ResponseModel<ProductResponse>(products)
            {
                Total = products.Count,
                Type = "Products"
            };
        }

        public async Task<ResponseModel<ProductResponse>> SearchProduct(PaginationRequest model, string value)
        {
            var products = await _context.Products.Where(p => p.Name.Contains(value) || p.Brand.Contains(value) || p.Service.Name.Contains(value)).Select(p => new ProductResponse
            {
                Id = p.Id,
                Service = _context.Services.Where(x => x.Id.Equals(p.ServiceId)).Select(x => x.Name).FirstOrDefault(),
                Name = p.Name,
                Brand = p.Brand,
                Price = p.Price,
                Image = p.Image,
                Status = p.Status,
                Expiration = p.Expiration
            }).OrderBy(x => x.Expiration).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            return new ResponseModel<ProductResponse>(products)
            {
                Total = products.Count,
                Type = "Products"
            };
        }

        public async Task<ResponseModel<ProductResponse>> SearchProductByPrice(PaginationRequest model, decimal min, decimal max)
        {
            var products = await _context.Products.Where(p => p.Price >= min && p.Price <= max).Select(p => new ProductResponse
            {
                Id = p.Id,
                Service = _context.Services.Where(x => x.Id.Equals(p.ServiceId)).Select(x => x.Name).FirstOrDefault(),
                Name = p.Name,
                Brand = p.Brand,
                Price = p.Price,
                Image = p.Image,
                Status = p.Status,
                Expiration = p.Expiration
            }).OrderBy(x => x.Expiration).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            return new ResponseModel<ProductResponse>(products)
            {
                Total = products.Count,
                Type = "Products"
            };
        }

        public async Task<ResponseModel<ProductResponse>> CreateProduct(ProductRequest model)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                ServiceId = await _context.Services.Where(x => x.Name.Equals(model.Service)).Select(x => x.Id).FirstOrDefaultAsync(),
                Name = model.Name,
                Brand = model.Brand,
                Price = model.Price,
                Image = model.Image,
                Status = model.Status,
                Expiration = model.Expiration
            };
            var list = new List<ProductResponse>();
            var message = "blank";
            var status = 500;
            var servicename = await _context.Products.Where(x => x.Name.Equals(product.Name)).FirstOrDefaultAsync();
            if (servicename != null)
            {
                status = 400;
                message = "Product is already exists!";
            }
            else
            {
                message = "Successfully";
                status = 201;
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
                list.Add(new ProductResponse
                {
                    Id = product.Id,
                    Service = _context.Services.Where(x => x.Id.Equals(product.ServiceId)).Select(x => x.Name).FirstOrDefault(),
                    Name = product.Name,
                    Brand = product.Brand,
                    Price = product.Price,
                    Image = product.Image,
                    Status = product.Status,
                    Expiration = product.Expiration,
                });
            }
            return new ResponseModel<ProductResponse>(list)
            {
                Message = message,
                Status = status,
                Total = list.Count,
                Type = "Product"
            };
        }
        public async Task<ResponseModel<ProductResponse>> UpdateProduct(Guid id, ProductRequest model)
        {
            var product = await _context.Products.Where(x => x.Id.Equals(id)).Select(x => new Product
            {
                Id = id,
                Name = model.Name,
                Brand = model.Brand,
                Price = model.Price,
                ServiceId = _context.Services.Where(x => x.Name.Equals(model.Service)).Select(x => x.Id).FirstOrDefault(),
                Image = model.Image,
                Status = model.Status,
                Expiration = model.Expiration
            }).FirstOrDefaultAsync();
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            var list = new List<ProductResponse>();
            list.Add(new ProductResponse
            {
                Id = product.Id,
                Service = _context.Services.Where(x => x.Id.Equals(product.ServiceId)).Select(x => x.Name).FirstOrDefault(),
                Name = product.Name,
                Brand = product.Brand,
                Price = product.Price,
                Image = product.Image,
                Status = product.Status,
                Expiration = product.Expiration,
            });
            return new ResponseModel<ProductResponse>(list)
            {
                Status = 201,
                Total = list.Count,
                Type = "Product"
            };
        }


    }
}
