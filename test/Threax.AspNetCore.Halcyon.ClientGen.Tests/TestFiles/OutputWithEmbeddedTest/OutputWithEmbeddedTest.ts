import * as hal from 'htmlrapier.halcyon/src/EndpointClient';
import { Fetcher } from 'htmlrapier/src/fetcher';

export class OutputResult {
    private client: hal.HalEndpointClient;

    constructor(client: hal.HalEndpointClient) {
        this.client = client;
    }

    private strongData: Output = undefined;
    public get data(): Output {
        this.strongData = this.strongData || this.client.GetData<Output>();
        return this.strongData;
    }

    private embedsStrong: EmbeddedObjectResult[];
    public get embeds(): EmbeddedObjectResult[] {
        if (this.embedsStrong === undefined) {
            var embeds = this.client.GetEmbed("embeds");
            var clients = embeds.GetAllClients();
            this.embedsStrong = [];
            for (var i = 0; i < clients.length; ++i) {
                this.embedsStrong.push(new EmbeddedObjectResult(clients[i]));
            }
        }
        return this.embedsStrong;
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
export interface EmbeddedObject {
    value?: number;
}

export interface Output {
}

export interface Input {
}

export interface HalEndpointDocQuery {
    includeRequest?: boolean;
    includeResponse?: boolean;
}
