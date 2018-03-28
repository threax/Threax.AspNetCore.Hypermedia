using System;
using System.Collections.Generic;
using System.Text;
using Threax.AspNetCore.Tests;
using NJsonSchema.Generation;
using NJsonSchema;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;

namespace Threax.AspNetCore.Halcyon.Ext.Tests
{
    public static class Mocks
    {
        /// <summary>
        /// Setup global shared mockups that can be used across may tests.
        /// </summary>
        /// <param name="mockup">The mockup class to configure.</param>
        /// <returns>The passed in mockup test.</returns>
        public static Mockup SetupGlobal(this Mockup mockup)
        {
            mockup.Add<JsonSchemaGenerator>(s => new JsonSchemaGenerator(HalcyonConvention.DefaultJsonSchemaGeneratorSettings));

            mockup.Add<JsonSerializerSettings>(s =>
            {
                var settings = HalcyonConvention.DefaultJsonSerializerSettings;
                settings.Formatting = Formatting.Indented; //Use indented for tests, makes test files easier to read
                return settings;
            });

            mockup.Add<JsonSerializer>(s => JsonSerializer.Create(s.Get<JsonSerializerSettings>()));

            return mockup;
        }
    }
}
