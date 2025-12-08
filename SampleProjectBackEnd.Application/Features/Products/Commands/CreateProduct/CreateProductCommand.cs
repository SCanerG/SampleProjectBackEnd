using MediatR;
using SampleProjectBackEnd.Application.Common.Results;
using SampleProjectBackEnd.Application.DTOs.Responses;

namespace SampleProjectBackEnd.Application.Features.Products.Commands.CreateCommand
{
    public class CreateProductCommand : IRequest<DataResult<ProductResponseDto>>
    {
        public string Name { get; set; }           // Ürün adı
        public string Description { get; set; }    // Ürün açıklaması
        public decimal Price { get; set; }         // Fiyat
        public int Stock { get; set; }             // Stok
    }
}

