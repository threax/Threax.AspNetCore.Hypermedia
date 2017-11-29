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
        public {Model}Profile()
        {{
            //Map the input model to the entity
            CreateMap<{Model}Input, {Model}Entity>()
                .ForMember(d => d.{Model}Id, opt => opt.Ignore()){additionalEntityMaps};

            //Map the entity to the view model.
            CreateMap<{Model}Entity, {Model}>();
        }}
    }}
}}";
        }
    }
}
