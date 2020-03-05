using Abp.Domain.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace YunDa.ISAS.MongoDB.Repositories
{
    public interface IMongoDbRepository<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>
    {
        IMongoCollection<TEntity> Collection { get; }
        IMongoDatabase Database { get; }

        TEntity GetOne(TPrimaryKey id);

        Task<TEntity> GetOneAsync(TPrimaryKey id);

        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> filter = null);

        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter = null);

        IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null);

        void InsertOne(TEntity entity);

        Task InsertOneAsync(TEntity entity);

        void InsertMany(IEnumerable<TEntity> entitys);

        Task InsertManyAsync(IEnumerable<TEntity> entitys);

        ReplaceOneResult UpdateOne(TEntity entity);

        Task<ReplaceOneResult> UpdateOneAsync(TEntity entity);

        DeleteResult DeleteOne(TPrimaryKey id);

        Task<DeleteResult> DeleteOneAsync(TPrimaryKey id);
    }
}