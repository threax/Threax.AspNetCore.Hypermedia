using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Text;
using Threax.AspNetCore.Models;

namespace Threax.ModelGen
{
    static class CrudInjectorGenerator
    {
        public static String Get(JsonSchema4 schema)
        {
            String Model, model;
            NameGenerator.CreatePascalAndCamel(schema.Title, out Model, out model);
            String Models, models;
            NameGenerator.CreatePascalAndCamel(schema.GetPluralName(), out Models, out models);
            return Create(Model, model, Models, models, NameGenerator.CreateCamel(schema.GetKeyName()));
        }

        private static String Create(String Model, String model, String Models, String models, String modelId) {
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
//        return String(item.data.{modelId});
//    }}
//
//    public createIdQuery(id: string): client.{Model}Query | null {{
//        return {{
//            {modelId}: id
//        }};
//    }}
//}}";
        }
    }
}
