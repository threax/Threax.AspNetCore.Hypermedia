using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Test.Database;
using Test.InputModels;
using Test.ViewModels;
using Test.Models;
using Test.Mappers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Threax.AspNetCore.Halcyon.Ext;

namespace Test.Repository
{
    public partial class ValueRepository : IValueRepository
    {
        private AppDbContext dbContext;
        private AppMapper mapper;

        public ValueRepository(AppDbContext dbContext, AppMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<ValueCollection> List(ValueQuery query)
        {
            var dbQuery = await query.Create(this.Entities);

            var total = await dbQuery.CountAsync();
            dbQuery = dbQuery.Skip(query.SkipTo(total)).Take(query.Limit);
            var resultQuery = dbQuery.Select(i => mapper.MapValue(i, new Value()));
            var results = await resultQuery.ToListAsync();

            return new ValueCollection(query, total, results);
        }

        public async Task<Value> Get(String valueId)
        {
            var entity = await this.Entity(valueId);
            return mapper.MapValue(entity, new Value());
        }

        public async Task<Value> Add(ValueInput value)
        {
            var entity = mapper.MapValue(value, new ValueEntity());
            this.dbContext.Add(entity);
            await SaveChanges();
            return mapper.MapValue(entity, new Value());
        }

        public async Task<Value> Update(String valueId, ValueInput value)
        {
            var entity = await this.Entity(valueId);
            if (entity != null)
            {
                mapper.MapValue(value, entity);
                await SaveChanges();
                return mapper.MapValue(entity, new Value());
            }
            throw new KeyNotFoundException($"Cannot find value {valueId.ToString()}");
        }

        public async Task Delete(String id)
        {
            var entity = await this.Entity(id);
            if (entity != null)
            {
                Entities.Remove(entity);
                await SaveChanges();
            }
        }

        public virtual async Task<bool> HasValues()
        {
            return await Entities.CountAsync() > 0;
        }

        public virtual async Task AddRange(IEnumerable<ValueInput> values)
        {
            var entities = values.Select(i => mapper.MapValue(i, new ValueEntity()));
            this.dbContext.Values.AddRange(entities);
            await SaveChanges();
        }

        protected virtual async Task SaveChanges()
        {
            await this.dbContext.SaveChangesAsync();
        }

        private DbSet<ValueEntity> Entities
        {
            get
            {
                return dbContext.Values;
            }
        }

        private Task<ValueEntity> Entity(String valueId)
        {
            return Entities.Where(i => i.ValueId == valueId).FirstOrDefaultAsync();
        }
    }
}