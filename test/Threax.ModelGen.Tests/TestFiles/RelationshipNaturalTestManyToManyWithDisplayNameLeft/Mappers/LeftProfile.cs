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
    public partial class LeftProfile : Profile
    {
        public LeftProfile()
        {
            //Map the input model to the entity
            MapInputToEntity(CreateMap<LeftInput, LeftEntity>());

            //Map the entity to the view model.
            MapEntityToView(CreateMap<LeftEntity, Left>());
        }

        partial void MapInputToEntity(IMappingExpression<LeftInput, LeftEntity> mapExpr);

        partial void MapEntityToView(IMappingExpression<LeftEntity, Left> mapExpr);
    }
}