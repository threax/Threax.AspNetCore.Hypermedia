using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Text;
using Threax.AspNetCore.Models;

namespace Threax.ModelGen
{
    static class RepoGenerator
    {
        public static String Get(JsonSchema4 schema, String ns)
        {
            String Model, model;
            NameGenerator.CreatePascalAndCamel(schema.Title, out Model, out model);
            String Models, models;
            NameGenerator.CreatePascalAndCamel(schema.GetPluralName(), out Models, out models);
            String ModelId, modelId;
            NameGenerator.CreatePascalAndCamel(schema.GetKeyName(), out ModelId, out modelId);
            return Create(ns, Model, model, Models, models, schema.GetKeyType().Name, ModelId, modelId);
        }

        private static String Create(String ns, String Model, String model, String Models, String models, String modelIdType, String ModelId, String modelId) {
            return
$@"using AutoMapper;
using Microsoft.EntityFrameworkCore;
using {ns}.Database;
using {ns}.InputModels;
using {ns}.ViewModels;
using {ns}.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Threax.AspNetCore.Halcyon.Ext;

namespace {ns}.Repository
{{
    public partial class {Model}Repository : I{Model}Repository
    {{
        private AppDbContext dbContext;
        private IMapper mapper;

        public {Model}Repository(AppDbContext dbContext, IMapper mapper)
        {{
            this.dbContext = dbContext;
            this.mapper = mapper;
        }}

        public async Task<{Model}Collection> List({Model}Query query)
        {{
            var dbQuery = query.Create(this.Entities);

            var total = await dbQuery.CountAsync();
            dbQuery = dbQuery.Skip(query.SkipTo(total)).Take(query.Limit);
            var resultQuery = dbQuery.Select(i => mapper.Map<{Model}>(i));
            var results = await resultQuery.ToListAsync();

            return new {Model}Collection(query, total, results);
        }}

        public async Task<{Model}> Get({modelIdType} {modelId})
        {{
            var entity = await this.Entity({modelId});
            return mapper.Map<{Model}>(entity);
        }}

        public async Task<{Model}> Add({Model}Input {model})
        {{
            var entity = mapper.Map<{Model}Entity>({model});
            this.dbContext.Add(entity);
            await this.dbContext.SaveChangesAsync();
            return mapper.Map<{Model}>(entity);
        }}

        public async Task<{Model}> Update({modelIdType} {modelId}, {Model}Input {model})
        {{
            var entity = await this.Entity({modelId});
            if (entity != null)
            {{
                mapper.Map({model}, entity);
                await this.dbContext.SaveChangesAsync();
                return mapper.Map<{Model}>(entity);
            }}
            throw new KeyNotFoundException($""Cannot find {model} {{{modelId}.ToString()}}"");
        }}

        public async Task Delete({modelIdType} id)
        {{
            var entity = await this.Entity(id);
            if (entity != null)
            {{
                Entities.Remove(entity);
                await this.dbContext.SaveChangesAsync();
            }}
        }}

        public virtual async Task<bool> Has{Models}()
        {{
            return await Entities.CountAsync() > 0;
        }}

        public virtual async Task AddRange(IEnumerable<{Model}Input> {models})
        {{
            var entities = {models}.Select(i => mapper.Map<{Model}Entity>(i));
            this.dbContext.{Models}.AddRange(entities);
            await this.dbContext.SaveChangesAsync();
        }}

        private DbSet<{Model}Entity> Entities
        {{
            get
            {{
                return dbContext.{Models};
            }}
        }}

        private Task<{Model}Entity> Entity({modelIdType} {modelId})
        {{
            return Entities.Where(i => i.{ModelId} == {modelId}).FirstOrDefaultAsync();
        }}
    }}
}}";
        }
    }
}
