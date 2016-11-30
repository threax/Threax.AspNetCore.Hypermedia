using Halcyon.HAL;
using Halcyon.HAL.Attributes;
using Halcyon.Web.HAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing.Template;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public class HalModelResultFilterAttribute : ResultFilterAttribute
    {
        private IUrlHelperFactory urlHelperFactory;
        private IActionContextAccessor actionContextAccessor;
        private IHALConverter halConverter;
        private IHalModelViewMapper modelViewMapper;

        public HalModelResultFilterAttribute(IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor, IHalModelViewMapper modelViewMapper, IHALConverter halConverter)
        {
            this.urlHelperFactory = urlHelperFactory;
            this.actionContextAccessor = actionContextAccessor;
            this.halConverter = halConverter;
            this.modelViewMapper = modelViewMapper;
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            //Controller must extend ControllerBase
            if (context.Controller is ControllerBase)
            {
                var objResult = context.Result as ObjectResult;
                if (objResult != null)
                {
                    HALResponse halResponse = objResult.Value as HALResponse;
                    if(halResponse == null)
                    {
                        var convertedObject = modelViewMapper.Convert(objResult.Value);
                        halResponse = halConverter.Convert(convertedObject);
                    }
                    if (halResponse != null)
                    {
                        context.Result = halResponse.ToActionResult(context.Controller as ControllerBase);
                    }
                }
            }
        }
    }
}