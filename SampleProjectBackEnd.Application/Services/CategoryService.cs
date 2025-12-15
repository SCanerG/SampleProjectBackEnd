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
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CategoryRequestDto> _validator;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IValidator<CategoryRequestDto> validator, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task<IDataResult<IEnumerable<CategoryResponseDto>>> GetAllAsync()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();

            if (!categories.Any())
                return new ErrorDataResult<IEnumerable<CategoryResponseDto>>("Kategori bulunamadı.");

            var response = _mapper.Map<IEnumerable<CategoryResponseDto>>(categories);

            return new SuccessDataResult<IEnumerable<CategoryResponseDto>>(response);
        }

        public async Task<IDataResult<CategoryResponseDto>> GetByIdAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);

            if (category == null)
                return new ErrorDataResult<CategoryResponseDto>("Kategori bulunamadı.");

            var response = _mapper.Map<CategoryResponseDto>(category);

            return new SuccessDataResult<CategoryResponseDto>(response);
        }

        public async Task<IDataResult<CategoryResponseDto>> CreateAsync(CategoryRequestDto dto)
        {
            var validationResult = await _validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return new ErrorDataResult<CategoryResponseDto>(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            }

            var category = new Category(dto.Name, dto.Description);

            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.CommitAsync();

            var response = _mapper.Map<CategoryResponseDto>(category);

            return new SuccessDataResult<CategoryResponseDto>(response, "Kategori eklendi.");
        }

        public async Task<IDataResult<CategoryResponseDto>> UpdateAsync(int id, CategoryRequestDto dto)
        {
            var validationResult = await _validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return new ErrorDataResult<CategoryResponseDto>(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            }

            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
                return new ErrorDataResult<CategoryResponseDto>("Kategori bulunamadı.");

            category.SetName(dto.Name);
            category.SetDescription(dto.Description);

            await _unitOfWork.Categories.UpdateAsync(category);
            await _unitOfWork.CommitAsync();

            var response = _mapper.Map<CategoryResponseDto>(category);

            return new SuccessDataResult<CategoryResponseDto>(response, "Kategori güncellendi.");
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
                return new ErrorResult("Kategori bulunamadı.");

            await _unitOfWork.Categories.DeleteAsync(category);
            await _unitOfWork.CommitAsync();

            return new SuccessResult("Kategori silindi.");
        }
    }
}
