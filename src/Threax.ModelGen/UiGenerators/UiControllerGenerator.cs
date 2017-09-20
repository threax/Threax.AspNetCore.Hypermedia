using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen
{
    static class UiControllerGenerator
    {
        public static String Get(String ns, String controller, String modelName)
        {
            String Model, model;
            NameGenerator.CreatePascalAndCamel(modelName, out Model, out model);
            return Create(ns, NameGenerator.CreatePascal(controller), Model, model);
        }

        private static String Create(String ns, String controller, String Model, String model) {
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
        public IActionResult {Model}s()
        {{
            return View();
        }}
    }}
}}";
        }
    }
}
