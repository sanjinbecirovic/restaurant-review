using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RestaurantReview.DataAccess.Repositories.Interfaces;

namespace RestaurantReview.DataAccess.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>
        where TEntity : class, IEntity
    {
        protected readonly RestaurantReviewContext dbContext;

        public GenericRepository(RestaurantReviewContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IQueryable<TEntity> GetAll()
            =>  this.dbContext.Set<TEntity>();


        public async Task<TEntity> GetByIdAsync(int id)
            => await this.dbContext.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);


        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            var strategy = this.dbContext.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(
                async () =>
                {
                    var result = await this.dbContext.Set<TEntity>().AddAsync(entity);
                    await this.dbContext.SaveChangesAsync();
                    return result.Entity;
                });
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            var strategy = this.dbContext.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(
                async () =>
                {
                    var result = this.dbContext.Attach(entity);
                    this.dbContext.Set<TEntity>().Update(entity);
                    await this.dbContext.SaveChangesAsync();
                    return result.Entity;
                });
        }

        public async Task DeleteAsync(int id)
        {
            var strategy = this.dbContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(
                async () =>
                {
                    var entity = await GetByIdAsync(id);
                    this.dbContext.Set<TEntity>().Remove(entity);
                    await this.dbContext.SaveChangesAsync();
                });
        }
    }
}
