using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Threax.AspNetCore.Models;

namespace Threax.ModelGen
{
    public static class MappingProfileGenerator
    {
        public static String GetFileName(JsonSchema4 schema, bool generated)
        {
            var genStr = generated ? ".Generated" : "";
            return $"Mappers/{schema.Title}Profile{genStr}.cs";
        }

        public static String Get(JsonSchema4 schema, String ns, bool generated)
        {
            String Model = NameGenerator.CreatePascal(schema.Title);
            return Create(ns, Model, NameGenerator.CreatePascal(schema.GetKeyName()), schema.AllowCreated(), schema.AllowModified(), generated, schema.Properties.Values, schema.GetExtraNamespaces(StrConstants.FileNewline));
        }

        private static String Create(String ns, String Model, String ModelId, bool hasCreated, bool hasModified, bool generated, IEnumerable<JsonProperty> props, String additionalNs)
        {
            String inputToEntity = null;
            String entityToViews = null;
            if (!generated)
            {
                CreateMaps(ModelId, hasCreated, hasModified, props, out inputToEntity, out entityToViews);
            }

            var result = 
$@"using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Threax.AspNetCore.Models;
using Threax.AspNetCore.Tracking;
using {ns}.InputModels;
using {ns}.Database;
using {ns}.ViewModels;{additionalNs}

namespace {ns}.Mappers
{{
    public partial class AppMapper
    {{
        public {Model}Entity Map{Model}({Model}Input src, {Model}Entity dest)
        {{
            return mapper.Map(src, dest);
        }}

        public {Model} Map{Model}({Model}Entity src, {Model} dest)
        {{
            return mapper.Map(src, dest);
        }}
    }}

    public partial class {Model}Profile : Profile
    {{
        public {Model}Profile()
        {{
            //Map the input model to the entity
            MapInputToEntity(CreateMap<{Model}Input, {Model}Entity>());

            //Map the entity to the view model.
            MapEntityToView(CreateMap<{Model}Entity, {Model}>());
        }}
";
            if (generated)
            {
                result += $@"
        partial void MapInputToEntity(IMappingExpression<{Model}Input, {Model}Entity> mapExpr);

        partial void MapEntityToView(IMappingExpression<{Model}Entity, {Model}> mapExpr);";
            }
            else
            {
                result += $@"
        partial void MapInputToEntity(IMappingExpression<{Model}Input, {Model}Entity> mapExpr)
        {{
            {inputToEntity}
        }}

        partial void MapEntityToView(IMappingExpression<{Model}Entity, {Model}> mapExpr)
        {{
            {entityToViews}
        }}";
            }
            result += $@"
    }}
}}";
            return result;
        }

        public static String GetGenerated(JsonSchema4 schema, String ns)
        {
            String Model = NameGenerator.CreatePascal(schema.Title);
            return CreateGenerated(ns, Model, NameGenerator.CreatePascal(schema.GetKeyName()), schema.AllowCreated(), schema.AllowModified(), schema.Properties.Values, schema.GetExtraNamespaces(StrConstants.FileNewline));
        }

        private static String CreateGenerated(String ns, String Model, String ModelId, bool hasCreated, bool hasModified, IEnumerable<JsonProperty> props, String additionalNs)
        {
            CreateMaps(ModelId, hasCreated, hasModified, props, out var inputToEntity, out var entityToViews);

            return
$@"using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Threax.AspNetCore.Models;
using Threax.AspNetCore.Tracking;
using {ns}.InputModels;
using {ns}.Database;
using {ns}.ViewModels;{additionalNs}

namespace {ns}.Mappers
{{
    public partial class {Model}Profile : Profile
    {{
        partial void MapInputToEntity(IMappingExpression<{Model}Input, {Model}Entity> mapExpr)
        {{
            {inputToEntity}
        }}

        partial void MapEntityToView(IMappingExpression<{Model}Entity, {Model}> mapExpr)
        {{
            {entityToViews}
        }}
    }}
}}";
        }

        private static void CreateMaps(string ModelId, bool hasCreated, bool hasModified, IEnumerable<JsonProperty> props, out string inputToEntity, out string entityToViews)
        {
            StringBuilder inputToEntityMaps = new StringBuilder($"mapExpr.ForMember(d => d.{ModelId}, opt => opt.Ignore())");
            StringBuilder entityToViewMaps = new StringBuilder("mapExpr");

            bool customEntityToView = false;
            foreach (var prop in props)
            {
                if (!prop.OnAllModelTypes())
                {
                    if (!prop.CreateInputModel() && prop.CreateEntity())
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
                inputToEntityMaps.Append("                .ForMember(d => d.Created, opt => opt.MapFrom<ICreatedResolver>())");
            }

            if (hasModified)
            {
                inputToEntityMaps.AppendLine();
                inputToEntityMaps.Append("                .ForMember(d => d.Modified, opt => opt.MapFrom<IModifiedResolver>())");
            }

            inputToEntityMaps.Append(";");
            entityToViewMaps.Append(";");

            entityToViews = "";
            if (customEntityToView) //Only take the string if we added customizations to it
            {
                entityToViews = entityToViewMaps.ToString();
            }
            inputToEntity = inputToEntityMaps.ToString();
        }
    }
}
