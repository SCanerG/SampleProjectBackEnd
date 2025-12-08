using Microsoft.AspNetCore.Identity;

namespace SampleProjectBackEnd.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        // Ek alanlar (Domain'e özgü)
        public string? FullName { get; private set; }
        public bool IsActive { get; private set; } = true;

        // Parametresiz ctor => EF Core & Identity için gerekli
        public ApplicationUser() : base() { }

        // Yeni kullanıcı oluşturma ctor
        public ApplicationUser(string userName, string email, string fullName)
        {
            UserName = userName;
            Email = email;
            FullName = fullName;
        }

        // Davranış: kullanıcı devre dışı bırakma
        public void Deactivate()
        {
            IsActive = false;
        }

        // Davranış: kullanıcı bilgisi güncelleme
        public void UpdateInfo(string? fullName, string? email)
        {
            if (!string.IsNullOrWhiteSpace(fullName))
                FullName = fullName;

            if (!string.IsNullOrWhiteSpace(email))
                Email = email;
        }
    }
}
