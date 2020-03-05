using Abp.Authorization;
using Abp.Domain.Uow;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YunDa.ISAS.DataTransferObject;
using YunDa.ISAS.Entities.MongoDB;
using YunDa.ISAS.MongoDB.Repositories;
using Newtonsoft.Json;

namespace YunDa.ISAS.Test
{
    public class TestMongoDB : ITestMongoDB
    {
        private readonly IMongoDbRepository<TestEntity, Guid> _testEntityRepository;
        private readonly ICurrentUnitOfWorkProvider _currentUnitOfWorkProvider;

        public TestMongoDB(
            IMongoDbRepository<TestEntity, Guid> testEntityRepository
            , ICurrentUnitOfWorkProvider currentUnitOfWorkProvider)
        {
            _testEntityRepository = testEntityRepository;
            _currentUnitOfWorkProvider = currentUnitOfWorkProvider;
        }

        //[UnitOfWork]
        [AbpAllowAnonymous]
        [HttpPost]
        public async Task<RequestEasyResult> TestMongoDBAdd()
        {
            RequestEasyResult rst = new RequestEasyResult
            {
                Flag = true
            };
            try
            {
                TestEntity t = new TestEntity
                {
                    Name = "测试MongoDB2233",
                    DTime = DateTime.Now
                };
                Console.WriteLine(t.ToString());
                await _testEntityRepository.InsertOneAsync(t);
                var tArr = new TestEntity[2000000];
                for (int i = 0; i < tArr.Length; i++)
                {
                    tArr[i] = new TestEntity
                    {
                        Name = "测试MongoDB2233" + i,
                        DTime = DateTime.Now
                    };
                }
                await _testEntityRepository.InsertManyAsync(tArr);

                Console.WriteLine(JsonConvert.SerializeObject(t));
            }
            catch (Exception ex)
            {
                _ = ex ?? throw new Exception(ex.ToString());
                rst.Flag = false;
            }

            return rst;
        }

        //[UnitOfWork]
        [AbpAllowAnonymous]
        [HttpPost]
        public RequestPageResult<TestEntity> TestMongoDBSearch()
        {
            RequestPageResult<TestEntity> rst = new RequestPageResult<TestEntity>
            {
                Flag = true
            };
            try
            {
                List<TestEntity> entities = new List<TestEntity>();
                var guid = Guid.Parse("d5865fed-eea5-f742-9159-cf449f5a7354");
                //entities.Add(_testEntityRepository.GetOne(guid));
                rst.ResultDatas = _testEntityRepository.GetAll(e => e.Name == "测试MongoDB2233").ToList();
            }
            catch (Exception ex)
            {
                _ = ex ?? throw new Exception(ex.ToString());
                rst.Flag = false;
            }
            return rst;
        }
    }
}