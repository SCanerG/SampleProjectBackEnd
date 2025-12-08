using SampleProjectBackEnd.Application.Common.Results;
using SampleProjectBackEnd.Application.DTOs.Requests;
using SampleProjectBackEnd.Application.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleProjectBackEnd.Application.Interfaces.Services
{
    public interface IProductService
    {
        Task<IDataResult<IEnumerable<ProductResponseDto>>> GetAllAsync();
        Task<IDataResult<ProductResponseDto>> GetByIdAsync(int id);
        Task<IDataResult<ProductResponseDto>> CreateAsync(ProductRequestDto dto);
        Task<IDataResult<ProductResponseDto>> UpdateAsync(int id, ProductRequestDto dto);
        Task<IResult> DeleteAsync(int id);
    }
}
