using MediatR;
using TwitterClone.Application.Interfaces;
using TwitterClone.Domain.Common;

namespace TwitterClone.Application.Auth.Queries;

public record UserDto(Guid Id, string Username, string Email);
public record LoginUserQuery(string Email, string Password) : IRequest<Result<UserDto>>;

public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, Result<UserDto>>
{
    private readonly IUserRepository _repository;

    public LoginUserQueryHandler(IUserRepository repository) => _repository = repository;

    public async Task<Result<UserDto>> Handle(LoginUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByEmailAsync(request.Email, cancellationToken);
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return Result<UserDto>.Failure("Invalid credentials.");
        }

        return Result<UserDto>.Success(new UserDto(user.Id, user.Username, user.Email));
    }
}