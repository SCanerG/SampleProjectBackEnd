using FluentValidation;
using MediatR;
using SampleProjectBackEnd.Application.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace SampleProjectBackEnd.Application.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
         where TRequest : IRequest<TResponse>
         where TResponse : class, IResult
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!_validators.Any())
                return await next();

            var context = new ValidationContext<TRequest>(request);
            var errors = _validators
                .Select(v => v.Validate(context))
                .SelectMany(r => r.Errors)
                .Where(e => e != null)
                .ToList();

            if (errors.Any())
            {
                var errorMessage = string.Join(" | ", errors.Select(e => e.ErrorMessage));

                // Response type Success = false, Message = errors
                return (TResponse)(object)new ErrorResult(errorMessage);
            }

            return await next();
        }
    }
}
//Her Request tipi için (örneğin CreateProductCommand) ilgili validator’ü bulur.

//Hataları toplar → tek mesaj haline getirir → ErrorResult döndürür.

//Böylece Controller hiçbir validation kodu yazmaz.