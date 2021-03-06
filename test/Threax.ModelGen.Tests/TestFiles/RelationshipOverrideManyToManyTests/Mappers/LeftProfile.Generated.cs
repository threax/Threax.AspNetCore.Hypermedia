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
        partial void MapInputToEntity(IMappingExpression<LeftInput, LeftEntity> mapExpr)
        {
            mapExpr.ForMember(d => d.LeftId, opt => opt.Ignore())
                .ForMember(d => d.Created, opt => opt.MapFrom<ICreatedResolver>())
                .ForMember(d => d.Modified, opt => opt.MapFrom<IModifiedResolver>());
        }

        partial void MapEntityToView(IMappingExpression<LeftEntity, Left> mapExpr)
        {
            mapExpr
                .ForMember(d => d.RightIds, opt => opt.Ignore());
        }
    }
}