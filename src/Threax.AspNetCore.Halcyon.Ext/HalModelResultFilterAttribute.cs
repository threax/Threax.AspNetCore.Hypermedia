using Halcyon.HAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public class HalModelResultFilterAttribute : ResultFilterAttribute
    {
        private IHALConverter halConverter;

        public HalModelResultFilterAttribute(IHALConverter halConverter)
        {
            this.halConverter = halConverter;
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var objResult = context.Result as ObjectResult;
            if (objResult != null)
            {
                HALResponse halResponse = objResult.Value as HALResponse;
                if(halResponse == null && !(objResult.Value is String))
                {
                    halResponse = halConverter.Convert(objResult.Value);
                }
                if (halResponse != null)
                {
                    context.Result = new ObjectResult(halResponse)
                    {
                        StatusCode = (int)HttpStatusCode.OK
                    };
                }
            }
        }
    }
}