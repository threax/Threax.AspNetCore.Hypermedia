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
    public partial class RightProfile : Profile
    {
        public RightProfile()
        {
            //Map the input model to the entity
            MapInputToEntity(CreateMap<RightInput, RightEntity>());

            //Map the entity to the view model.
            MapEntityToView(CreateMap<RightEntity, Right>());
        }

        partial void MapInputToEntity(IMappingExpression<RightInput, RightEntity> mapExpr);

        partial void MapEntityToView(IMappingExpression<RightEntity, Right> mapExpr);
    }
}