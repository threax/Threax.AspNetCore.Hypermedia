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
using Microsoft.AspNetCore.Authorization;

namespace {ns}.Controllers
{{
    public partial class {controller}Controller
    {{
        //Uncomment the line below and remove the [AllowAnonymous] to secure the view, you will probably have to define or change the role
        //[Authorize(Roles = Roles.Edit{Models}, AuthenticationSchemes = AuthCoreSchemes.Cookies)]
        [AllowAnonymous]
        public IActionResult {Models}()
        {{
            return View();
        }}
    }}
}}";
        }
    }
}
