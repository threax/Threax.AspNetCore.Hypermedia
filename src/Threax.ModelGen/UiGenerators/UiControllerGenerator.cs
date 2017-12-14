using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Text;
using Threax.AspNetCore.Models;

namespace Threax.ModelGen
{
    public static class UiControllerGenerator
    {
        public static String Get(JsonSchema4 schema, String ns)
        {
            String Model, model;
            NameGenerator.CreatePascalAndCamel(schema.Title, out Model, out model);
            String Models, models;
            NameGenerator.CreatePascalAndCamel(schema.GetPluralName(), out Models, out models);

            var authAttribute = "";
            String authName = schema.GetAuthorizationRoleString();
            if (authName != null)
            {
                authAttribute = $@"[Authorize(Roles = {authName})]
        ";
            }
            return Create(ns, NameGenerator.CreatePascal(schema.GetUiControllerName()), Model, model, Models, models, authAttribute);
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
