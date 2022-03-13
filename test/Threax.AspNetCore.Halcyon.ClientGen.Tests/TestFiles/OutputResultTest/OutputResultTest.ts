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

    public save(data: Input): Promise<OutputResult> {
        return this.client.LoadLinkWithData("Save", data)
               .then(r => {
                    return new OutputResult(r);
                });

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
export interface Output {
}

export interface Input {
}

export interface HalEndpointDocQuery {
    includeRequest?: boolean;
    includeResponse?: boolean;
}
