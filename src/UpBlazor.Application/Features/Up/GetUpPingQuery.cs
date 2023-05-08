using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Up.NET.Api.Utilities;
using Up.NET.Models;
using UpBlazor.Application.Common.Services;

namespace UpBlazor.Application.Features.Up;

public record GetUpPingQuery(bool ForceReload = false) : IRequest<UpResponse<PingResponse>>;

public class GetUpPingQueryHandler : IRequestHandler<GetUpPingQuery, UpResponse<PingResponse>>
{
    private readonly ICurrentUserService _currentUserService;

    public GetUpPingQueryHandler(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    public async Task<UpResponse<PingResponse>> Handle(GetUpPingQuery request, CancellationToken cancellationToken)
    {
        var upApi = await _currentUserService.GetApiAsync(forceReload: request.ForceReload, cancellationToken: cancellationToken);

        if (upApi is null)
        {
            return null;
        }

        var output = await upApi.GetPingAsync();
        return output;
    }
}