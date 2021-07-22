using Threax.NJsonSchema;
using System;
using System.Collections.Generic;
using System.Text;
using Threax.AspNetCore.Models;

namespace Threax.ModelGen
{
    public static class CrudUiTypescriptGenerator
    {
        public static String GetFileName(JsonSchema4 schema)
        {
            return $"Views/{schema.GetUiControllerName()}/{schema.GetPluralName()}.ts";
        }

        public static String Get(String modelName)
        {
            return Create(NameGenerator.CreatePascal(modelName));
        }

        private static String Create(String Model) {
            return
$@"import * as standardCrudPage from 'htmlrapier.widgets/src/StandardCrudPage';
import * as startup from 'Client/Libs/startup';
import * as deepLink from 'htmlrapier/src/deeplink';
//import {{ {Model}CrudInjector }} from 'Client/Libs/{Model}CrudInjector';

//var injector = {Model}CrudInjector;

//var builder = startup.createBuilder();
//deepLink.addServices(builder.Services);
//standardCrudPage.addServices(builder, injector);
//standardCrudPage.createControllers(builder, new standardCrudPage.Settings());";
        }
    }
}
