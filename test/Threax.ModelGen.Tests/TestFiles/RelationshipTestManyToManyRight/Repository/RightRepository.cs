using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Test.Database;
using Test.InputModels;
using Test.ViewModels;
using Test.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Threax.AspNetCore.Halcyon.Ext;

namespace Test.Repository
{
    public partial class RightRepository : IRightRepository
    {
        private AppDbContext dbContext;
        private IMapper mapper;

        public RightRepository(AppDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<RightCollection> List(RightQuery query)
        {
            var dbQuery = await query.Create(this.Entities);

            var total = await dbQuery.CountAsync();
            dbQuery = dbQuery.Skip(query.SkipTo(total)).Take(query.Limit);
            var resultQuery = dbQuery.Select(i => mapper.Map<Right>(i));
            var results = await resultQuery.ToListAsync();

            return new RightCollection(query, total, results);
        }

        public async Task<Right> Get(Guid rightId)
        {
            var entity = await this.Entity(rightId);
            return mapper.Map<Right>(entity);
        }

        public async Task<Right> Add(RightInput right)
        {
            var entity = mapper.Map<RightEntity>(right);
            this.dbContext.Add(entity);
            await SaveChanges();
            return mapper.Map<Right>(entity);
        }

        public async Task<Right> Update(Guid rightId, RightInput right)
        {
            var entity = await this.Entity(rightId);
            if (entity != null)
            {
                mapper.Map(right, entity);
                await SaveChanges();
                return mapper.Map<Right>(entity);
            }
            throw new KeyNotFoundException($"Cannot find right {rightId.ToString()}");
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

        public virtual async Task<bool> HasRights()
        {
            return await Entities.CountAsync() > 0;
        }

        public virtual async Task AddRange(IEnumerable<RightInput> rights)
        {
            var entities = rights.Select(i => mapper.Map<RightEntity>(i));
            this.dbContext.Rights.AddRange(entities);
            await SaveChanges();
        }

        protected virtual async Task SaveChanges()
        {
            await this.dbContext.SaveChangesAsync();
        }

        private DbSet<RightEntity> Entities
        {
            get
            {
                return dbContext.Rights;
            }
        }

        private Task<RightEntity> Entity(Guid rightId)
        {
            return Entities.Where(i => i.RightId == rightId).FirstOrDefaultAsync();
        }
    }
}