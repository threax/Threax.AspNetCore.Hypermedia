using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Threax.AspNetCore.Models;
using Threax.AspNetCore.Tracking;
using Test.InputModels;
using Test.Database;
using Test.ViewModels;

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

        partial void MapInputToEntity(IMappingExpression<ValueInput, ValueEntity> mapExpr)
        {
            mapExpr.ForMember(d => d.CrazyKey, opt => opt.Ignore())
                .ForMember(d => d.Created, opt => opt.MapFrom<ICreatedResolver>())
                .ForMember(d => d.Modified, opt => opt.MapFrom<IModifiedResolver>());
        }

        partial void MapEntityToView(IMappingExpression<ValueEntity, Value> mapExpr)
        {
            
        }
    }
}