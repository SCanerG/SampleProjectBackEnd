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
        private readonly ICategoryRepository _repo;

        public CategoryService(ICategoryRepository repo)
        {
            _repo = repo;
        }

        public async Task<IDataResult<IEnumerable<CategoryResponseDto>>> GetAllAsync()
        {
            var categories = await _repo.GetAllAsync();

            if (!categories.Any())
                return new ErrorDataResult<IEnumerable<CategoryResponseDto>>("Kategori bulunamadı.");

            var response = categories.Select(c => new CategoryResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            }).ToList();

            return new SuccessDataResult<IEnumerable<CategoryResponseDto>>(response);
        }

        public async Task<IDataResult<CategoryResponseDto>> GetByIdAsync(int id)
        {
            var category = await _repo.GetByIdAsync(id);

            if (category == null)
                return new ErrorDataResult<CategoryResponseDto>("Kategori bulunamadı.");

            var response = new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };

            return new SuccessDataResult<CategoryResponseDto>(response);
        }

        public async Task<IDataResult<CategoryResponseDto>> CreateAsync(CategoryRequestDto dto)
        {
            var category = new Category(dto.Name, dto.Description);

            await _repo.AddAsync(category);
            await _repo.SaveChangesAsync();

            var response = new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };

            return new SuccessDataResult<CategoryResponseDto>(response, "Kategori eklendi.");
        }

        public async Task<IDataResult<CategoryResponseDto>> UpdateAsync(int id, CategoryRequestDto dto)
        {
            var category = await _repo.GetByIdAsync(id);
            if (category == null)
                return new ErrorDataResult<CategoryResponseDto>("Kategori bulunamadı.");

            category.SetName(dto.Name);
            category.SetDescription(dto.Description);

            await _repo.UpdateAsync(category);
            await _repo.SaveChangesAsync();

            var response = new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };

            return new SuccessDataResult<CategoryResponseDto>(response, "Kategori güncellendi.");
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            var category = await _repo.GetByIdAsync(id);
            if (category == null)
                return new ErrorResult("Kategori bulunamadı.");

            await _repo.DeleteAsync(category);
            await _repo.SaveChangesAsync();

            return new SuccessResult("Kategori silindi.");
        }

  
    }
}

