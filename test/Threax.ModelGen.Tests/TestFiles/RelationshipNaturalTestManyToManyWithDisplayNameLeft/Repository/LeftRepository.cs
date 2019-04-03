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
    public partial class LeftRepository : ILeftRepository
    {
        private AppDbContext dbContext;
        private AppMapper mapper;

        public LeftRepository(AppDbContext dbContext, AppMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<LeftCollection> List(LeftQuery query)
        {
            var dbQuery = await query.Create(this.Entities);

            var total = await dbQuery.CountAsync();
            dbQuery = dbQuery.Skip(query.SkipTo(total)).Take(query.Limit);
            var results = await dbQuery.ToListAsync();

            return new LeftCollection(query, total, results.Select(i => mapper.MapLeft(i, new Left())));
        }

        public async Task<Left> Get(Guid leftId)
        {
            var entity = await this.Entity(leftId);
            return mapper.MapLeft(entity, new Left());
        }

        public async Task<Left> Add(LeftInput left)
        {
            var entity = mapper.MapLeft(left, new LeftEntity());
            this.dbContext.Add(entity);
            await SaveChanges();
            return mapper.MapLeft(entity, new Left());
        }

        public async Task<Left> Update(Guid leftId, LeftInput left)
        {
            var entity = await this.Entity(leftId);
            if (entity != null)
            {
                mapper.MapLeft(left, entity);
                await SaveChanges();
                return mapper.MapLeft(entity, new Left());
            }
            throw new KeyNotFoundException($"Cannot find left {leftId.ToString()}");
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

        public virtual async Task<bool> HasLefts()
        {
            return await Entities.CountAsync() > 0;
        }

        public virtual async Task AddRange(IEnumerable<LeftInput> lefts)
        {
            var entities = lefts.Select(i => mapper.MapLeft(i, new LeftEntity()));
            this.dbContext.Lefts.AddRange(entities);
            await SaveChanges();
        }

        protected virtual async Task SaveChanges()
        {
            await this.dbContext.SaveChangesAsync();
        }

        private DbSet<LeftEntity> Entities
        {
            get
            {
                return dbContext.Lefts;
            }
        }

        private Task<LeftEntity> Entity(Guid leftId)
        {
            return Entities.Where(i => i.LeftId == leftId).FirstOrDefaultAsync();
        }
    }
}