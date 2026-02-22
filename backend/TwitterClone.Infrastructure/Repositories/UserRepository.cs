using Microsoft.EntityFrameworkCore;
using TwitterClone.Application.Interfaces;
using TwitterClone.Domain.Entities;
using TwitterClone.Infrastructure.Data;

namespace TwitterClone.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly TwitterDbContext _context;
    public UserRepository(TwitterDbContext context) => _context = context;

    public Task<User?> GetByIdAsync(Guid id, CancellationToken ct) => _context.Users.FirstOrDefaultAsync(u => u.Id == id, ct);
    public Task<User?> GetByEmailAsync(string email, CancellationToken ct) => _context.Users.FirstOrDefaultAsync(u => u.Email == email, ct);

    public async Task AddAsync(User user, CancellationToken ct)
    {
        await _context.Users.AddAsync(user, ct);
        await _context.SaveChangesAsync(ct);
    }
}