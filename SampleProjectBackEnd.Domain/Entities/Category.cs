using SampleProjectBackEnd.Domain.Abstractions;
using Domain.Exceptions;

namespace SampleProjectBackEnd.Domain.Entities
{
    public class Category : BaseEntity<int>
    {
        public string Name { get; private set; }
        public string? Description { get; private set; }

        public Category(string name, string? description)
        {
            SetName(name);
            SetDescription(description);
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Category name cannot be empty.");

            Name = name;
        }

        public void SetDescription(string? description)
        {
            Description = description?.Trim();
        }
    }
}
