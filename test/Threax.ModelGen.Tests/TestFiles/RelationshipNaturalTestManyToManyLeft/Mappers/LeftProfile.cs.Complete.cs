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
        public LeftEntity MapLeft(LeftInput src, LeftEntity dest)
        {
            return mapper.Map(src, dest);
        }

        public Left MapLeft(LeftEntity src, Left dest)
        {
            return mapper.Map(src, dest);
        }
    }

    public partial class LeftProfile : Profile
    {
        public LeftProfile()
        {
            //Map the input model to the entity
            MapInputToEntity(CreateMap<LeftInput, LeftEntity>());

            //Map the entity to the view model.
            MapEntityToView(CreateMap<LeftEntity, Left>());
        }

        partial void MapInputToEntity(IMappingExpression<LeftInput, LeftEntity> mapExpr)
        {
            mapExpr.ForMember(d => d.LeftId, opt => opt.Ignore())
                .ForMember(d => d.Created, opt => opt.MapFrom<ICreatedResolver>())
                .ForMember(d => d.Modified, opt => opt.MapFrom<IModifiedResolver>());
        }

        partial void MapEntityToView(IMappingExpression<LeftEntity, Left> mapExpr)
        {
            
        }
    }
}