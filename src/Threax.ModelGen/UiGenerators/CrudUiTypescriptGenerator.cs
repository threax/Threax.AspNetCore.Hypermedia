using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen
{
    static class CrudUiTypescriptGenerator
    {
        public static String Get(String modelName)
        {
            return Create(NameGenerator.CreatePascal(modelName));
        }

        private static String Create(String Model) {
            return
$@"import * as standardCrudPage from 'hr.widgets.StandardCrudPage';
import * as startup from 'clientlibs.startup';
//import {{ {Model}CrudInjector }} from 'clientlibs.{Model}CrudInjector';

//var injector = {Model}CrudInjector;
//var builder = startup.createBuilder();
//standardCrudPage.addServices(builder, injector);
//standardCrudPage.createControllers(builder, new standardCrudPage.Settings());";
        }
    }
}
