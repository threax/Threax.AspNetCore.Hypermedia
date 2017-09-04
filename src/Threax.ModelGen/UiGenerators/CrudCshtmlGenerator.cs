using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen
{
    static class CrudCshtmlInjectorGenerator
    {
        /// <summary>
        /// Create a cshtml view. OutDir must match the folder where the view will be located, use / between folders.
        /// This is important because it will be set as the runner and must match the typescript file.
        /// </summary>
        /// <param name="modelName">The name of the model.</param>
        /// <param name="outDir">The directory the view will be placed in.</param>
        /// <returns></returns>
        public static String Get(String modelName, String outDir = "Views/Home", IEnumerable<String> propertyNames = null)
        {
            String Model, model;
            NameGenerator.CreatePascalAndCamel(modelName, out Model, out model);

            if(propertyNames == null)
            {
                propertyNames = new String[] { "Thing" };
            }

            return Create(Model, model, outDir, propertyNames);
        }

        private static String Create(String Model, String model, String outDir, IEnumerable<String> propertyNames) {
            var sb = new StringBuilder(
$@"@{{
    ViewData[""Title""] = ""{Model}s"";
}}

<h1 data-hr-run=""{outDir}/{Model}s"">{Model}s</h1>
<div data-hr-controller=""mainTable"">
    <load visible=""true"">
        <p>Working...</p>
    </load>
    <main>
        <div class=""table-responsive"">
            <table class=""table table-bordered"">
                <thead>
                    <tr>");

            sb.AppendLine();
            foreach(var name in propertyNames)
            {
                sb.AppendLine($"<th>{NameGenerator.CreatePascal(name)}</th>");
            }
            sb.Append(

$@"                        <th>Edit</th>
                    </tr>
                </thead>
                <tbody data-hr-model=""listing"" data-hr-model-component=""mainTableBody""></tbody>
            </table>
            <template data-hr-component=""mainTableBody"">
                <table>
                    <tr>");

            sb.AppendLine();
            foreach (var name in propertyNames)
            {
                sb.AppendLine($"<td>{{{{{NameGenerator.CreateCamel(name)}}}}}</td>");
            }
            sb.Append(

$@"                        <td>
                            <button data-hr-on-click=""edit"" class=""btn btn-default"" data-hr-toggle=""edit"" data-hr-style-off=""display:none;"">Edit</button>
                            <button data-hr-on-click=""del"" class=""btn btn-default"" data-hr-toggle=""del"" data-hr-style-off=""display:none;"">Delete</button>
                        </td>
                    </tr>
                </table>
            </template>
        </div>
        <button data-hr-on-click=""add"" class=""btn btn-default"" data-hr-toggle=""add"" data-hr-style-off=""display:none;"">Add</button>

        <page-numbers></page-numbers>
    </main>

    <error>
        An error occured loading the {model}s. Please try again later.
    </error>
</div>

<modal data-hr-controller=""entryEditor"" title-text=""{Model}"" dialog-classes=""modal-lg"">
    <load class=""modal-body"">
        <p>Working...</p>
    </load>
    <main>
        <item-edit-form class=""form-horizontal"" data-hr-form-component=""hr.forms.horizontal""></item-edit-form>
    </main>
    <error class=""modal-body"">
        <p>An error occured loading the value. Please try again later.</p>
    </error>
</modal>");

            return sb.ToString();
        }
    }
}
