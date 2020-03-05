using Abp.Domain.Entities;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using YunDa.ISAS.MongoDB.Configuration;
using YunDa.ISAS.MongoDB.Factory;

namespace YunDa.ISAS.MongoDB.Repositories
{
    public class MongoDbRepository<TEntity, TPrimaryKey> : IMongoDbRepository<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>
    {
        public virtual IMongoDatabase Database
        {
            get
            {
                return _databaseFactory.InstanceMongoDatabase();
            }
        }

        public virtual IMongoCollection<TEntity> Collection
        {
            get
            {
                return _databaseFactory.InstanceMongoDatabase().GetCollection<TEntity>(typeof(TEntity).Name);
            }
        }

        private readonly IMongoClientFactory _databaseFactory;

        public MongoDbRepository(IMongoDBConfiguration mongoDbConfiguration, IMongoClientFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null)
        {
            if (filter != null)
                return Collection.AsQueryable().Where(filter);
            else
                return Collection.AsQueryable();
        }

        public TEntity GetOne(TPrimaryKey id)
        {
            var entity = Collection.Find<TEntity>(s => s.Id.Equals(id)).FirstOrDefault();
            if (entity == null)
            {
                throw new EntityNotFoundException("There is no such an entity with given primary key. Entity type: " + typeof(TEntity).FullName + ", primary key: " + id);
            }

            return entity;
        }

        public Task<TEntity> GetOneAsync(TPrimaryKey id)
        {
            var entity = Collection.Find<TEntity>(s => s.Id.Equals(id)).FirstOrDefaultAsync();
            if (entity == null)
            {
                throw new EntityNotFoundException("There is no such an entity with given primary key. Entity type: " + typeof(TEntity).FullName + ", primary key: " + id);
            }

            return entity;
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> filter = null)
        {
            if (filter != null)
                return Collection.Find(filter).FirstOrDefault();
            else
                return Collection.AsQueryable().FirstOrDefault();
        }

        public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            if (filter != null)
                return Collection.Find(filter).FirstOrDefaultAsync();
            else
                return Collection.AsQueryable().FirstOrDefaultAsync();
        }

        public void InsertOne(TEntity entity)
        {
            Collection.InsertOne(entity);
        }

        public Task InsertOneAsync(TEntity entity)
        {
            return Collection.InsertOneAsync(entity);
        }

        public void InsertMany(IEnumerable<TEntity> entitys)
        {
            Collection.InsertMany(entitys);
        }

        public Task InsertManyAsync(IEnumerable<TEntity> entitys)
        {
            return Collection.InsertManyAsync(entitys);
        }

        public ReplaceOneResult UpdateOne(TEntity entity)
        {
            //var fieldList = new List<UpdateDefinition<TEntity>>();
            //foreach (var property in typeof(TEntity).GetProperties(BindingFlags.Instance | BindingFlags.Public))
            //{
            //    if (property.Name != "Id")//更新集中不能有实体键_id
            //    {
            //        fieldList.Add(Builders<TEntity>.Update.Set(property.Name, property.GetValue(entity)));
            //    }
            //}
            //Collection.UpdateOne(s => s.Id.Equals(entity.Id), Builders<TEntity>.Update.Combine(fieldList), new UpdateOptions() { IsUpsert = true });
            return Collection.ReplaceOne(s => s.Id.Equals(entity.Id), entity);
        }

        public Task<ReplaceOneResult> UpdateOneAsync(TEntity entity)
        {
            return Collection.ReplaceOneAsync(s => s.Id.Equals(entity.Id), entity);
        }

        public DeleteResult DeleteOne(TPrimaryKey id)
        {
            return Collection.DeleteOne(s => s.Id.Equals(id));
        }

        public Task<DeleteResult> DeleteOneAsync(TPrimaryKey id)
        {
            return Collection.DeleteOneAsync(s => s.Id.Equals(id));
        }

        public DeleteResult DeleteMany(Expression<Func<TEntity, bool>> filter)
        {
            return Collection.DeleteMany(filter);
        }

        public Task<DeleteResult> DeleteManyAsync(Expression<Func<TEntity, bool>> filter)
        {
            return Collection.DeleteManyAsync(filter);
        }
    }
}