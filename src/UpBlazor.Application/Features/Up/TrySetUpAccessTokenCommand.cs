using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UpBlazor.Application.Services;
using UpBlazor.Core.Models;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Application.Features.Up;

public record TrySetUpAccessTokenCommand(string AccessToken) : IRequest<string>;

public class TrySetUpAccessTokenCommandHandler : IRequestHandler<TrySetUpAccessTokenCommand, string>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IUpUserTokenRepository _upUserTokenRepository;

    public TrySetUpAccessTokenCommandHandler(ICurrentUserService currentUserService, IUpUserTokenRepository upUserTokenRepository)
    {
        _currentUserService = currentUserService;
        _upUserTokenRepository = upUserTokenRepository;
    }

    public async Task<string> Handle(TrySetUpAccessTokenCommand request, CancellationToken cancellationToken)
    {
        var temporaryApi = await _currentUserService.GetApiAsync(request.AccessToken, cancellationToken: cancellationToken);

        var pingResponse = await temporaryApi.GetPingAsync();

        if (!pingResponse.Success)
        {
            // Bad token
            throw new Exception("You did not input a valid access token - did you copy and paste the whole token?");
        }

        var userId = await _currentUserService.GetUserIdAsync(cancellationToken);

        var output = new UpUserToken
        {
            UserId = userId,
            AccessToken = request.AccessToken
        };
        
        await _upUserTokenRepository.AddOrUpdateAsync(output, cancellationToken);

        return output.UserId;
    }
}