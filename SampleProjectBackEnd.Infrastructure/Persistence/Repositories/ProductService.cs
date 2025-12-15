using SampleProjectBackEnd.Application.Common.Results;
using SampleProjectBackEnd.Application.DTOs.Requests;
using SampleProjectBackEnd.Application.DTOs.Responses;
using SampleProjectBackEnd.Application.Interfaces.Repositories;
using SampleProjectBackEnd.Application.Interfaces.Services;
using SampleProjectBackEnd.Domain.Entities;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IDataResult<IEnumerable<ProductResponseDto>>> GetAllAsync()
    {
        var products = await _productRepository.GetAllAsync();

        if (!products.Any())
            return new ErrorDataResult<IEnumerable<ProductResponseDto>>("Kayıtlı ürün bulunamadı.");

        var response = products.Select(p => new ProductResponseDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            Stock = p.Stock
        }).ToList();

        return new SuccessDataResult<IEnumerable<ProductResponseDto>>(response, "Ürünler listelendi.");
    }

    public async Task<IDataResult<ProductResponseDto>> GetByIdAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if (product == null)
            return new ErrorDataResult<ProductResponseDto>("Ürün bulunamadı.");

        var response = new ProductResponseDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock
        };

        return new SuccessDataResult<ProductResponseDto>(response, "Ürün bulundu.");
    }

    public async Task<IDataResult<ProductResponseDto>> CreateAsync(ProductRequestDto dto)
    {
        try
        {
            var product = new Product(dto.Name, dto.Description, dto.Price, dto.Stock, dto.CategoryId);

            await _productRepository.AddAsync(product);
            await _productRepository.SaveChangesAsync();

            var response = new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock
            };

            return new SuccessDataResult<ProductResponseDto>(response, "Ürün başarıyla eklendi.");
        }
        catch (Exception ex)
        {
            return new ErrorDataResult<ProductResponseDto>($"Ürün eklenirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<IDataResult<ProductResponseDto>> UpdateAsync(int id, ProductRequestDto dto)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
            return new ErrorDataResult<ProductResponseDto>("Güncellenecek ürün bulunamadı.");

        try
        {
            product.SetName(dto.Name);
            product.SetDescription(dto.Description);
            product.SetPrice(dto.Price);
            product.SetStock(dto.Stock);

            await _productRepository.UpdateAsync(product);
            await _productRepository.SaveChangesAsync();

            var response = new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock
            };

            return new SuccessDataResult<ProductResponseDto>(response, "Ürün başarıyla güncellendi.");
        }
        catch (Exception ex)
        {
            return new ErrorDataResult<ProductResponseDto>($"Ürün güncellenirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<IResult> DeleteAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
            return new ErrorResult("Silinecek ürün bulunamadı.");

        try
        {
            await _productRepository.DeleteAsync(product);
            await _productRepository.SaveChangesAsync();
            return new SuccessResult("Ürün başarıyla silindi.");
        }
        catch (Exception ex)
        {
            return new ErrorResult($"Ürün silinirken hata oluştu: {ex.Message}");
        }
    }

}
