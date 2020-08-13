using Threax.NJsonSchema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Threax.AspNetCore.Models;

namespace Threax.ModelGen
{
    public static class CrudCshtmlInjectorGenerator
    {
        public static String GetFileName(JsonSchema4 schema)
        {
            return $"Views/{schema.GetUiControllerName()}/{schema.GetPluralName()}.cshtml";
        }

        /// <summary>
        /// Create a cshtml view. OutDir must match the folder where the view will be located, use / between folders.
        /// This is important because it will be set as the runner and must match the typescript file.
        /// </summary>
        /// <param name="schema">The schema.</param>
        /// <param name="propertyNames">The property names.</param>
        /// <returns></returns>
        public static String Get(JsonSchema4 schema, IEnumerable<String> propertyNames = null)
        {
            String Model, model;
            NameGenerator.CreatePascalAndCamel(schema.Title, out Model, out model);
            String Models, models;
            NameGenerator.CreatePascalAndCamel(schema.GetPluralName(), out Models, out models);

            if (propertyNames == null)
            {
                propertyNames = new String[] { "Thing" };
            }

            return Create(Model, model, Models, models, propertyNames, schema.Properties.Values.Any(i => i.IsQueryableShowOnUi()));
        }

        private static String Create(String Model, String model, String Models, String models, IEnumerable<String> propertyNames, bool hasQuery) {
            var cls = hasQuery ? @" class=""col-sm-8""" : null;
            var sb = new StringBuilder(
$@"@{{
    ViewData[""Title""] = ""{Models}"";
}}

<div data-hr-controller=""mainTable""{cls}>
    <h1>{Models}</h1>
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
                sb.AppendLine($"                        <th>{NameGenerator.CreatePascal(name)}</th>");
            }
            sb.Append(

$@"                        <th>Edit</th>
                    </tr>
                </thead>
                <tbody data-hr-view=""listing"" data-hr-view-component=""mainTableBody""></tbody>
            </table>
            <template data-hr-component=""mainTableBody"">
                <table>
                    <tr>");

            sb.AppendLine();
            foreach (var name in propertyNames)
            {
                sb.AppendLine($"                        <td>{{{{{NameGenerator.CreateCamel(name)}}}}}</td>");
            }
            sb.Append(

$@"                        <td>
                            <button data-hr-on-click=""edit"" class=""btn btn-default"" data-hr-toggle=""edit"" data-hr-style-off=""display:none;"">Edit</button>
                            <button data-hr-on-click=""view"" class=""btn btn-default"" data-hr-toggle=""view"" data-hr-style-off=""display:none;"">View</button>
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
        An error occured loading the {models}. Please try again later.
    </error>
</div>");

            if (hasQuery)
            {
                sb.Append($@"
<div class=""col-sm-4"">
    <h3>Search</h3>
    <div data-hr-controller=""search"">
        <load visible=""true"">
            <p>Loading Search...</p>
        </load>
        <main>
            <form data-hr-on-submit=""submit"" data-hr-form=""input"">
                <p class=""bg-danger hiddenToggler"" data-hr-toggle=""mainError"" data-hr-view=""mainError"" data-hr-style-on=""display:block;"">{{{{message}}}}</p>

                <button type=""submit"" class=""btn btn-primary"" data-hr-form-end>Search</button>
            </form>
        </main>
        <error>
            An error occured loading the search form. Please try again later.
        </error>
    </div>
</div>");
            }

sb.Append($@"

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
