using AutoMapper;
using FluentValidation;
using SampleProjectBackEnd.Application.Common.Results;
using SampleProjectBackEnd.Application.DTOs.Requests;
using SampleProjectBackEnd.Application.DTOs.Responses;
using SampleProjectBackEnd.Application.Interfaces.Repositories;
using SampleProjectBackEnd.Application.Interfaces.Services;
using SampleProjectBackEnd.Domain.Entities;

namespace SampleProjectBackEnd.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<ProductRequestDto> _validator;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IValidator<ProductRequestDto> validator, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task<IDataResult<IEnumerable<ProductResponseDto>>> GetAllAsync()
        {
            var products = await _unitOfWork.Products.GetAllAsync();

            if (!products.Any())
                return new ErrorDataResult<IEnumerable<ProductResponseDto>>("Kayıtlı ürün bulunamadı.");

            var response = _mapper.Map<IEnumerable<ProductResponseDto>>(products);

            return new SuccessDataResult<IEnumerable<ProductResponseDto>>(response, "Ürünler listelendi.");
        }

        public async Task<IDataResult<ProductResponseDto>> GetByIdAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);

            if (product == null)
                return new ErrorDataResult<ProductResponseDto>("Ürün bulunamadı.");

            var response = _mapper.Map<ProductResponseDto>(product);

            return new SuccessDataResult<ProductResponseDto>(response, "Ürün bulundu.");
        }

        public async Task<IDataResult<ProductResponseDto>> CreateAsync(ProductRequestDto dto)
        {
            var validationResult = await _validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return new ErrorDataResult<ProductResponseDto>(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            }

            // Entity'nin constructor'ı olduğu için mapper yerine manual creation ya da ConstructUsing kullanabiliriz.
            // Domain constructor best practice, auto mapper ile karmaşıklaşabilir. 
            // Ancak MappingProfile'da CreateMap<ProductRequestDto, Product>() tanımladık.
            // Basitlik için constructor kullanımı devam edebilir, dönüşte Mapper kullanırız.
            // Veya Mapper'ın ConstructUsing özelliğini kullanabiliriz.
            // Şimdilik Create işleminde Entity Constructor'ı korumak Clean Domain için daha güvenlidir.
            
            var product = new Product(dto.Name, dto.Description, dto.Price, dto.Stock, dto.CategoryId);

            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.CommitAsync();

            var response = _mapper.Map<ProductResponseDto>(product);

            return new SuccessDataResult<ProductResponseDto>(response, "Ürün başarıyla eklendi.");
        }

        public async Task<IDataResult<ProductResponseDto>> UpdateAsync(int id, ProductRequestDto dto)
        {
            var validationResult = await _validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return new ErrorDataResult<ProductResponseDto>(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            }

            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null)
                return new ErrorDataResult<ProductResponseDto>("Güncellenecek ürün bulunamadı.");

            product.SetName(dto.Name);
            product.SetDescription(dto.Description);
            product.SetPrice(dto.Price);
            product.SetStock(dto.Stock);

            await _unitOfWork.Products.UpdateAsync(product);
            await _unitOfWork.CommitAsync();

            var response = _mapper.Map<ProductResponseDto>(product);

            return new SuccessDataResult<ProductResponseDto>(response, "Ürün başarıyla güncellendi.");
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null)
                return new ErrorResult("Silinecek ürün bulunamadı.");

            await _unitOfWork.Products.DeleteAsync(product);
            await _unitOfWork.CommitAsync();
            
            return new SuccessResult("Ürün başarıyla silindi.");
        }
    }
}
