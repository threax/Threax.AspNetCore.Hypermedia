﻿using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Text;
using Threax.AspNetCore.Models;

namespace Threax.ModelGen
{
    static class UiControllerGenerator
    {
        public static String Get(String ns, String controller, String modelName, String modelPluralName, JsonSchema4 schema)
        {
            String Model, model;
            NameGenerator.CreatePascalAndCamel(modelName, out Model, out model);
            String Models, models;
            NameGenerator.CreatePascalAndCamel(modelPluralName, out Models, out models);

            var authAttribute = "";
            Object authName;
            if (schema.ExtensionData.TryGetValue(RequireAuthorizationAttribute.Name, out authName))
            {
                authAttribute = $@"[Authorize(Roles = {authName})]
        ";
            }
            return Create(ns, NameGenerator.CreatePascal(controller), Model, model, Models, models, authAttribute);
        }

        private static String Create(String ns, String controller, String Model, String model, String Models, String models, String authAttribute) {
            return
$@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace {ns}.Controllers
{{
    public partial class {controller}Controller
    {{
        {authAttribute}public IActionResult {Models}()
        {{
            return View();
        }}
    }}
}}";
        }
    }
}
