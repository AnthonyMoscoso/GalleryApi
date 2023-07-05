using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Core.Repository
{
    public abstract class EFRepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class, new()
    {
        protected DbContext _context;
        public DbSet<TEntity> Table;
        public EFRepository(DbContext context)
        {
            _context = context;
            Table = context.Set<TEntity>();
        }

        public async Task Delete(TKey key)
        {
            TEntity? entity = await Get(key);
            if (entity != null)
            {
                Table.Remove(entity);
                await Save();
            }

        }

        public DbSet<TEntity> Include(params Expression<Func<TEntity, object>>[] includeExpressions)
        {
            foreach (var includeExpression in includeExpressions)
            {
                Table.Include(includeExpression);
            }
            return Table;

        }

        public async Task<TEntity?> Get(TKey key)
        {
            return await Table.FindAsync(key);
        }

        public async Task<TEntity?> Get(Expression<Func<TEntity, bool>> expression)
        {
            return await Table.FirstOrDefaultAsync(expression);
        }

        public async Task<IQueryable<TEntity>> GetAll()
        {
            IList<TEntity> entities = await Table.ToListAsync();
            return entities.AsQueryable();
        }

        public async Task<IQueryable<TEntity>> GetAll(Expression<Func<TEntity, bool>> expression)
        {
            IList<TEntity> entities = await Table.Where(expression).ToListAsync();
            return entities.AsQueryable();
        }

        public async Task<IQueryable<TEntity>> GetOrder<TProperty>(Expression<Func<TEntity, TProperty>> orderExpresion, bool isAsc = false)
        {
            IQueryable<TEntity> search = Table;
            search = isAsc ? search.OrderBy(orderExpresion) : Table.OrderByDescending(orderExpresion);
            List<TEntity> entities = await search.ToListAsync();
            return entities.AsQueryable();

        }

        public async Task<IQueryable<TEntity>> GetPag<TProperty>(Expression<Func<TEntity, TProperty>> orderExpresion, int skip, int take, bool isAsc = false)
        {
            IQueryable<TEntity> search = Table;
            search = isAsc ? search.OrderBy(orderExpresion) : Table.OrderByDescending(orderExpresion);
            search = search.Skip(skip).Take(take);
            List<TEntity> entities = await search.ToListAsync();
            return entities.AsQueryable();
        }

        public async Task<IQueryable<TEntity>> GetPag<TProperty>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TProperty>> orderExpresion, int skip, int take, bool isAsc = false)
        {
            IQueryable<TEntity> search = Table.Where(expression);
            if (search.Any())
            {
                search = isAsc ? search.OrderBy(orderExpresion) : Table.OrderByDescending(orderExpresion);
                search = search.Skip(skip).Take(take);
            }
            List<TEntity> entities = await search.ToListAsync();
            return entities.AsQueryable();
        }

        public async Task<TEntity> Insert(TEntity entity)
        {
            Table.Add(entity);
            await Save();
            return entity;
        }

        public async Task<TEntity?> Update(TEntity entity, TKey key)
        {
            TEntity? search = await Table.FindAsync(key);
            if (search != null)
            {
                _context.Entry(search).CurrentValues.SetValues(entity);
                await Save();
            }

            return await Get(key);
        }

        protected async Task Save()
        {
            await _context.SaveChangesAsync();
        }

 

        public async Task DeleteRange(ICollection<TEntity> list)
        {
            if(!list.Any())
            {
                return;
            }
            Table.RemoveRange(list);
            await Save();
        }

        public async Task<IEnumerable<TEntity>> InsertRange(IEnumerable<TEntity> entities)
        {
            Table.AddRange(entities);
            await Save();
            return entities;
        }
    }
}
