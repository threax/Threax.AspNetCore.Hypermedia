using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen
{
    static class CrudInjectorGenerator
    {
        public static String Get(String modelName)
        {
            String Model, model;
            NameGenerator.CreatePascalAndCamel(modelName, out Model, out model);
            return Create(Model, model);
        }

        private static String Create(String Model, String model) {
            return
$@"import * as client from 'clientlibs.ServiceClient';
import * as hyperCrud from 'hr.widgets.HypermediaCrudService';
import * as di from 'hr.di';

//export class {Model}CrudInjector extends hyperCrud.AbstractHypermediaPageInjector {{
//    public static get InjectorArgs(): di.DiFunction<any>[] {{
//        return [client.EntryPointInjector];
//    }}
//
//    constructor(private injector: client.EntryPointInjector) {{
//        super();
//    }}
//
//    async list(query: any): Promise<hyperCrud.HypermediaCrudCollection> {{
//        var entry = await this.injector.load();
//        return entry.list{Model}s(query);
//    }}
//
//    async canList(): Promise<boolean> {{
//        var entry = await this.injector.load();
//        return entry.canList{Model}s();
//    }}
//
//    public getDeletePrompt(item: client.{Model}Result): string {{
//        return ""Are you sure you want to delete the {model}?"";
//    }}
//}}";
        }
    }
}
