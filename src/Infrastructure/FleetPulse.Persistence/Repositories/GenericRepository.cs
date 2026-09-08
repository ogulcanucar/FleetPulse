using FleetPulse.Application.Abstractions.Repositories;
using FleetPulse.Domain.Common;
using FleetPulse.Persistence.Context;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FleetPulse.Persistence.Repositories
{
    public class GenericRepository<T>(MongoDbContext context, string collectionName) : IRepository<T> where T : BaseEntity
    {
        protected readonly IMongoCollection<T> _collection = context.GetType()
                                 .GetProperty(collectionName)?
                                 .GetValue(context) as IMongoCollection<T>
                                 ?? throw new InvalidOperationException($"Collection {collectionName} not found.");

        public async Task<T?> GetByIdAsync(string id)
        {
            return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<T>> ListAllAsync(Expression<Func<T, bool>>? predicate = null)
        {
            if (predicate == null)
                return await _collection.Find(_ => true).ToListAsync();

            return await _collection.Find(predicate).ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            await _collection.InsertOneAsync(entity);
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            await _collection.ReplaceOneAsync(x => x.Id == entity.Id, entity);
        }

        public async Task DeleteAsync(T entity)
        {
            await _collection.DeleteOneAsync(x => x.Id == entity.Id);
        }
    }
}
