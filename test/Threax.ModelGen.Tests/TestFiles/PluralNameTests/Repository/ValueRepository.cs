using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Test.Database;
using Test.InputModels;
using Test.ViewModels;
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
            var results = await dbQuery.ToListAsync();

            return new ValueCollection(query, total, results.Select(i => mapper.MapValue(i, new Value())));
        }

        public async Task<Value> Get(Guid valueId)
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

        public async Task<Value> Update(Guid valueId, ValueInput value)
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

        public async Task Delete(Guid id)
        {
            var entity = await this.Entity(id);
            if (entity != null)
            {
                Entities.Remove(entity);
                await SaveChanges();
            }
        }

        public virtual async Task<bool> HasLotsaValues()
        {
            return await Entities.CountAsync() > 0;
        }

        public virtual async Task AddRange(IEnumerable<ValueInput> lotsaValues)
        {
            var entities = lotsaValues.Select(i => mapper.MapValue(i, new ValueEntity()));
            this.dbContext.LotsaValues.AddRange(entities);
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
                return dbContext.LotsaValues;
            }
        }

        private Task<ValueEntity> Entity(Guid valueId)
        {
            return Entities.Where(i => i.ValueId == valueId).FirstOrDefaultAsync();
        }
    }
}