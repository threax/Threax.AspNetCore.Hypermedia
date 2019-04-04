using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Threax.AspNetCore.Models;
using Threax.AspNetCore.Tracking;
using Test.InputModels;
using Test.Database;
using Test.ViewModels;
using An.Extra.Namespace;
using Can.Be.Multiline;
using System.Linq;

namespace Test.Mappers
{
    public partial class AppMapper
    {
        public ValueEntity MapValue(ValueInput src, ValueEntity dest)
        {
            return mapper.Map(src, dest);
        }

        public Value MapValue(ValueEntity src, Value dest)
        {
            return mapper.Map(src, dest);
        }

        public IQueryable<Value> ProjectValue(IQueryable<ValueEntity> query)
        {
            return mapper.ProjectTo<Value>(query);
        }
    }

    public partial class ValueProfile : Profile
    {
        public ValueProfile()
        {
            //Map the input model to the entity
            MapInputToEntity(CreateMap<ValueInput, ValueEntity>());

            //Map the entity to the view model.
            MapEntityToView(CreateMap<ValueEntity, Value>());
        }

        partial void MapInputToEntity(IMappingExpression<ValueInput, ValueEntity> mapExpr);

        partial void MapEntityToView(IMappingExpression<ValueEntity, Value> mapExpr);
    }
}