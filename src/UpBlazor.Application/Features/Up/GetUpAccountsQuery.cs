using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Up.NET.Api.Accounts;
using Up.NET.Models;
using UpBlazor.Application.Common.Services;
using UpBlazor.Application.Exceptions;

namespace UpBlazor.Application.Features.Up;

public record GetUpAccountsQuery : IRequest<IReadOnlyList<AccountResource>>;

public class GetUpAccountsQueryHandler : IRequestHandler<GetUpAccountsQuery, IReadOnlyList<AccountResource>>
{
    private readonly ICurrentUserService _currentUserService;

    public GetUpAccountsQueryHandler(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    public async Task<IReadOnlyList<AccountResource>> Handle(GetUpAccountsQuery request, CancellationToken cancellationToken)
    {
        var upApi = await _currentUserService.GetApiAsync(cancellationToken: cancellationToken);

        var allAccounts = new List<AccountResource>();

        var response = await upApi.GetAccountsAsync();
        
        if (!response.Success)
        {
            throw new UpApiException(response.Errors);
        }

        allAccounts.AddRange(response.Response.Data);
        
        while (response.Success && response.Response.Links.HasNext)
        {
            response = await response.Response.GetNextPageAsync();
            
            if (!response.Success)
            {
                throw new UpApiException(response.Errors);
            }

            allAccounts.AddRange(response.Response.Data);
        }
        
        var duplicateAccounts = allAccounts.GroupBy(x => x.Attributes.DisplayName);

        foreach (var duplicateAccountGrouping in duplicateAccounts)
        {
            var duplicateAccountGroupingList = duplicateAccountGrouping.ToList();
            for (var i = 0; i < duplicateAccountGroupingList.Count; i++)
            {
                if (i == 0)
                {
                    continue;
                }

                var duplicateAccount = duplicateAccountGroupingList[i];

                duplicateAccount.Attributes.DisplayName += " #" + i;
            }
        }

        return allAccounts;
    }
}