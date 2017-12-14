import * as client from 'clientlibs.ServiceClient';
import * as hyperCrud from 'hr.widgets.HypermediaCrudService';
import * as di from 'hr.di';

//export class ValueCrudInjector extends hyperCrud.AbstractHypermediaPageInjector {
//    public static get InjectorArgs(): di.DiFunction<any>[] {
//        return [client.EntryPointInjector];
//    }
//
//    constructor(private injector: client.EntryPointInjector) {
//        super();
//    }
//
//    async list(query: any): Promise<hyperCrud.HypermediaCrudCollection> {
//        var entry = await this.injector.load();
//        return entry.listValues(query);
//    }
//
//    async canList(): Promise<boolean> {
//        var entry = await this.injector.load();
//        return entry.canListValues();
//    }
//
//    public getDeletePrompt(item: client.ValueResult): string {
//        return "Are you sure you want to delete the value?";
//    }
//
//    public getItemId(item: client.ValueResult): string | null {
//        return String(item.data.valueId);
//    }
//
//    public createIdQuery(id: string): client.ValueQuery | null {
//        return {
//            valueId: id
//        };
//    }
//}