using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RAP.Contracts;
using RAP.Persistence.Primary.Entities;

namespace RAP.Persistence.Primary
{
    public class BaseStorage<T> : IStorage<T> where T : class, IBaseEntity
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        private readonly string _connectionString;

        public BaseStorage()
        {

        }
        public BaseStorage(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async virtual Task<T> FindByAsync(Expression<Func<T, bool>> predicate)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseMySql(_connectionString);
            using var context = new DatabaseContext(optionsBuilder.Options);
            DbSet<T> dbSet = context.Set<T>();

            var item = await dbSet.SingleOrDefaultAsync(predicate);

            return item;
        }

        public async virtual Task<T> FindAsync(Guid id)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseMySql(_connectionString);
            using var context = new DatabaseContext(optionsBuilder.Options);
            DbSet<T> dbSet = context.Set<T>();

            var item = await dbSet.FindAsync(id);
            await context.SaveChangesAsync();

            return item;
        }

        public async virtual Task<T> UpdateAsync(T item)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseMySql(_connectionString);
            using var context = new DatabaseContext(optionsBuilder.Options);
            DbSet<T> dbSet = context.Set<T>();

            item.UpdatedAt = DateTimeOffset.UtcNow;

            dbSet.Update(item);
            await context.SaveChangesAsync();

            return item;
        }

        public async virtual Task<T> CreateAsync(T item)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseMySql(_connectionString);
            using var context = new DatabaseContext(optionsBuilder.Options);
            DbSet<T> dbSet = context.Set<T>();

            item.CreatedAt = DateTimeOffset.UtcNow;
            item.UpdatedAt = DateTimeOffset.UtcNow;

            await dbSet.AddAsync(item);
            await context.SaveChangesAsync();

            return item;
        }

        public async Task<List<T>> FindAllAsync()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseMySql(_connectionString);
            using var context = new DatabaseContext(optionsBuilder.Options);
            DbSet<T> dbSet = context.Set<T>();

            var items = await dbSet.ToListAsync();

            return items;
        }
    }
}
