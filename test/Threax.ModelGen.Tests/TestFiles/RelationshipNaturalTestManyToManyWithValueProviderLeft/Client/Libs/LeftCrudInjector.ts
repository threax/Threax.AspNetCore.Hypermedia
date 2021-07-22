import * as client from 'Client/Libs/ServiceClient';
import * as hyperCrud from 'htmlrapier.widgets/src/HypermediaCrudService';
import * as di from 'htmlrapier/src/di';

//export class LeftCrudInjector extends hyperCrud.AbstractHypermediaPageInjector {
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
//        return entry.listLefts(query);
//    }
//
//    async canList(): Promise<boolean> {
//        var entry = await this.injector.load();
//        return entry.canListLefts();
//    }
//
//    public getDeletePrompt(item: client.LeftResult): string {
//        return "Are you sure you want to delete the left?";
//    }
//
//    public getItemId(item: client.LeftResult): string | null {
//        return String(item.data.leftId);
//    }
//
//    public createIdQuery(id: string): client.LeftQuery | null {
//        return {
//            leftId: id
//        };
//    }
//}