using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen
{
    static class CrudInjectorGenerator
    {
        public static String Get(String modelName, String modelPluralName)
        {
            String Model, model;
            NameGenerator.CreatePascalAndCamel(modelName, out Model, out model);
            String Models, models;
            NameGenerator.CreatePascalAndCamel(modelPluralName, out Models, out models);
            return Create(Model, model, Models, models);
        }

        private static String Create(String Model, String model, String Models, String models) {
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
//        return entry.list{Models}(query);
//    }}
//
//    async canList(): Promise<boolean> {{
//        var entry = await this.injector.load();
//        return entry.canList{Models}();
//    }}
//
//    public getDeletePrompt(item: client.{Model}Result): string {{
//        return ""Are you sure you want to delete the {model}?"";
//    }}
//
//    public getItemId(item: client.{Model}Result): string | null {{
//        return String(item.data.{model}Id);
//    }}
//
//    public createIdQuery(id: string): client.{Model}Query | null {{
//        return {{
//            {model}Id: id
//        }};
//    }}
//}}";
        }
    }
}
