using SampleProjectBackEnd.Domain.Abstractions;

namespace SampleProjectBackEnd.Domain.Entities
{
    public class RefreshToken : BaseEntity<int>
    {
        public string Token { get; private set; } = string.Empty;
        public int UserId { get; private set; } // int olarak güncellendi
        public DateTime Expires { get; private set; }
        public bool Revoked { get; private set; }
        public string? ReplacedByToken { get; private set; }

        private RefreshToken() { } // EF için boş ctor

        public RefreshToken(int userId, string token, DateTime expires)
        {
            if (userId <= 0)
                throw new ArgumentException("UserId geçersiz.");
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Token boş olamaz.");
            if (expires <= DateTime.UtcNow)
                throw new ArgumentException("Token süresi geçmiş olamaz.");

            UserId = userId;
            Token = token;
            Expires = expires;
            Revoked = false;
        }

        public bool IsExpired => DateTime.UtcNow >= Expires;
        public bool IsActive => !Revoked && !IsExpired;

        public void Revoke(string? replacedByToken = null)
        {
            if (Revoked)
                throw new InvalidOperationException("Token zaten iptal edilmiş.");
            Revoked = true;
            ReplacedByToken = replacedByToken;
            UpdateTimestamp();
        }
    }
}
