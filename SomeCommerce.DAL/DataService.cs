using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using SomeCommerce.DAL.Data;
using System.Linq.Expressions;

namespace SomeCommerce.DAL
{
    public class DataService
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<DataService> _logger;
        public DataService(ApplicationDbContext db, ILogger<DataService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<bool> AnyAsync<TEntity>() where TEntity : class => await _db.Set<TEntity>().AnyAsync();

        public async Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>> whereExpression = null) where TEntity : class
        {
            IQueryable<TEntity> query = _db.Set<TEntity>().AsQueryable();
            if (whereExpression != null) query = query.Where(whereExpression);
            try
            {
                return await query.AnyAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                throw;
            }
        }

        public IdentityResult AddOrUpdate<TEntity>(TEntity entity) where TEntity : class
        {
            try
            {
                _db.Update(entity);
                _db.SaveChanges();
                return IdentityResult.Success;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                return IdentityResult.Failed();
            }
        }

        public async Task<IdentityResult> AddOrUpdateAsync<TEntity>(TEntity entity) where TEntity : class
        {
            try
            {
                _db.Update(entity);
                await _db.SaveChangesAsync();
                return IdentityResult.Success;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                return IdentityResult.Failed();
            }
        }

        public async Task<IdentityResult> AddRangeAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            try
            {
                await _db.AddRangeAsync(entities);
                await _db.SaveChangesAsync();
                return IdentityResult.Success;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                return IdentityResult.Failed();
            }
        }

        public IdentityResult AddRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            try
            {
                _db.AddRange(entities);
                _db.SaveChanges();
                return IdentityResult.Success;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                return IdentityResult.Failed();
            }
        }

        public async Task<IdentityResult> UpdateRangeAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            try
            {
                _db.UpdateRange(entities);
                await _db.SaveChangesAsync();
                return IdentityResult.Success;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                return IdentityResult.Failed();
            }
        }

        public async Task<TEntity> GetAsync<TEntity, TKey>(TKey id) where TEntity : class
        {
            try
            {
                return await _db.FindAsync<TEntity>(id);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                return null;
            }
        }

        public async Task<TEntity> GetAsync<TEntity>(Guid id, Expression<Func<TEntity, TEntity>> selectExpression = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeExpression = null) where TEntity : class
        {
            IProperty keyProperty = _db.Model.FindEntityType(typeof(TEntity)).FindPrimaryKey().Properties[0];
            IQueryable<TEntity> query = _db.Set<TEntity>().AsQueryable();
            if (includeExpression != null) query = includeExpression(query);
            if (selectExpression != null) query = query.Select(selectExpression);
            try
            {
                return await query.FirstOrDefaultAsync(e => EF.Property<Guid>(e, keyProperty.Name) == id);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                return null;
            }
        }

        public async Task<TEntity> FirstOrDefaultAsNoTrackingAsync<TEntity>(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, TEntity>> selectExpression = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeExpression = null) where TEntity : class
        {
            IQueryable<TEntity> query = _db.Set<TEntity>().AsQueryable();
            if (includeExpression != null) query = includeExpression(query);
            query = query.Where(whereExpression);
            if (selectExpression != null) query = query.Select(selectExpression);
            try
            {
                return await query.AsNoTracking().FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                return null;
            }
        }

        public async Task<TEntity> FirstOrDefaultAsync<TEntity>(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, TEntity>> selectExpression = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeExpression = null) where TEntity : class
        {
            IQueryable<TEntity> query = _db.Set<TEntity>().AsQueryable();
            if (includeExpression != null) query = includeExpression(query);
            query = query.Where(whereExpression);
            if (selectExpression != null) query = query.Select(selectExpression);
            try
            {
                return await query.FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                return null;
            }
        }

        public TEntity GetAsNoTracking<TEntity>(Guid id, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeExpression = null) where TEntity : class
        {
            IProperty keyProperty = _db.Model.FindEntityType(typeof(TEntity)).FindPrimaryKey().Properties[0];
            IQueryable<TEntity> query = _db.Set<TEntity>().AsQueryable();
            if (includeExpression != null) query = includeExpression(query);
            try
            {
                return query.AsNoTracking().FirstOrDefault(e => EF.Property<Guid>(e, keyProperty.Name) == id);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                return null;
            }
        }

        public async Task<TEntity> GetAsNoTrackingAsync<TEntity>(
            Guid id,
            Expression<Func<TEntity, TEntity>> selectExpression = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeExpression = null,
            bool asSplitQuery = false) where TEntity : class
        {
            IProperty keyProperty = _db.Model.FindEntityType(typeof(TEntity)).FindPrimaryKey().Properties[0];
            IQueryable<TEntity> query = _db.Set<TEntity>().AsQueryable();
            if (includeExpression != null) query = includeExpression(query);
            if (selectExpression != null) query = query.Select(selectExpression);
            else if (asSplitQuery) query = query.AsSplitQuery();
            try
            {
                return await query.AsNoTracking().FirstOrDefaultAsync(e => EF.Property<Guid>(e, keyProperty.Name) == id);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                return null;
            }
        }

        public async Task<List<TEntity>> GetAllAsNoTrackingAsync<TEntity>(
            Expression<Func<TEntity, bool>> whereExpression = null,
            Expression<Func<TEntity, TEntity>> selectExpression = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeExpression = null,
            List<Expression<Func<TEntity, object>>> orderExpression = null,
            bool byDescending = false,
            int? take = null,
            int? skip = null,
            bool asSplitQuery = false) where TEntity : class
        {
            IQueryable<TEntity> query = _db.Set<TEntity>().AsQueryable();
            if (whereExpression != null) query = query.Where(whereExpression);
            if (includeExpression != null) query = includeExpression(query);

            if (orderExpression != null && orderExpression.Any())
            {
                IOrderedQueryable<TEntity> orderedQuery = null;
                for (int i = 0; i < orderExpression.Count; i++)
                {
                    if (!byDescending)
                    {
                        if (orderedQuery == null)
                            orderedQuery = query.OrderBy(orderExpression[i]);
                        else orderedQuery.ThenBy(orderExpression[i]);
                    }
                    else
                    {
                        if (orderedQuery == null)
                            orderedQuery = query.OrderByDescending(orderExpression[i]);
                        else orderedQuery.ThenByDescending(orderExpression[i]);
                    }
                }
                query = orderedQuery;
            }

            if (skip.HasValue) query = query.Skip(skip.Value);
            if (take.HasValue) query = query.Take(take.Value);
            if (selectExpression != null) query = query.Select(selectExpression);
            else if (asSplitQuery) query = query.AsSplitQuery();

            try
            {
                return await query.AsNoTracking().ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                throw;
            }
        }

        public async Task<int> CountAsync<TEntity>(Expression<Func<TEntity, bool>> whereExpression = null) where TEntity : class
        {
            IQueryable<TEntity> query = _db.Set<TEntity>().AsQueryable();
            if (whereExpression != null) query = query.Where(whereExpression);

            try
            {
                return await query.AsNoTracking().CountAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                throw;
            }
        }


        public List<TEntity> GetAll<TEntity>() where TEntity : class => _db.Set<TEntity>().ToList();

        public async Task<List<TEntity>> GetAllAsync<TEntity>() where TEntity : class => await _db.Set<TEntity>().ToListAsync();

        public async Task<List<TEntity>> GetAllAsync<TEntity>(Expression<Func<TEntity, bool>> whereExpression = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeExpression = null) where TEntity : class
        {
            IQueryable<TEntity> query = _db.Set<TEntity>().AsQueryable();
            if (includeExpression != null) query = includeExpression(query);
            if (whereExpression != null) query = query.Where(whereExpression);
            try
            {
                return await query.ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                throw;
            }
        }

        public async Task<IdentityResult> RemoveAsync<TEntity, TKey>(TKey id) where TEntity : class
        {
            TEntity entity = await GetAsync<TEntity, TKey>(id);
            try
            {
                return await RemoveAsync(entity);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                return IdentityResult.Failed();
            }
        }

        public async Task<IdentityResult> RemoveAsync<TEntity>(TEntity entity) where TEntity : class
        {
            try
            {
                _db.Remove(entity);
                await _db.SaveChangesAsync();
                _logger.LogInformation($"{typeof(TEntity)} has been removed: {{@entity}}", entity);
                return IdentityResult.Success;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                return IdentityResult.Failed();
            }
        }

        public async Task<IdentityResult> RemoveRangeAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            try
            {
                _db.RemoveRange(entities);
                await _db.SaveChangesAsync();
                _logger.LogInformation($"Following List of {typeof(TEntity)} has been removed: {{@entities}}", entities);
                return IdentityResult.Success;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                return IdentityResult.Failed();
            }
        }

        public IdentityResult RemoveRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            try
            {
                _db.RemoveRange(entities);
                _db.SaveChanges();
                _logger.LogInformation($"Following List of {typeof(TEntity)} has been removed: {{@entities}}", entities);
                return IdentityResult.Success;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                return IdentityResult.Failed();
            }
        }

        public void Detach(object entity) => _db.Entry(entity).State = EntityState.Detached;
    }
}
