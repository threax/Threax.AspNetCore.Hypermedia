using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen
{
    static class UiControllerGenerator
    {
        public static String Get(String ns, String controller, String modelName, String modelPluralName)
        {
            String Model, model;
            NameGenerator.CreatePascalAndCamel(modelName, out Model, out model);
            String Models, models;
            NameGenerator.CreatePascalAndCamel(modelPluralName, out Models, out models);
            return Create(ns, NameGenerator.CreatePascal(controller), Model, model, Models, models);
        }

        private static String Create(String ns, String controller, String Model, String model, String Models, String models) {
            return
$@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace {ns}.Controllers
{{
    public partial class {controller}Controller
    {{
        //[Authorize(Roles = Roles.Edit{Models})] //Uncomment this to secure the view, you will probably have to define the role
        public IActionResult {Models}()
        {{
            return View();
        }}
    }}
}}";
        }
    }
}
