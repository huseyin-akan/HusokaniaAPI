using MediatR;
using Application.Common.Abstractions.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Application.Common.Behaviours;

public class TransactionScopeBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
where TRequest : IRequest<TResponse>, ITransactionalRequest
{
    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        using TransactionScope transactionScope = new(TransactionScopeAsyncFlowOption.Enabled);
        TResponse response;
        try
        {
            response = await next();
            transactionScope.Complete();
        }
        catch (Exception)
        {
            transactionScope.Dispose();
            throw;
        }
        return response;
    }
}
