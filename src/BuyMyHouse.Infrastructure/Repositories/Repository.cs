using BuyMyHouse.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using BuyMyHouse.Infrastructure.Database;

namespace BuyMyHouse.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly BuyMyHouseDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(BuyMyHouseDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);
    public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);
    public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();
    public async Task<IEnumerable<T>> FindAsync(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        => await _dbSet.Where(predicate).ToListAsync();
    public void Remove(T entity) => _dbSet.Remove(entity);
    public void Update(T entity) => _dbSet.Update(entity);
    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
}