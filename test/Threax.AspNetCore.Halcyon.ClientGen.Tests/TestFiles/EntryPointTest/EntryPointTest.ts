import * as hal from 'htmlrapier.halcyon/src/EndpointClient';
import { Fetcher } from 'htmlrapier/src/fetcher';

export class EntryPointInjector {
    private instancePromise: Promise<EntryPointResult>;

    constructor(private url: string, private fetcher: Fetcher, private data?: any) {}

    public load(): Promise<EntryPointResult> {
        if (!this.instancePromise) {
            if (this.data) {
                this.instancePromise = Promise.resolve(new EntryPointResult(new hal.HalEndpointClient(this.data, this.fetcher)));
            }
            else {
                this.instancePromise = EntryPointResult.Load(this.url, this.fetcher);
            }
        }
        return this.instancePromise;
    }
}

export class EntryPointResult {
    private client: hal.HalEndpointClient;

    public static Load(url: string, fetcher: Fetcher): Promise<EntryPointResult> {
        return hal.HalEndpointClient.Load({
            href: url,
            method: "GET"
        }, fetcher)
            .then(c => {
                 return new EntryPointResult(c);
             });
            }

    constructor(client: hal.HalEndpointClient) {
        this.client = client;
    }

    private strongData: EntryPoint = undefined;
    public get data(): EntryPoint {
        this.strongData = this.strongData || this.client.GetData<EntryPoint>();
        return this.strongData;
    }

    public refresh(): Promise<EntryPointResult> {
        return this.client.LoadLink("self")
               .then(r => {
                    return new EntryPointResult(r);
                });

    }

    public canRefresh(): boolean {
        return this.client.HasLink("self");
    }

    public linkForRefresh(): hal.HalLink {
        return this.client.GetLink("self");
    }

    public getRefreshDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("self", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasRefreshDocs(): boolean {
        return this.client.HasLinkDoc("self");
    }
}
export interface EntryPoint {
}

export interface HalEndpointDocQuery {
    includeRequest?: boolean;
    includeResponse?: boolean;
}
