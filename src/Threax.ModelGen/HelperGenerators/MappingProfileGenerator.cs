using System;
using System.Collections.Generic;
using System.Text;

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

        public static String GetGenerated(String ns, String modelName, bool hasCreated, bool hasModified)
        {
            String Model = NameGenerator.CreatePascal(modelName);
            return CreateGenerated(ns, Model, hasCreated, hasModified);
        }

        private static String CreateGenerated(String ns, String Model, bool hasCreated, bool hasModified)
        {
            var additionalEntityMaps = "";
            if (hasCreated)
            {
                additionalEntityMaps += @"
                .ForMember(d => d.Created, opt => opt.ResolveUsing<ICreatedResolver>())";
            }

            if (hasModified)
            {
                additionalEntityMaps += @"
                .ForMember(d => d.Modified, opt => opt.ResolveUsing<IModifiedResolver>())";
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
            mapExpr.ForMember(d => d.{Model}Id, opt => opt.Ignore()){additionalEntityMaps};
        }}

        partial void MapEntityToView(IMappingExpression<{Model}Entity, {Model}> mapExpr)
        {{
            
        }}
    }}
}}";
        }
    }
}
