using Domain.Exceptions;
using SampleProjectBackEnd.Domain.Abstractions;

namespace SampleProjectBackEnd.Domain.Entities
{
    public class Product : BaseEntity<int>   // BaseEntity (Temel Varlık)
    {
        public string Name { get; private set; }             // Ürün adı
        public string Description { get; private set; }      // Ürün açıklaması
        public decimal Price { get; private set; }           // Ürün fiyatı
        public int Stock { get; private set; }
        public int CategoryId { get; private set; }
        public Category Category { get; private set; }

        // Domain constructor (Kurucu metot)
        public Product(string name, string description, decimal price, int stock, int categoryId)
        {
            SetName(name);
            SetDescription(description);
            SetPrice(price);
            SetStock(stock);
            SetCategory(categoryId);

        }

        public void SetCategory(int categoryId)
        {
            if (categoryId <= 0)
                throw new DomainException("CategoryId must be greater than zero.");

            CategoryId = categoryId;
        }
        // ----- Domain Rules (Alan kuralları) -----

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Product name cannot be empty."); // DomainException (Alan hatası)

            Name = name;
        }

        public void SetDescription(string description)
        {
            Description = description?.Trim() ?? "";
        }

        public void SetPrice(decimal price)
        {
            if (price <= 0)
                throw new DomainException("Price must be greater than zero.");

            Price = price;
        }

        public void SetStock(int stock)
        {
            if (stock < 0)
                throw new DomainException("Stock cannot be negative.");

            Stock = stock;
        }
    }
    
   

}
