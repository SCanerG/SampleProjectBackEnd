using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace SampleProjectBackEnd.Domain.Abstractions
{
    public abstract class BaseEntity<T>
    {
        public T Id { get; protected set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; private set; }

        public void UpdateTimestamp()
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
// ID T tipinde Çünküü

//Guid kullanmak istersen, sıkıntı.

//Bazı aggregate’lerde string / başka tür ID gerekiyorsa, sıkıntı.

//Başka bir servisle entegre olup oradaki Id türüne göre davranmak istersen, sıkıntı.

//T kullanarak diyorsun ki:

//"Bu entity’nin identity tipine domain değil, context karar verir."