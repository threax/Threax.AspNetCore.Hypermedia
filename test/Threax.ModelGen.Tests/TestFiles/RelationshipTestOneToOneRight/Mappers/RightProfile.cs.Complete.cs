using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Threax.AspNetCore.Models;
using Threax.AspNetCore.Tracking;
using Test.InputModels;
using Test.Database;
using Test.ViewModels;
using System.Linq;

namespace Test.Mappers
{
    public partial class AppMapper
    {
        public RightEntity MapRight(RightInput src, RightEntity dest)
        {
            return mapper.Map(src, dest);
        }

        public Right MapRight(RightEntity src, Right dest)
        {
            return mapper.Map(src, dest);
        }

        public IQueryable<Right> ProjectRight(IQueryable<RightEntity> query)
        {
            return mapper.ProjectTo<Right>(query);
        }
    }

    public partial class RightProfile : Profile
    {
        public RightProfile()
        {
            //Map the input model to the entity
            MapInputToEntity(CreateMap<RightInput, RightEntity>());

            //Map the entity to the view model.
            MapEntityToView(CreateMap<RightEntity, Right>());
        }

        void MapInputToEntity(IMappingExpression<RightInput, RightEntity> mapExpr)
        {
            mapExpr.ForMember(d => d.RightId, opt => opt.Ignore())
                .ForMember(d => d.Created, opt => opt.MapFrom<ICreatedResolver>())
                .ForMember(d => d.Modified, opt => opt.MapFrom<IModifiedResolver>());
        }

        void MapEntityToView(IMappingExpression<RightEntity, Right> mapExpr)
        {
            
        }
    }
}