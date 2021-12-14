using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Up.NET.Api.Transactions;
using Up.NET.Models;
using UpBlazor.Application.Exceptions;
using UpBlazor.Application.Services;

namespace UpBlazor.Application.Features.Up;

public record GetUpTransactionsQuery(string AccountId = null) : IRequest<IReadOnlyList<TransactionResource>>;

public class GetUpTransactionsQueryHandler : IRequestHandler<GetUpTransactionsQuery, IReadOnlyList<TransactionResource>>
{
    private readonly ICurrentUserService _currentUserService;

    public GetUpTransactionsQueryHandler(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    public async Task<IReadOnlyList<TransactionResource>> Handle(GetUpTransactionsQuery request, CancellationToken cancellationToken)
    {
        var upApi = await _currentUserService.GetApiAsync(cancellationToken: cancellationToken);

        UpResponse<PaginatedDataResponse<TransactionResource>> response;

        if (request.AccountId is null)
        {
            response = await upApi.GetTransactionsAsync(100, TransactionStatus.Settled);
        }
        else
        {
            response = await upApi.GetTransactionsAsync(request.AccountId, 100, TransactionStatus.Settled);
        }

        if (!response.Success)
        {
            throw new UpApiException(response.Errors);
        }

        return response.Response.Data
            .AsReadOnly();
    }
}