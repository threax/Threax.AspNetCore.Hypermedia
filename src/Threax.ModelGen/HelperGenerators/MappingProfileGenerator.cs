using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Threax.AspNetCore.Models;

namespace Threax.ModelGen
{
    static class MappingProfileGenerator
    {
        public static String Get(String ns, String modelName, bool hasCreated, bool hasModified)
        {
            String Model = NameGenerator.CreatePascal(modelName);
            return Create(ns, Model, hasCreated, hasModified);
        }

        private static String Create(String ns, String Model, bool hasCreated, bool hasModified)
        {
            return
$@"using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Threax.AspNetCore.Models;
using Threax.AspNetCore.Tracking;
using {ns}.InputModels;
using {ns}.Database;
using {ns}.ViewModels;

namespace {ns}.Mappers
{{
    public partial class {Model}Profile : Profile
    {{
        public {Model}Profile()
        {{
            //Map the input model to the entity
            MapInputToEntity(CreateMap<{Model}Input, {Model}Entity>());

            //Map the entity to the view model.
            MapEntityToView(CreateMap<{Model}Entity, {Model}>());
        }}

        partial void MapInputToEntity(IMappingExpression<{Model}Input, {Model}Entity> mapExpr);

        partial void MapEntityToView(IMappingExpression<{Model}Entity, {Model}> mapExpr);
    }}
}}";
        }

        public static String GetGenerated(String ns, JsonSchema4 schema, bool hasCreated, bool hasModified)
        {
            String Model = NameGenerator.CreatePascal(schema.Title);
            return CreateGenerated(ns, Model, hasCreated, hasModified, schema.Properties.Values);
        }

        private static String CreateGenerated(String ns, String Model, bool hasCreated, bool hasModified, IEnumerable<JsonProperty> props)
        {
            StringBuilder inputToEntityMaps = new StringBuilder($"mapExpr.ForMember(d => d.{Model}Id, opt => opt.Ignore())");
            StringBuilder entityToViewMaps = new StringBuilder("mapExpr");

            bool customEntityToView = false;
            foreach(var prop in props)
            {
                if (!prop.OnAllModelTypes())
                {
                    if(!prop.CreateInputModel() && prop.CreateEntity())
                    {
                        inputToEntityMaps.AppendLine();
                        inputToEntityMaps.Append($"                .ForMember(d => d.{NameGenerator.CreatePascal(prop.Name)}, opt => opt.Ignore())");
                    }

                    if (!prop.CreateEntity() && prop.CreateViewModel())
                    {
                        customEntityToView = true;
                        entityToViewMaps.AppendLine();
                        entityToViewMaps.Append($"                .ForMember(d => d.{NameGenerator.CreatePascal(prop.Name)}, opt => opt.Ignore())");
                    }
                }
            }

            if (hasCreated)
            {
                inputToEntityMaps.AppendLine();
                inputToEntityMaps.Append("                .ForMember(d => d.Created, opt => opt.ResolveUsing<ICreatedResolver>())");
            }

            if (hasModified)
            {
                inputToEntityMaps.AppendLine();
                inputToEntityMaps.Append("                .ForMember(d => d.Modified, opt => opt.ResolveUsing<IModifiedResolver>())");
            }

            inputToEntityMaps.Append(";");
            entityToViewMaps.Append(";");

            var entityToViews = "";
            if (customEntityToView) //Only take the string if we added customizations to it
            {
                entityToViews = entityToViewMaps.ToString();
            }

            return
$@"using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Threax.AspNetCore.Models;
using Threax.AspNetCore.Tracking;
using {ns}.InputModels;
using {ns}.Database;
using {ns}.ViewModels;

namespace {ns}.Mappers
{{
    public partial class {Model}Profile : Profile
    {{
        partial void MapInputToEntity(IMappingExpression<{Model}Input, {Model}Entity> mapExpr)
        {{
            {inputToEntityMaps.ToString()}
        }}

        partial void MapEntityToView(IMappingExpression<{Model}Entity, {Model}> mapExpr)
        {{
            {entityToViews}
        }}
    }}
}}";
        }
    }
}
