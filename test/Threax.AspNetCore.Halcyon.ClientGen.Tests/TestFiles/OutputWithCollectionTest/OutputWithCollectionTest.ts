import * as hal from 'htmlrapier.halcyon/src/EndpointClient';
import { Fetcher } from 'htmlrapier/src/fetcher';

export class OutputCollectionResult {
    private client: hal.HalEndpointClient;

    constructor(client: hal.HalEndpointClient) {
        this.client = client;
    }

    private strongData: OutputCollection = undefined;
    public get data(): OutputCollection {
        this.strongData = this.strongData || this.client.GetData<OutputCollection>();
        return this.strongData;
    }

    private itemsStrong: OutputResult[];
    public get items(): OutputResult[] {
        if (this.itemsStrong === undefined) {
            var embeds = this.client.GetEmbed("values");
            var clients = embeds.GetAllClients();
            this.itemsStrong = [];
            for (var i = 0; i < clients.length; ++i) {
                this.itemsStrong.push(new OutputResult(clients[i]));
            }
        }
        return this.itemsStrong;
    }

    public save(data: Input): Promise<void> {
        return this.client.LoadLinkWithData("Save", data).then(hal.makeVoid);
    }

    public canSave(): boolean {
        return this.client.HasLink("Save");
    }

    public linkForSave(): hal.HalLink {
        return this.client.GetLink("Save");
    }

    public getSaveDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("Save", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasSaveDocs(): boolean {
        return this.client.HasLinkDoc("Save");
    }
}
export interface OutputCollection {
    offset?: number;
    limit?: number;
    total?: number;
}

export interface Input {
    /** The number of pages (item number = Offset * Limit) into the collection to query. */
    offset?: number;
    /** The limit of the number of items to return. */
    limit?: number;
}

export interface HalEndpointDocQuery {
    includeRequest?: boolean;
    includeResponse?: boolean;
}
