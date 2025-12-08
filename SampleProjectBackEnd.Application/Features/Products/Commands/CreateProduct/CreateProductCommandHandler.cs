using MediatR;
using SampleProjectBackEnd.Application.Common.Results;
using SampleProjectBackEnd.Application.DTOs.Responses;
using SampleProjectBackEnd.Application.Features.Products.Commands.CreateCommand;
using SampleProjectBackEnd.Application.Interfaces.Repositories;
using SampleProjectBackEnd.Domain.Entities;

namespace SampleProjectBackEnd.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommandHandler
        : IRequestHandler<CreateProductCommand, DataResult<ProductResponseDto>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductCommandHandler(
            IProductRepository productRepository,
            IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<DataResult<ProductResponseDto>> Handle(
            CreateProductCommand request,
            CancellationToken cancellationToken)
        {
            // Domain yeni ürün oluşturma
            Product product = new Product(
                request.Name,
                request.Description,
                request.Price,
                request.Stock
            );

            await _productRepository.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();

            // DTO dönüşü
            var dto = new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock
            };

            return new SuccessDataResult<ProductResponseDto>(dto, "Product created successfully.");
        }
    }
}
