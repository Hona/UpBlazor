using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Up.NET.Api.Transactions;
using Up.NET.Models;
using UpBlazor.Application.Exceptions;
using UpBlazor.Application.Services;

namespace UpBlazor.Application.Features.Up;

public record GetUpTransactionsPagedQuery(string AccountId = null) : IRequest<UpResponse<PaginatedDataResponse<TransactionResource>>>;

public class GetUpTransactionsPagedQueryHandler : IRequestHandler<GetUpTransactionsPagedQuery, UpResponse<PaginatedDataResponse<TransactionResource>>>
{
    private readonly ICurrentUserService _currentUserService;

    public GetUpTransactionsPagedQueryHandler(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    public async Task<UpResponse<PaginatedDataResponse<TransactionResource>>> Handle(GetUpTransactionsPagedQuery request, CancellationToken cancellationToken)
    {
        var upApi = await _currentUserService.GetApiAsync(cancellationToken: cancellationToken);

        UpResponse<PaginatedDataResponse<TransactionResource>> response;

        if (string.IsNullOrWhiteSpace(request.AccountId))
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

        return response;
    }
}