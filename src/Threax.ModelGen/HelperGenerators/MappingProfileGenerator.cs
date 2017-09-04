using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen
{
    static class MappingProfileGenerator
    {
        public static String Get(String ns, String modelName)
        {
            String Model = NameGenerator.CreatePascal(modelName);
            return Create(ns, Model);
        }

        private static String Create(String ns, String Model)
        {
            return
$@"using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Threax.AspNetCore.Models;
using {ns}.InputModels;
using {ns}.Database;
using {ns}.ViewModels;

namespace {ns}.Mappers
{{
    public partial class {Model}Mapper : Profile
    {{
        public {Model}Mapper()
        {{
            //Map the input model to the entity
            CreateMap<{Model}Input, {Model}Entity>()
                .ForMember(d => d.{Model}Id, opt => opt.Ignore());

            //Map the entity to the view model.
            CreateMap<{Model}Entity, {Model}>();
        }}
    }}
}}";
        }
    }
}
