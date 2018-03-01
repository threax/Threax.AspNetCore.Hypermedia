import * as hal from 'hr.halcyon.EndpointClient';

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

    public save(data: Input): Promise<void> {
        return this.client.LoadLinkWithBody("Save", data).then(hal.makeVoid);
    }

    public canSave(): boolean {
        return this.client.HasLink("Save");
    }

    public linkForSave(): hal.HalLink {
        return this.client.GetLink("Save");
    }

    public getSaveDocs(): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("Save")
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasSaveDocs(): boolean {
        return this.client.HasLinkDoc("Save");
    }
}
//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v9.10.10.0 (Newtonsoft.Json v10.0.0.0) (http://NJsonSchema.org)
// </auto-generated>
//----------------------





export interface Output {
}

export interface Input {
    Date?: string;
}
