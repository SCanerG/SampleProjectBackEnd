using SampleProjectBackEnd.Application.Common.Results;
using SampleProjectBackEnd.Application.DTOs.Requests;
using SampleProjectBackEnd.Application.DTOs.Responses;
using SampleProjectBackEnd.Application.Interfaces.Repositories;
using SampleProjectBackEnd.Domain.Entities;


namespace SampleProjectBackEnd.Application.Interfaces.Services
    {
        public interface ICategoryService
        {
            Task<IDataResult<IEnumerable<CategoryResponseDto>>> GetAllAsync();
            Task<IDataResult<CategoryResponseDto>> GetByIdAsync(int id);
            Task<IDataResult<CategoryResponseDto>> CreateAsync(CategoryRequestDto dto);
            Task<IDataResult<CategoryResponseDto>> UpdateAsync(int id, CategoryRequestDto dto);
            Task<IResult> DeleteAsync(int id);
        }

}
