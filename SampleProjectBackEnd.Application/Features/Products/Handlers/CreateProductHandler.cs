using MediatR;
using SampleProjectBackEnd.Application.Common.Results;
using SampleProjectBackEnd.Application.DTOs.Responses;
using SampleProjectBackEnd.Application.Features.Products.Commands;
using SampleProjectBackEnd.Application.Features.Products.Commands.CreateCommand;
using SampleProjectBackEnd.Application.Interfaces.Repositories;
using SampleProjectBackEnd.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleProjectBackEnd.Application.Features.Products.Handlers
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, IDataResult<ProductResponseDto>>
    {
        private readonly IProductRepository _productRepository;

        public CreateProductHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IDataResult<ProductResponseDto>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var product = new Product(
                    request.Name,
                    request.Description,
                    request.Price,
                    request.Stock,
                    request.CategoryId
                );
                await _productRepository.AddAsync(product);
                await _productRepository.SaveChangesAsync();

                var dto = new ProductResponseDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Stock = product.Stock ,
                    CategoryId = product.CategoryId
                };

                return new SuccessDataResult<ProductResponseDto>(dto, "Ürün başarıyla eklendi.");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<ProductResponseDto>($"Ürün eklenirken hata oluştu: {ex.Message}");
            }
        }
    }
}
