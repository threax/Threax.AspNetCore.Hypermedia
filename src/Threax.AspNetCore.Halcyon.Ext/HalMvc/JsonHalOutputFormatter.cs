﻿using Halcyon.HAL;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
#if NET6_0
using JsonOutputFormatter = Microsoft.AspNetCore.Mvc.Formatters.NewtonsoftJsonOutputFormatter;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Microsoft.AspNetCore.Mvc;
#endif

namespace Halcyon.Web.HAL.Json
{
    public class JsonHalOutputFormatter : IOutputFormatter
    {
        public const string HalJsonType = "application/hal+json";

        private readonly IEnumerable<string> halJsonMediaTypes;
        private readonly JsonOutputFormatter jsonFormatter;
        private readonly JsonSerializerSettings serializerSettings;

        public JsonHalOutputFormatter(
#if NET6_0
        MvcOptions mvcOpt,
#endif
        IEnumerable<string> halJsonMediaTypes = null)
        {
            if (halJsonMediaTypes == null) halJsonMediaTypes = new string[] { HalJsonType };

            serializerSettings = JsonSerializerSettingsProvider.CreateSerializerSettings();

            this.jsonFormatter = new JsonOutputFormatter(serializerSettings, ArrayPool<Char>.Create()
#if NET6_0
            , mvcOpt, null
#endif
            );

            this.halJsonMediaTypes = halJsonMediaTypes;
        }
        public JsonHalOutputFormatter(JsonSerializerSettings serializerSettings
#if NET6_0
        , MvcOptions mvcOpt
#endif
            , IEnumerable<string> halJsonMediaTypes = null)
        {
            if (halJsonMediaTypes == null) halJsonMediaTypes = new string[] { HalJsonType };

            this.serializerSettings = serializerSettings;
            this.jsonFormatter = new JsonOutputFormatter(serializerSettings, ArrayPool<Char>.Create()
#if NET6_0
            , mvcOpt, null
#endif
            );

            this.halJsonMediaTypes = halJsonMediaTypes;
        }

        public bool CanWriteResult(OutputFormatterCanWriteContext context)
        {
            return context.ObjectType == typeof(HALResponse);
        }

        public async Task WriteAsync(OutputFormatterWriteContext context)
        {
            string mediaType = context.ContentType.HasValue ? context.ContentType.Value : null;

            object value = null;
            var halResponse = ((HALResponse)context.Object);

            // If it is a HAL response but set to application/json - convert to a plain response
            var serializer = JsonSerializer.Create(serializerSettings);

            if (!halResponse.Config.ForceHAL && !halJsonMediaTypes.Contains(mediaType))
            {
                value = halResponse.ToPlainResponse(serializer);
            }
            else
            {
                value = halResponse.ToJObject(serializer);
            }

            var jsonContext = new OutputFormatterWriteContext(context.HttpContext, context.WriterFactory, value.GetType(), value);
            jsonContext.ContentType = context.ContentType;

            await jsonFormatter.WriteAsync(jsonContext);
        }
    }
}
