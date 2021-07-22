import * as client from 'Client/Libs/ServiceClient';
import * as hyperCrud from 'htmlrapier.widgets/src/HypermediaCrudService';
import * as di from 'htmlrapier/src/di';

//export class RightCrudInjector extends hyperCrud.AbstractHypermediaPageInjector {
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
//        return entry.listRights(query);
//    }
//
//    async canList(): Promise<boolean> {
//        var entry = await this.injector.load();
//        return entry.canListRights();
//    }
//
//    public getDeletePrompt(item: client.RightResult): string {
//        return "Are you sure you want to delete the right?";
//    }
//
//    public getItemId(item: client.RightResult): string | null {
//        return String(item.data.rightId);
//    }
//
//    public createIdQuery(id: string): client.RightQuery | null {
//        return {
//            rightId: id
//        };
//    }
//}