using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Threax.ModelGen.ModelWriters
{
    internal class RoleWriter
    {
        public static void CreateRoles(IEnumerable<String> roleNames, GeneratorSettings generatorSettings)
        {
            var roleDefinitions = new StringBuilder();
            var databaseRoles = new StringBuilder();
            var roleAssignments = new StringBuilder();
            foreach (var roleName in roleNames)
            {
                roleDefinitions.AppendLine($"public const string {roleName} = \"{roleName}\";");
                databaseRoles.AppendLine($"yield return {roleName};");
                roleAssignments.AppendLine($"public bool {roleName} {{ get; set; }}");
            }

            var rolesFile = $@"using Halcyon.HAL.Attributes;
using {generatorSettings.AppNamespace}.Controllers.Api;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Threax.AspNetCore.Halcyon.Ext;
using Threax.AspNetCore.UserBuilder.Entities.Mvc;
using System.Linq;

namespace {generatorSettings.AppNamespace}
{{
    /// <summary>
    /// This class makes it easy to keep track of role constants throught the system.
    /// </summary>
    public static partial class Roles
    {{
        {roleDefinitions}

        /// <summary>
        /// All roles, any roles added above that you want to add to the database should be defined here.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<String> DatabaseRoles()
        {{
            {databaseRoles}
        }}
    }}

    [HalModel]
    [HalSelfActionLink(RolesControllerRels.GetUser, typeof(RolesController))]
    [HalActionLink(RolesControllerRels.SetUser, typeof(RolesController))]
    [HalActionLink(CrudRels.Update, RolesControllerRels.SetUser, typeof(RolesController))]
    [HalActionLink(CrudRels.Delete, RolesControllerRels.DeleteUser, typeof(RolesController))]
    public partial class RoleAssignments : ReflectedRoleAssignments
    {{
        {roleAssignments}
    }}
}}
";

            File.WriteAllText(Path.Combine(generatorSettings.AppOutDir, "Roles.cs"), rolesFile);
        }
    }
}
