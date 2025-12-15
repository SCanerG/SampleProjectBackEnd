using AutoMapper;
using SampleProjectBackEnd.Application.DTOs.Requests;
using SampleProjectBackEnd.Application.DTOs.Responses;
using SampleProjectBackEnd.Domain.Entities;

namespace SampleProjectBackEnd.Application.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductResponseDto>();
            CreateMap<ProductRequestDto, Product>();

            CreateMap<Category, CategoryResponseDto>();
            CreateMap<CategoryRequestDto, Category>();
        }
    }
}
