using MediatR;
using TwitterClone.Application.Interfaces;
using TwitterClone.Domain.Common;
using TwitterClone.Domain.Entities;

namespace TwitterClone.Application.Auth.Commands;

public record RegisterUserCommand(string Username, string Email, string Password) : IRequest<Result<Guid>>;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<Guid>>
{
    private readonly IUserRepository _repository;

    public RegisterUserCommandHandler(IUserRepository repository) => _repository = repository;

    public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _repository.GetByEmailAsync(request.Email, cancellationToken);
        if (existingUser != null) return Result<Guid>.Failure("Email already in use.");

        var hash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var user = new User(Guid.NewGuid(), request.Username, request.Email, hash);

        await _repository.AddAsync(user, cancellationToken);
        return Result<Guid>.Success(user.Id);
    }
}