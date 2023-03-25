using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UpBlazor.Domain.Models.Mock;
using UpBlazor.Domain.Repositories;

namespace UpBlazor.Application.Features.Users;

public record GetAllUsersQuery : IRequest<IReadOnlyList<RegisteredUserDto>>;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IReadOnlyList<RegisteredUserDto>>
{
    private readonly IRegisteredUserRepository _registeredUserRepository;
    private readonly IUpUserTokenRepository _upUserTokenRepository;

    public GetAllUsersQueryHandler(IRegisteredUserRepository registeredUserRepository, IUpUserTokenRepository upUserTokenRepository)
    {
        _registeredUserRepository = registeredUserRepository;
        _upUserTokenRepository = upUserTokenRepository;
    }

    public async Task<IReadOnlyList<RegisteredUserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var allUsers = await _registeredUserRepository.GetAllAsync(cancellationToken);
        var accessTokens = await _upUserTokenRepository.GetAllAsync(cancellationToken);
        
        var output = allUsers
            .Select(x =>
            {
                var accessToken = accessTokens.SingleOrDefault(token => x.Id == token.UserId)?
                    .AccessToken;

                return new RegisteredUserDto
                {
                    Id = x.Id,
                    Email = x.Email,
                    GivenName = x.GivenName,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,
                    Token = accessToken switch
                    {
                        null => RegisteredUserDto.TokenType.NotSet,
                        MockUpApi.MockUpToken => RegisteredUserDto.TokenType.Demo,
                        _ => RegisteredUserDto.TokenType.Real
                    }
                };
            })
            .ToList()
            .AsReadOnly();
        
        return output;
    }
}