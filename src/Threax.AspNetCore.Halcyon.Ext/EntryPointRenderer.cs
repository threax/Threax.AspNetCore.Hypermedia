using Halcyon.HAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public class EntryPointRenderer<T> : IEntryPointRenderer<T> 
        where T : Controller
    {
        private static Lazy<JsonSerializer> serializer;

        static EntryPointRenderer()
        {
            serializer = new Lazy<JsonSerializer>(() => JsonSerializer.Create(HalcyonConvention.DefaultJsonSerializerSettings), LazyThreadSafetyMode.ExecutionAndPublication);
        }

        private readonly IHALConverter halConverter;
        private readonly T entryPointController;
        private readonly Func<T, object> getResult;

        public EntryPointRenderer(IHALConverter halConverter, T entryPointController, Func<T, Object> getResult)
        {
            this.halConverter = halConverter;
            this.entryPointController = entryPointController;
            this.getResult = getResult;
        }

        public void AddEntryPoint(Controller controller)
        {
            this.entryPointController.Url = controller.Url;
            this.entryPointController.ControllerContext = controller.ControllerContext;

            var entryPoint = getResult(entryPointController);
            if (!halConverter.CanConvert(entryPoint.GetType()))
            {
                throw new InvalidOperationException($"Cannot convert entry point class '{entryPoint.GetType().FullName}' to a hal result.");
            }
            var halEntryPoint = halConverter.Convert(entryPoint);
            controller.ViewData["EntryJson"] = JObject.FromObject(halEntryPoint, serializer.Value);
        }
    }
}
