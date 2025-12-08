using SampleProjectBackEnd.Domain.Abstractions;

namespace SampleProjectBackEnd.Domain.Entities
{
    public class RefreshToken : BaseEntity<int>
    {
        public string Token { get; private set; } = string.Empty;
        public string UserId { get; private set; } = string.Empty;
        public DateTime Expires { get; private set; }
        public bool Revoked { get; private set; }
        public string? ReplacedByToken { get; private set; }

        private RefreshToken() { } // EF için boş ctor

        public RefreshToken(string userId, string token, DateTime expires)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("UserId boş olamaz.");
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
